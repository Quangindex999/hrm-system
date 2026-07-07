using payroll_service.DTOs;

namespace payroll_service.Services
{
    public interface IHrServiceClient
    {
        Task<string> GetEmployeesAsync();
        Task<HrEmployeeDTO?> GetEmployeeByIdAsync(string employeeId);
        Task<HrEmployeeExtendedDTO?> GetEmployeeExtendedAsync(string employeeId);
        Task<HrContractDTO?> GetActiveContractAsync(string employeeId);
        Task<HrContractTypeDTO?> GetContractTypeAsync(string contractTypeId);
        Task<HrPayrollDataDTO> GetPayrollDataAsync(string employeeId);
    }
}