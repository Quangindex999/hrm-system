using payroll_service.DTOs;

namespace payroll_service.Services
{
    public interface IEmailService
    {
        /// <summary>Gửi phiếu lương chi tiết qua email cho nhân viên</summary>
        Task<bool> SendPayslipEmailAsync(string toEmail, string employeeName, PayslipDTO payslip);
    }
}