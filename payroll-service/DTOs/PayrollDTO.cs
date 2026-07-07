using System.ComponentModel.DataAnnotations;

namespace payroll_service.DTOs
{
    public class PayrollDTO
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DepartmentName { get; set; }
        public string? TaxCode { get; set; }

        // Kỳ lương
        public string? PayPeriod { get; set; }

        // Hợp đồng
        public decimal ContractBasicSalary { get; set; }
        public decimal SalaryRatio { get; set; } = 1.0m;
        public string TaxType { get; set; } = "Progressive";
        public bool IsSocialInsuranceSubject { get; set; } = true;

        // BaseSalary ở đây = lương tính theo ngày công thực tế (sau nhân ratio)
        public decimal BaseSalary { get; set; }

        // Ngày công
        public int WorkingDays { get; set; }
        public int StandardWorkingDays { get; set; } = 26;
        public int LeaveDays { get; set; }
        public int UnpaidLeaveDays { get; set; }

        // Người phụ thuộc
        public int DependentCount { get; set; }
        public decimal DependentDeduction { get; set; }

        // Tăng ca & thưởng
        public decimal OvertimePay { get; set; }
        public decimal Bonus { get; set; }

        // Thu nhập gộp
        public decimal GrossIncome { get; set; }

        // Bảo hiểm NV đóng
        public decimal BhxhEmployee { get; set; }
        public decimal BhytEmployee { get; set; }
        public decimal BhtnEmployee { get; set; }

        // Bảo hiểm DN đóng
        public decimal BhxhEmployer { get; set; }
        public decimal BhytEmployer { get; set; }
        public decimal BhtnEmployer { get; set; }

        // Thuế TNCN
        public decimal PersonalDeduction { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal PersonalTax { get; set; }

        // Tổng hợp
        public decimal Deduction { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal FinalSalary { get; set; }

        // Trạng thái bảng lương
        public string PayrollStatus { get; set; } = "Draft";   // Draft | Approved
        public DateTime? ApprovedAt { get; set; }

        // Trạng thái nhân viên lúc tính lương
        public string? EmployeeStatus { get; set; }            // Active | Inactive

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PayrollCreateDTO
    {
        [Required(ErrorMessage = "EmployeeId không được để trống")]
        public string EmployeeId { get; set; } = null!;

        [Required(ErrorMessage = "EmployeeName không được để trống")]
        public string EmployeeName { get; set; } = null!;

        public string? PayPeriod { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal BaseSalary { get; set; }

        public decimal ContractBasicSalary { get; set; }
        public decimal SalaryRatio { get; set; } = 1.0m;
        public string TaxType { get; set; } = "Progressive";
        public bool IsSocialInsuranceSubject { get; set; } = true;
        public string? TaxCode { get; set; }

        [Required, Range(0, 31)]
        public int WorkingDays { get; set; }

        public int StandardWorkingDays { get; set; } = 26;

        [Range(0, 31)] public int LeaveDays { get; set; }
        [Range(0, 31)] public int UnpaidLeaveDays { get; set; }
        [Range(0, 20)] public int DependentCount { get; set; } = 0;

        [Range(0, double.MaxValue)] public decimal OvertimePay { get; set; }
        [Range(0, double.MaxValue)] public decimal Bonus { get; set; }
        [Range(0, double.MaxValue)] public decimal Deduction { get; set; }
    }

    public class PayrollUpdateDTO
    {
        [Required] public string EmployeeName { get; set; } = null!;
        public string? PayPeriod { get; set; }

        [Required, Range(0, double.MaxValue)] public decimal BaseSalary { get; set; }
        public decimal ContractBasicSalary { get; set; }
        public decimal SalaryRatio { get; set; } = 1.0m;
        public string TaxType { get; set; } = "Progressive";
        public bool IsSocialInsuranceSubject { get; set; } = true;

        [Required, Range(0, 31)] public int WorkingDays { get; set; }
        public int StandardWorkingDays { get; set; } = 26;
        [Range(0, 31)] public int LeaveDays { get; set; }
        [Range(0, 31)] public int UnpaidLeaveDays { get; set; }
        [Range(0, 20)] public int DependentCount { get; set; } = 0;

        [Range(0, double.MaxValue)] public decimal OvertimePay { get; set; }
        [Range(0, double.MaxValue)] public decimal Bonus { get; set; }
        [Range(0, double.MaxValue)] public decimal Deduction { get; set; }
    }

    public class PayrollReportDTO
    {
        public int TotalPayrolls { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinSalary { get; set; }
    }

    // ── Phiếu lương chi tiết ──────────────────────────────────────────
    public class PayslipDTO
    {
        public string EmployeeId { get; set; } = null!;
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DepartmentName { get; set; }
        public string? TaxCode { get; set; }
        public string? PayPeriod { get; set; }
        public decimal ContractBasicSalary { get; set; }
        public decimal SalaryRatio { get; set; }
        public int StandardWorkingDays { get; set; }
        public int WorkingDays { get; set; }
        public int PaidLeaveDays { get; set; }
        public int UnpaidLeaveDays { get; set; }
        public PayslipIncomeSection Income { get; set; } = new();
        public PayslipDeductionSection Deductions { get; set; } = new();
        public decimal GrossIncome { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class PayslipIncomeSection
    {
        public decimal ActualSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Bonus { get; set; }
        public decimal Total { get; set; }
    }

    public class PayslipDeductionSection
    {
        public decimal BhxhEmployee { get; set; }
        public decimal BhytEmployee { get; set; }
        public decimal BhtnEmployee { get; set; }
        public decimal TotalInsurance { get; set; }
        public decimal PersonalDeduction { get; set; }
        public decimal DependentDeduction { get; set; }
        public decimal OtherDeduction { get; set; }
        public decimal TaxableIncome { get; set; }
        public string TaxType { get; set; } = "Progressive";
        public decimal PersonalTax { get; set; }
        public decimal TotalDeduction { get; set; }
    }

    public class HrEmployeeDTO
    {
        public string? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DepartmentName { get; set; }
        public decimal? BaseSalary { get; set; }
        public string? TaxCode { get; set; }
        public int DependentsCount { get; set; } = 0;
    }

    public class CalculatePayrollRequest
    {
        [Required]
        public string Month { get; set; } = null!;
    }

    // ── Duyệt bảng lương & gửi email ─────────────────────────────────
    public class ApprovePayrollRequest
    {
        /// <summary>Danh sách ID cụ thể cần duyệt. Để trống = duyệt tất cả Draft trong PayPeriod</summary>
        public List<int>? PayrollIds { get; set; }

        [Required]
        public string PayPeriod { get; set; } = null!;

        /// <summary>Có gửi email payslip cho nhân viên không</summary>
        public bool SendEmail { get; set; } = true;
    }

    public class ApprovePayrollResult
    {
        public int TotalApproved { get; set; }
        public int EmailSent { get; set; }
        public int EmailFailed { get; set; }
        public List<ApprovePayrollItem> Items { get; set; } = new();
    }

    public class ApprovePayrollItem
    {
        public int PayrollId { get; set; }
        public string? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        /// <summary>Approved | EmailSent | EmailFailed | Error</summary>
        public string Status { get; set; } = "";
        public string? Note { get; set; }
    }
}