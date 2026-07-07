using payroll_service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace payroll_service.Repositories
{
    public interface IPayrollRepository
    {
        Task<List<Payroll>> GetAllPayrollsAsync();
        Task<Payroll> GetPayrollByIdAsync(int id);
        Task<Payroll> CreatePayrollAsync(Payroll payroll);
        Task<Payroll> UpdatePayrollAsync(Payroll payroll);
        Task<bool> DeletePayrollAsync(int id);
        Task<decimal> GetTotalSalaryAsync();
        Task<decimal> GetAverageSalaryAsync();
        Task<decimal> GetMaxSalaryAsync();
        Task<decimal> GetMinSalaryAsync();
        Task<int> GetTotalPayrollsCountAsync();
    }
}
