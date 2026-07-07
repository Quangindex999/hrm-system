using payroll_service.DTOs;
using payroll_service.Models;
using payroll_service.Repositories;

namespace payroll_service.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _repository;
        private readonly IHrServiceClient _hrClient;
        private readonly IEmailService _emailService;
        private readonly ILogger<PayrollService> _logger;

        // ── Tỷ lệ bảo hiểm ───────────────────────────────────────────
        private const decimal BHXH_NV = 0.08m;
        private const decimal BHYT_NV = 0.015m;
        private const decimal BHTN_NV = 0.01m;
        private const decimal BHXH_DN = 0.175m;
        private const decimal BHYT_DN = 0.03m;
        private const decimal BHTN_DN = 0.01m;

        // ── Giảm trừ gia cảnh ────────────────────────────────────────
        private const decimal GTGC_BANTH = 11_000_000m;
        private const decimal GTGC_NGUOI_PHU_THUOC = 4_400_000m;
        private const decimal TRAN_BHTN = 20 * 1_490_000m;  // 20× lương cơ sở vùng

        public PayrollService(
            IPayrollRepository repository,
            IHrServiceClient hrClient,
            IEmailService emailService,
            ILogger<PayrollService> logger)
        {
            _repository = repository;
            _hrClient = hrClient;
            _emailService = emailService;
            _logger = logger;
        }

        // ════════════════════════════════════════════════════════════════
        // CORE CALCULATION
        // ════════════════════════════════════════════════════════════════
        private SalaryCalcResult CalcFullSalary(
            decimal contractBasicSalary,
            decimal salaryRatio,
            int workingDays,
            int paidLeaveDays,
            int standardDays,
            decimal overtimePay,
            decimal bonus,
            decimal otherDeduction,
            int dependentCount,
            bool isSocialInsuranceSubject,
            string taxType)
        {
            if (standardDays <= 0) standardDays = 26;

            // 1. Lương theo ngày công thực tế
            decimal dailySalary = contractBasicSalary / standardDays;
            decimal actualSalary = dailySalary * (workingDays + paidLeaveDays) * salaryRatio;

            // 2. Tổng thu nhập gộp
            decimal grossIncome = actualSalary + overtimePay + bonus;

            // 3. Bảo hiểm
            decimal bhxhNv = 0, bhytNv = 0, bhtnNv = 0;
            decimal bhxhDn = 0, bhytDn = 0, bhtnDn = 0;

            if (isSocialInsuranceSubject)
            {
                decimal insBase = contractBasicSalary * salaryRatio;
                decimal bhtnBase = Math.Min(insBase, TRAN_BHTN);

                bhxhNv = Math.Round(insBase * BHXH_NV, 0);
                bhytNv = Math.Round(insBase * BHYT_NV, 0);
                bhtnNv = Math.Round(bhtnBase * BHTN_NV, 0);
                bhxhDn = Math.Round(insBase * BHXH_DN, 0);
                bhytDn = Math.Round(insBase * BHYT_DN, 0);
                bhtnDn = Math.Round(bhtnBase * BHTN_DN, 0);
            }

            decimal totalBhNv = bhxhNv + bhytNv + bhtnNv;

            // 4. Giảm trừ gia cảnh
            decimal dependentDeduction = dependentCount * GTGC_NGUOI_PHU_THUOC;

            // 5. Thu nhập chịu thuế
            decimal taxableIncome = Math.Max(0,
                grossIncome - totalBhNv - GTGC_BANTH - dependentDeduction - otherDeduction);

            // 6. Thuế TNCN
            decimal personalTax = taxType.Equals("Flat10", StringComparison.OrdinalIgnoreCase)
                ? Math.Round(grossIncome * 0.10m, 0)
                : CalcProgressiveTax(taxableIncome);

            // 7. Net
            decimal totalDeduction = totalBhNv + personalTax + otherDeduction;
            decimal finalSalary = Math.Max(0, grossIncome - totalDeduction);

            return new SalaryCalcResult
            {
                ActualSalary = Math.Round(actualSalary, 0),
                GrossIncome = Math.Round(grossIncome, 0),
                BhxhNv = bhxhNv,
                BhytNv = bhytNv,
                BhtnNv = bhtnNv,
                BhxhDn = bhxhDn,
                BhytDn = bhytDn,
                BhtnDn = bhtnDn,
                DependentDeduction = dependentDeduction,
                TaxableIncome = Math.Round(taxableIncome, 0),
                PersonalTax = personalTax,
                TotalDeduction = Math.Round(totalDeduction, 0),
                FinalSalary = Math.Round(finalSalary, 0)
            };
        }

        private static decimal CalcProgressiveTax(decimal income)
        {
            if (income <= 0) return 0;
            decimal tax = 0;
            var brackets = new (decimal from, decimal to, decimal rate)[]
            {
                (0,           5_000_000,        0.05m),
                (5_000_000,   10_000_000,       0.10m),
                (10_000_000,  18_000_000,       0.15m),
                (18_000_000,  32_000_000,       0.20m),
                (32_000_000,  52_000_000,       0.25m),
                (52_000_000,  80_000_000,       0.30m),
                (80_000_000,  decimal.MaxValue, 0.35m),
            };
            foreach (var (from, to, rate) in brackets)
            {
                if (income <= from) break;
                tax += (Math.Min(income, to) - from) * rate;
            }
            return Math.Round(tax, 0);
        }

        // ════════════════════════════════════════════════════════════════
        // PUBLIC API
        // ════════════════════════════════════════════════════════════════
        public async Task<List<PayrollDTO>> GetAllPayrollsAsync()
        {
            var list = await _repository.GetAllPayrollsAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<PayrollDTO> GetPayrollByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id phải lớn hơn 0");
            var p = await _repository.GetPayrollByIdAsync(id)
                    ?? throw new KeyNotFoundException($"Không tìm thấy payroll id {id}");
            return MapToDTO(p);
        }

        public async Task<PayslipDTO> GetPayslipAsync(int id)
        {
            var dto = await GetPayrollByIdAsync(id);
            return new PayslipDTO
            {
                EmployeeId = dto.EmployeeId,
                EmployeeCode = dto.EmployeeCode,
                FullName = dto.FullName ?? dto.EmployeeName,
                DepartmentName = dto.DepartmentName,
                TaxCode = dto.TaxCode,
                PayPeriod = dto.PayPeriod,
                ContractBasicSalary = dto.ContractBasicSalary,
                SalaryRatio = dto.SalaryRatio,
                StandardWorkingDays = dto.StandardWorkingDays,
                WorkingDays = dto.WorkingDays,
                PaidLeaveDays = dto.LeaveDays,
                UnpaidLeaveDays = dto.UnpaidLeaveDays,
                GrossIncome = dto.GrossIncome,
                TotalDeduction = dto.TotalDeduction,
                NetSalary = dto.FinalSalary,
                Income = new PayslipIncomeSection
                {
                    ActualSalary = dto.BaseSalary,
                    OvertimePay = dto.OvertimePay,
                    Bonus = dto.Bonus,
                    Total = dto.GrossIncome
                },
                Deductions = new PayslipDeductionSection
                {
                    BhxhEmployee = dto.BhxhEmployee,
                    BhytEmployee = dto.BhytEmployee,
                    BhtnEmployee = dto.BhtnEmployee,
                    TotalInsurance = dto.BhxhEmployee + dto.BhytEmployee + dto.BhtnEmployee,
                    PersonalDeduction = dto.PersonalDeduction,
                    DependentDeduction = dto.DependentDeduction,
                    OtherDeduction = dto.Deduction,
                    TaxableIncome = dto.TaxableIncome,
                    TaxType = dto.TaxType,
                    PersonalTax = dto.PersonalTax,
                    TotalDeduction = dto.TotalDeduction
                },
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<PayrollDTO> CreatePayrollAsync(PayrollCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeId))
                throw new ArgumentException("EmployeeId không được để trống");

            var hrData = await _hrClient.GetPayrollDataAsync(dto.EmployeeId);
            if (hrData.Employee == null)
                throw new ArgumentException($"Nhân viên '{dto.EmployeeId}' không tìm thấy trong HR");

            decimal contractBasic = hrData.ContractBasicSalary > 0
                ? hrData.ContractBasicSalary
                : dto.ContractBasicSalary > 0 ? dto.ContractBasicSalary : dto.BaseSalary;

            decimal ratio = hrData.SalaryRatio > 0 ? hrData.SalaryRatio : dto.SalaryRatio;
            string taxType = hrData.TaxType ?? dto.TaxType;
            bool isBhxh = hrData.IsSocialInsuranceSubject;
            int dependents = hrData.DependentsCount > 0 ? hrData.DependentsCount : dto.DependentCount;

            var calc = CalcFullSalary(contractBasic, ratio,
                dto.WorkingDays, dto.LeaveDays, dto.StandardWorkingDays,
                dto.OvertimePay, dto.Bonus, dto.Deduction,
                dependents, isBhxh, taxType);

            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                EmployeeName = hrData.Employee.FullName ?? dto.EmployeeName,
                EmployeeCode = hrData.Employee.EmployeeCode,
                FullName = hrData.Employee.FullName,
                DepartmentName = hrData.Employee.DepartmentName,
                TaxCode = hrData.TaxCode ?? dto.TaxCode,
                PayPeriod = dto.PayPeriod,
                ContractBasicSalary = contractBasic,
                SalaryRatio = ratio,
                BaseSalary = calc.ActualSalary,
                TaxType = taxType,
                IsSocialInsuranceSubject = isBhxh,
                WorkingDays = dto.WorkingDays,
                StandardWorkingDays = dto.StandardWorkingDays,
                LeaveDays = dto.LeaveDays,
                UnpaidLeaveDays = dto.UnpaidLeaveDays,
                DependentCount = dependents,
                OvertimePay = dto.OvertimePay,
                Bonus = dto.Bonus,
                GrossIncome = calc.GrossIncome,
                BhxhEmployee = calc.BhxhNv,
                BhytEmployee = calc.BhytNv,
                BhtnEmployee = calc.BhtnNv,
                BhxhEmployer = calc.BhxhDn,
                BhytEmployer = calc.BhytDn,
                BhtnEmployer = calc.BhtnDn,
                PersonalDeduction = GTGC_BANTH,
                DependentDeduction = calc.DependentDeduction,
                TaxableIncome = calc.TaxableIncome,
                PersonalTax = calc.PersonalTax,
                Deduction = dto.Deduction,
                TotalDeduction = calc.TotalDeduction,
                FinalSalary = calc.FinalSalary,
                PayrollStatus = "Draft",
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreatePayrollAsync(payroll);
            return MapToDTO(created);
        }

        public async Task<PayrollDTO> UpdatePayrollAsync(int id, PayrollUpdateDTO dto)
        {
            if (id <= 0) throw new ArgumentException("Id phải lớn hơn 0");
            var payroll = await _repository.GetPayrollByIdAsync(id)
                          ?? throw new KeyNotFoundException($"Không tìm thấy payroll id {id}");

            decimal contractBasic = dto.ContractBasicSalary > 0 ? dto.ContractBasicSalary : dto.BaseSalary;
            var calc = CalcFullSalary(contractBasic, dto.SalaryRatio,
                dto.WorkingDays, dto.LeaveDays, dto.StandardWorkingDays,
                dto.OvertimePay, dto.Bonus, dto.Deduction,
                dto.DependentCount, dto.IsSocialInsuranceSubject, dto.TaxType);

            payroll.EmployeeName = dto.EmployeeName;
            payroll.PayPeriod = dto.PayPeriod ?? payroll.PayPeriod;
            payroll.ContractBasicSalary = contractBasic;
            payroll.SalaryRatio = dto.SalaryRatio;
            payroll.BaseSalary = calc.ActualSalary;
            payroll.TaxType = dto.TaxType;
            payroll.IsSocialInsuranceSubject = dto.IsSocialInsuranceSubject;
            payroll.WorkingDays = dto.WorkingDays;
            payroll.StandardWorkingDays = dto.StandardWorkingDays;
            payroll.LeaveDays = dto.LeaveDays;
            payroll.UnpaidLeaveDays = dto.UnpaidLeaveDays;
            payroll.DependentCount = dto.DependentCount;
            payroll.OvertimePay = dto.OvertimePay;
            payroll.Bonus = dto.Bonus;
            payroll.GrossIncome = calc.GrossIncome;
            payroll.BhxhEmployee = calc.BhxhNv;
            payroll.BhytEmployee = calc.BhytNv;
            payroll.BhtnEmployee = calc.BhtnNv;
            payroll.BhxhEmployer = calc.BhxhDn;
            payroll.BhytEmployer = calc.BhytDn;
            payroll.BhtnEmployer = calc.BhtnDn;
            payroll.PersonalDeduction = GTGC_BANTH;
            payroll.DependentDeduction = calc.DependentDeduction;
            payroll.TaxableIncome = calc.TaxableIncome;
            payroll.PersonalTax = calc.PersonalTax;
            payroll.Deduction = dto.Deduction;
            payroll.TotalDeduction = calc.TotalDeduction;
            payroll.FinalSalary = calc.FinalSalary;
            payroll.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdatePayrollAsync(payroll);
            return MapToDTO(updated);
        }

        public async Task<bool> DeletePayrollAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Id phải lớn hơn 0");
            return await _repository.DeletePayrollAsync(id);
        }

        public async Task<PayrollReportDTO> GetPayrollReportAsync()
        {
            var count = await _repository.GetTotalPayrollsCountAsync();
            if (count == 0) return new PayrollReportDTO();
            return new PayrollReportDTO
            {
                TotalPayrolls = count,
                TotalSalary = await _repository.GetTotalSalaryAsync(),
                AverageSalary = await _repository.GetAverageSalaryAsync(),
                MaxSalary = await _repository.GetMaxSalaryAsync(),
                MinSalary = await _repository.GetMinSalaryAsync(),
            };
        }

        // ════════════════════════════════════════════════════════════════
        // DUYỆT BẢNG LƯƠNG & GỬI EMAIL PAYSLIP
        // ════════════════════════════════════════════════════════════════
        public async Task<ApprovePayrollResult> ApproveAndSendPayslipAsync(ApprovePayrollRequest request)
        {
            var result = new ApprovePayrollResult();

            // Lấy toàn bộ payroll trước để log diagnostic
            var allPayrolls = await _repository.GetAllPayrollsAsync();

            _logger.LogInformation("[Approve] Tổng số payroll trong DB: {Total}", allPayrolls.Count);

            // Log phân bố status để debug
            var statusGroups = allPayrolls.GroupBy(p => p.PayrollStatus ?? "NULL").ToList();
            foreach (var g in statusGroups)
                _logger.LogInformation("[Approve] Status '{Status}': {Count} bản ghi", g.Key, g.Count());

            // Lọc theo PayPeriod
            var byPeriod = allPayrolls
                .Where(p => p.PayPeriod == request.PayPeriod)
                .ToList();

            _logger.LogInformation("[Approve] PayPeriod '{Period}': tìm thấy {Count} bản ghi",
                request.PayPeriod, byPeriod.Count);

            if (!byPeriod.Any())
            {
                _logger.LogWarning("[Approve] Không tìm thấy payroll nào cho kỳ {Period}", request.PayPeriod);
                result.Items.Add(new ApprovePayrollItem
                {
                    Status = "NoData",
                    Note = $"Không tìm thấy payroll nào cho kỳ lương {request.PayPeriod}"
                });
                return result;
            }

            // Lọc targets — chấp nhận cả NULL và "Draft" (bản ghi cũ trước khi có cột payroll_status)
            List<Payroll> targets;

            if (request.PayrollIds != null && request.PayrollIds.Any())
            {
                targets = byPeriod
                    .Where(p => request.PayrollIds.Contains(p.Id))
                    .ToList();
                _logger.LogInformation("[Approve] Duyệt theo ID cụ thể: {Count} bản ghi", targets.Count);
            }
            else
            {
                targets = byPeriod
                    .Where(p => string.IsNullOrEmpty(p.PayrollStatus)
                             || p.PayrollStatus == "Draft")
                    .ToList();

                var alreadyApproved = byPeriod.Count(p => p.PayrollStatus == "Approved");
                _logger.LogInformation("[Approve] Eligible (Draft/NULL): {Count}, đã Approved trước đó: {Already}",
                    targets.Count, alreadyApproved);

                if (!targets.Any())
                {
                    var reason = alreadyApproved > 0
                        ? $"Tất cả {alreadyApproved} payroll kỳ {request.PayPeriod} đã được duyệt trước đó"
                        : $"Không có payroll nào ở trạng thái Draft trong kỳ {request.PayPeriod}";

                    _logger.LogWarning("[Approve] {Reason}", reason);
                    result.Items.Add(new ApprovePayrollItem
                    {
                        Status = "NoEligible",
                        Note = reason
                    });
                    return result;
                }
            }

            _logger.LogInformation("[Approve] Bắt đầu duyệt {Count} bản ghi kỳ {Period}",
                targets.Count, request.PayPeriod);

            foreach (var payroll in targets)
            {
                var item = new ApprovePayrollItem
                {
                    PayrollId = payroll.Id,
                    EmployeeId = payroll.EmployeeId,
                    FullName = payroll.FullName ?? payroll.EmployeeName,
                };
                try
                {
                    // Cập nhật trạng thái Approved
                    payroll.PayrollStatus = "Approved";
                    payroll.ApprovedAt = DateTime.UtcNow;
                    payroll.UpdatedAt = DateTime.UtcNow;
                    await _repository.UpdatePayrollAsync(payroll);
                    result.TotalApproved++;
                    item.Status = "Approved";

                    _logger.LogInformation("[Approve] Đã duyệt payroll {Id} — nhân viên {EmpId}",
                        payroll.Id, payroll.EmployeeId);

                    // Gửi email
                    if (request.SendEmail)
                    {
                        string? email = null;
                        try
                        {
                            var hrEmp = await _hrClient.GetEmployeeExtendedAsync(payroll.EmployeeId);
                            email = hrEmp?.Email;
                            item.Email = email;

                            _logger.LogInformation("[Approve] HR trả email cho {EmpId}: '{Email}'",
                                payroll.EmployeeId, email ?? "(null)");
                        }
                        catch (Exception hrEx)
                        {
                            _logger.LogError(hrEx, "[Approve] Lỗi gọi HR lấy email cho {EmpId}",
                                payroll.EmployeeId);
                            item.Status = "EmailFailed";
                            item.Note = $"Lỗi gọi HR Service: {hrEx.Message}";
                            result.EmailFailed++;
                            result.Items.Add(item);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(email))
                        {
                            _logger.LogWarning("[Approve] Nhân viên {EmpId} không có email đăng ký",
                                payroll.EmployeeId);
                            item.Status = "EmailFailed";
                            item.Note = "Nhân viên không có email đăng ký trong HR";
                            result.EmailFailed++;
                        }
                        else
                        {
                            try
                            {
                                var payslip = MapToPayslip(payroll);
                                var sent = await _emailService.SendPayslipEmailAsync(
                                                  email, item.FullName ?? "", payslip);
                                if (sent)
                                {
                                    _logger.LogInformation("[Approve] Gửi email thành công → {Email}", email);
                                    item.Status = "EmailSent";
                                    result.EmailSent++;
                                }
                                else
                                {
                                    _logger.LogWarning("[Approve] Gửi email thất bại (SendPayslipEmail trả false) → {Email}",
                                        email);
                                    item.Status = "EmailFailed";
                                    item.Note = "Gửi email thất bại (SMTP trả lỗi)";
                                    result.EmailFailed++;
                                }
                            }
                            catch (Exception mailEx)
                            {
                                _logger.LogError(mailEx, "[Approve] Exception khi gửi email → {Email}", email);
                                item.Status = "EmailFailed";
                                item.Note = $"Exception gửi email: {mailEx.Message}";
                                result.EmailFailed++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Approve] Lỗi duyệt payroll {Id}", payroll.Id);
                    item.Status = "Error";
                    item.Note = ex.Message;
                }

                result.Items.Add(item);
            }

            _logger.LogInformation(
                "[Approve] Hoàn thành: {Approved} approved, {Sent} email sent, {Failed} failed",
                result.TotalApproved, result.EmailSent, result.EmailFailed);

            return result;
        }

        // ════════════════════════════════════════════════════════════════
        // MAPPING
        // ════════════════════════════════════════════════════════════════

        /// <summary>Map Payroll entity → PayslipDTO (dùng nội bộ, không gọi lại DB)</summary>
        private static PayslipDTO MapToPayslip(Payroll p) => new()
        {
            EmployeeId = p.EmployeeId,
            EmployeeCode = p.EmployeeCode,
            FullName = p.FullName ?? p.EmployeeName,
            DepartmentName = p.DepartmentName,
            TaxCode = p.TaxCode,
            PayPeriod = p.PayPeriod,
            ContractBasicSalary = p.ContractBasicSalary,
            SalaryRatio = p.SalaryRatio,
            StandardWorkingDays = p.StandardWorkingDays,
            WorkingDays = p.WorkingDays,
            PaidLeaveDays = p.LeaveDays,
            UnpaidLeaveDays = p.UnpaidLeaveDays,
            GrossIncome = p.GrossIncome,
            TotalDeduction = p.TotalDeduction,
            NetSalary = p.FinalSalary,
            Income = new PayslipIncomeSection
            {
                ActualSalary = p.BaseSalary,
                OvertimePay = p.OvertimePay,
                Bonus = p.Bonus,
                Total = p.GrossIncome
            },
            Deductions = new PayslipDeductionSection
            {
                BhxhEmployee = p.BhxhEmployee,
                BhytEmployee = p.BhytEmployee,
                BhtnEmployee = p.BhtnEmployee,
                TotalInsurance = p.BhxhEmployee + p.BhytEmployee + p.BhtnEmployee,
                PersonalDeduction = p.PersonalDeduction,
                DependentDeduction = p.DependentDeduction,
                OtherDeduction = p.Deduction,
                TaxableIncome = p.TaxableIncome,
                TaxType = p.TaxType,
                PersonalTax = p.PersonalTax,
                TotalDeduction = p.TotalDeduction
            },
            GeneratedAt = DateTime.UtcNow
        };

        private static PayrollDTO MapToDTO(Payroll p) => new()
        {
            Id = p.Id,
            EmployeeId = p.EmployeeId,
            EmployeeName = p.EmployeeName,
            EmployeeCode = p.EmployeeCode,
            FullName = p.FullName,
            DepartmentName = p.DepartmentName,
            TaxCode = p.TaxCode,
            PayPeriod = p.PayPeriod,
            ContractBasicSalary = p.ContractBasicSalary,
            SalaryRatio = p.SalaryRatio,
            BaseSalary = p.BaseSalary,
            TaxType = p.TaxType,
            IsSocialInsuranceSubject = p.IsSocialInsuranceSubject,
            WorkingDays = p.WorkingDays,
            StandardWorkingDays = p.StandardWorkingDays,
            LeaveDays = p.LeaveDays,
            UnpaidLeaveDays = p.UnpaidLeaveDays,
            DependentCount = p.DependentCount,
            DependentDeduction = p.DependentDeduction,
            OvertimePay = p.OvertimePay,
            Bonus = p.Bonus,
            GrossIncome = p.GrossIncome,
            BhxhEmployee = p.BhxhEmployee,
            BhytEmployee = p.BhytEmployee,
            BhtnEmployee = p.BhtnEmployee,
            BhxhEmployer = p.BhxhEmployer,
            BhytEmployer = p.BhytEmployer,
            BhtnEmployer = p.BhtnEmployer,
            PersonalDeduction = p.PersonalDeduction,
            TaxableIncome = p.TaxableIncome,
            PersonalTax = p.PersonalTax,
            Deduction = p.Deduction,
            TotalDeduction = p.TotalDeduction,
            FinalSalary = p.FinalSalary,
            PayrollStatus = p.PayrollStatus,
            ApprovedAt = p.ApprovedAt,
            EmployeeStatus = p.EmployeeStatus,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
        };
    }

    internal class SalaryCalcResult
    {
        public decimal ActualSalary { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal BhxhNv { get; set; }
        public decimal BhytNv { get; set; }
        public decimal BhtnNv { get; set; }
        public decimal BhxhDn { get; set; }
        public decimal BhytDn { get; set; }
        public decimal BhtnDn { get; set; }
        public decimal DependentDeduction { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal PersonalTax { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal FinalSalary { get; set; }
    }
}