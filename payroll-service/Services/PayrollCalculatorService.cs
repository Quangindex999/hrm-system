using payroll_service.DTOs;

namespace payroll_service.Services
{
    public class PayrollCalculationResult
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;

        // Dữ liệu gốc
        public double BaseSalary { get; set; }
        public double WorkingDays { get; set; }
        public double StandardDays { get; set; }
        public double OvertimeHours { get; set; }
        public double PaidLeaveDays { get; set; }
        public double UnpaidLeaveDays { get; set; }

        // Kết quả tính lương
        public double DailySalary { get; set; }
        public double ActualSalary { get; set; }
        public double OvertimePay { get; set; }
        public double UnpaidLeaveDeduct { get; set; }
        public double GrossSalary { get; set; }

        public string? Note { get; set; }
    }

    public interface IPayrollCalculatorService
    {
        Task<PayrollCalculationResult> CalculateAsync(string employeeId, string month);
    }

    public class PayrollCalculatorService : IPayrollCalculatorService
    {
        private const double OvertimeRateNormal = 1.5;
        private const double OvertimeRateWeekend = 2.0;
        private const double OvertimeRateHoliday = 3.0;
        private const double StandardHoursPerDay = 8.0;

        private readonly IHrServiceClient _hrClient;
        private readonly IAttendanceServiceClient _attendanceClient;
        private readonly ILogger<PayrollCalculatorService> _logger;

        public PayrollCalculatorService(
            IHrServiceClient hrClient,
            IAttendanceServiceClient attendanceClient,
            ILogger<PayrollCalculatorService> logger)
        {
            _hrClient = hrClient;
            _attendanceClient = attendanceClient;
            _logger = logger;
        }

        public async Task<PayrollCalculationResult> CalculateAsync(string employeeId, string month)
        {
            _logger.LogInformation("[Payroll] Tính lương {Id} tháng {Month}", employeeId, month);

            // Gọi song song HR + Attendance
            var hrTask = _hrClient.GetEmployeeByIdAsync(employeeId);
            var attendanceTask = _attendanceClient.GetAttendanceForEmployeeAsync(employeeId, month);
            await Task.WhenAll(hrTask, attendanceTask);

            var hr = hrTask.Result;
            var attendance = attendanceTask.Result;

            var result = new PayrollCalculationResult
            {
                EmployeeId = employeeId,
                Month = month,
            };

            if (hr == null)
            {
                _logger.LogWarning("[Payroll] Không lấy được HR data — {Id}", employeeId);
                result.Note = "Không lấy được thông tin lương cơ bản từ HR Service.";
                return result;
            }

            // Nếu không có Attendance → vẫn tính lương với mặc định 26 ngày
            double standardDays = 26;
            double workingDays = standardDays;
            double paidLeaveDays = 0;
            double unpaidLeaveDays = 0;
            double overtimeNormal = 0;
            double overtimeWeekend = 0;
            double overtimeHoliday = 0;
            double overtimeTotal = 0;

            if (attendance != null)
            {
                standardDays = attendance.GetStandardDays();
                workingDays = (double)attendance.WorkingDays;
                paidLeaveDays = (double)attendance.GetPaidLeaveDays();
                unpaidLeaveDays = (double)attendance.GetUnpaidLeaveDays();
                overtimeNormal = attendance.GetOvertimeNormal();
                overtimeWeekend = attendance.GetOvertimeWeekend();
                overtimeHoliday = attendance.GetOvertimeHoliday();
                overtimeTotal = attendance.OvertimeHours;
            }
            else
            {
                _logger.LogWarning("[Payroll] Không có Attendance — tính lương mặc định 26 ngày — {Id}", employeeId);
                result.Note = "Không có dữ liệu chấm công, tính theo mặc định 26 ngày.";
            }

            // ── Gán dữ liệu gốc ────────────────────────────────────────────
            result.BaseSalary = hr.BaseSalary.HasValue ? (double)hr.BaseSalary.Value : 0.0;
            result.WorkingDays = workingDays;
            result.StandardDays = standardDays;
            result.OvertimeHours = overtimeTotal;
            result.PaidLeaveDays = paidLeaveDays;
            result.UnpaidLeaveDays = unpaidLeaveDays;

            // ── Tính lương ──────────────────────────────────────────────────
            result.DailySalary = result.StandardDays > 0
                                  ? result.BaseSalary / result.StandardDays
                                  : 0;

            result.ActualSalary = result.DailySalary * (result.WorkingDays + result.PaidLeaveDays);

            double hourlyRate = result.StandardDays > 0
                                  ? result.BaseSalary / (result.StandardDays * StandardHoursPerDay)
                                  : 0;

            result.OvertimePay = (overtimeNormal * OvertimeRateNormal * hourlyRate) +
                                  (overtimeWeekend * OvertimeRateWeekend * hourlyRate) +
                                  (overtimeHoliday * OvertimeRateHoliday * hourlyRate);

            // Fallback nếu Attendance chỉ trả tổng giờ, không chia loại
            if (result.OvertimePay == 0 && result.OvertimeHours > 0)
                result.OvertimePay = result.OvertimeHours * OvertimeRateNormal * hourlyRate;

            result.UnpaidLeaveDeduct = result.DailySalary * result.UnpaidLeaveDays;
            result.GrossSalary = result.ActualSalary + result.OvertimePay - result.UnpaidLeaveDeduct;

            _logger.LogInformation(
                "[Payroll] Xong — {Id} / {Month} | base={Base:N0} | gross={Gross:N0}",
                employeeId, month, result.BaseSalary, result.GrossSalary);

            return result;
        }
    }
}