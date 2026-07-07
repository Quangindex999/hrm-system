using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace payroll_service.Models
{
    [Table("payrolls")]
    public class Payroll
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required, Column("employee_id", TypeName = "VARCHAR(50)")]
        public string EmployeeId { get; set; } = null!;

        [Required, Column("employee_name", TypeName = "NVARCHAR(255)")]
        public string EmployeeName { get; set; } = null!;

        [Column("employee_code", TypeName = "VARCHAR(50)")]
        public string? EmployeeCode { get; set; }

        [Column("full_name", TypeName = "NVARCHAR(255)")]
        public string? FullName { get; set; }

        [Column("department_name", TypeName = "NVARCHAR(255)")]
        public string? DepartmentName { get; set; }

        // ── Kỳ lương ─────────────────────────────────────────────────────
        [Column("pay_period", TypeName = "VARCHAR(7)")]
        public string? PayPeriod { get; set; }

        [Column("tax_code", TypeName = "VARCHAR(20)")]
        public string? TaxCode { get; set; }

        // ── Hợp đồng ─────────────────────────────────────────────────────
        [Column("contract_basic_salary", TypeName = "DECIMAL(12,2)")]
        public decimal ContractBasicSalary { get; set; }

        [Column("salary_ratio", TypeName = "DECIMAL(5,4)")]
        public decimal SalaryRatio { get; set; } = 1.0m;

        /// <summary>Lương tính theo ngày công thực tế (sau ratio)</summary>
        [Column("base_salary", TypeName = "DECIMAL(12,2)")]
        public decimal BaseSalary { get; set; }

        [Column("tax_type", TypeName = "VARCHAR(20)")]
        public string TaxType { get; set; } = "Progressive";

        [Column("is_social_insurance_subject", TypeName = "TINYINT(1)")]
        public bool IsSocialInsuranceSubject { get; set; } = true;

        // ── Ngày công ─────────────────────────────────────────────────────
        [Required, Column("working_days", TypeName = "INT")]
        public int WorkingDays { get; set; }

        [Column("standard_working_days", TypeName = "INT")]
        public int StandardWorkingDays { get; set; } = 26;

        [Column("leave_days", TypeName = "INT")]
        public int LeaveDays { get; set; }

        [Column("unpaid_leave_days", TypeName = "INT")]
        public int UnpaidLeaveDays { get; set; }

        // ── Người phụ thuộc ───────────────────────────────────────────────
        [Column("dependent_count", TypeName = "INT")]
        public int DependentCount { get; set; }

        // ── Tăng ca & thưởng ─────────────────────────────────────────────
        [Column("overtime_pay", TypeName = "DECIMAL(12,2)")]
        public decimal OvertimePay { get; set; }

        [Column("bonus", TypeName = "DECIMAL(12,2)")]
        public decimal Bonus { get; set; }

        // ── Thu nhập gộp ─────────────────────────────────────────────────
        [Column("gross_income", TypeName = "DECIMAL(12,2)")]
        public decimal GrossIncome { get; set; }

        // ── Bảo hiểm NV đóng ─────────────────────────────────────────────
        [Column("bhxh_employee", TypeName = "DECIMAL(12,2)")]
        public decimal BhxhEmployee { get; set; }

        [Column("bhyt_employee", TypeName = "DECIMAL(12,2)")]
        public decimal BhytEmployee { get; set; }

        [Column("bhtn_employee", TypeName = "DECIMAL(12,2)")]
        public decimal BhtnEmployee { get; set; }

        // ── Bảo hiểm DN đóng ─────────────────────────────────────────────
        [Column("bhxh_employer", TypeName = "DECIMAL(12,2)")]
        public decimal BhxhEmployer { get; set; }

        [Column("bhyt_employer", TypeName = "DECIMAL(12,2)")]
        public decimal BhytEmployer { get; set; }

        [Column("bhtn_employer", TypeName = "DECIMAL(12,2)")]
        public decimal BhtnEmployer { get; set; }

        // ── Giảm trừ & thuế TNCN ─────────────────────────────────────────
        [Column("personal_deduction", TypeName = "DECIMAL(12,2)")]
        public decimal PersonalDeduction { get; set; }

        [Column("dependent_deduction", TypeName = "DECIMAL(12,2)")]
        public decimal DependentDeduction { get; set; }

        [Column("taxable_income", TypeName = "DECIMAL(12,2)")]
        public decimal TaxableIncome { get; set; }

        [Column("personal_tax", TypeName = "DECIMAL(12,2)")]
        public decimal PersonalTax { get; set; }

        // ── Tổng hợp ─────────────────────────────────────────────────────
        [Column("deduction", TypeName = "DECIMAL(12,2)")]
        public decimal Deduction { get; set; }

        [Column("total_deduction", TypeName = "DECIMAL(12,2)")]
        public decimal TotalDeduction { get; set; }

        [Column("final_salary", TypeName = "DECIMAL(12,2)")]
        public decimal FinalSalary { get; set; }

        // ── Trạng thái bảng lương ─────────────────────────────────────────
        [Column("payroll_status", TypeName = "VARCHAR(20)")]
        public string PayrollStatus { get; set; } = "Draft";   // Draft | Approved

        [Column("approved_at", TypeName = "DATETIME")]
        public DateTime? ApprovedAt { get; set; }

        /// <summary>Trạng thái nhân viên lúc tính lương (Active / Inactive)</summary>
        [Column("employee_status", TypeName = "VARCHAR(20)")]
        public string? EmployeeStatus { get; set; }

        [Column("created_at", TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "DATETIME")]
        public DateTime? UpdatedAt { get; set; }
    }
}