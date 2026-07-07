using Microsoft.EntityFrameworkCore;
using payroll_service.Data;
using payroll_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payroll_service.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private readonly PayrollDbContext _context;

        public PayrollRepository(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payroll>> GetAllPayrollsAsync()
        {
            return await _context.Payrolls
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payroll> GetPayrollByIdAsync(int id)
        {
            return await _context.Payrolls
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payroll> CreatePayrollAsync(Payroll payroll)
        {
            payroll.CreatedAt = DateTime.UtcNow;
            _context.Payrolls.Add(payroll);
            try
            {
                await _context.SaveChangesAsync();
                return payroll;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                // Surface inner exception message for diagnostics
                var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new InvalidOperationException($"Database update failed: {inner}", dbEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save payroll: {ex.Message}", ex);
            }
        }

        public async Task<Payroll> UpdatePayrollAsync(Payroll payroll)
        {
            payroll.UpdatedAt = DateTime.UtcNow;
            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
            return payroll;
        }

        public async Task<bool> DeletePayrollAsync(int id)
        {
            var payroll = await _context.Payrolls.FirstOrDefaultAsync(p => p.Id == id);
            if (payroll == null)
                return false;

            _context.Payrolls.Remove(payroll);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalSalaryAsync()
        {
            return await _context.Payrolls
                .SumAsync(p => p.FinalSalary);
        }

        public async Task<decimal> GetAverageSalaryAsync()
        {
            return await _context.Payrolls
                .AverageAsync(p => p.FinalSalary);
        }

        public async Task<decimal> GetMaxSalaryAsync()
        {
            var maxSalary = await _context.Payrolls
                .MaxAsync(p => (decimal?)p.FinalSalary);
            return maxSalary ?? 0;
        }

        public async Task<decimal> GetMinSalaryAsync()
        {
            var minSalary = await _context.Payrolls
                .MinAsync(p => (decimal?)p.FinalSalary);
            return minSalary ?? 0;
        }

        public async Task<int> GetTotalPayrollsCountAsync()
        {
            return await _context.Payrolls.CountAsync();
        }
    }
}
