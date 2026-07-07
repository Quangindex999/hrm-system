namespace payroll_service.DTOs
{
    public class HrContractDTO
    {
        public string? Id { get; set; }
        public string? ContractNumber { get; set; }
        public string? EmployeeId { get; set; }
        public string? ContractTypeId { get; set; }
        public string? ContractTypeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal SalaryRatio { get; set; } = 1.0m;
        public DateTime? SignDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class HrContractTypeDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal DefaultSalaryRatio { get; set; } = 1.0m;
        public bool IsSocialInsuranceSubject { get; set; } = true;
        /// <summary>"Progressive" hoặc "Flat10"</summary>
        public string TaxType { get; set; } = "Progressive";
        public string? Description { get; set; }
    }

    public class HrEmployeeExtendedDTO
    {
        public string? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }          // ← THÊM — dùng để gửi payslip
        public string? DepartmentName { get; set; }
        public string? TaxCode { get; set; }
        public int DependentsCount { get; set; } = 0;
        public string? Status { get; set; }
    }

    /// <summary>Tổng hợp Employee + Contract + ContractType để tính lương</summary>
    public class HrPayrollDataDTO
    {
        public HrEmployeeExtendedDTO? Employee { get; set; }
        public HrContractDTO? ActiveContract { get; set; }
        public HrContractTypeDTO? ContractType { get; set; }

        public decimal ContractBasicSalary => ActiveContract?.BasicSalary ?? 0;
        public decimal SalaryRatio => ActiveContract?.SalaryRatio ?? 1.0m;
        public decimal EffectiveSalary => ContractBasicSalary * SalaryRatio;
        public bool IsSocialInsuranceSubject => ContractType?.IsSocialInsuranceSubject ?? true;
        public string TaxType => ContractType?.TaxType ?? "Progressive";
        public string? TaxCode => Employee?.TaxCode;
        public int DependentsCount => Employee?.DependentsCount ?? 0;
    }
}