using System.Net;
using System.Net.Mail;
using System.Text;
using payroll_service.DTOs;

namespace payroll_service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendPayslipEmailAsync(string toEmail, string employeeName, PayslipDTO payslip)
        {
            try
            {
                var smtpHost = _config["Email:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_config["Email:SmtpPort"] ?? "587");
                var smtpUser = _config["Email:Username"] ?? "";
                var smtpPass = _config["Email:Password"] ?? "";
                var fromEmail = _config["Email:FromEmail"] ?? smtpUser;
                var fromName = _config["Email:FromName"] ?? "Phòng Nhân sự";

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var subject = $"Phiếu lương tháng {payslip.PayPeriod} - {employeeName}";
                var body = BuildEmailBody(employeeName, payslip);

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };
                mail.To.Add(toEmail);

                await client.SendMailAsync(mail);
                _logger.LogInformation("[Email] Gửi payslip thành công → {Email}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Email] Lỗi gửi payslip → {Email}", toEmail);
                return false;
            }
        }

        private static string BuildEmailBody(string employeeName, PayslipDTO p)
        {
            var period = p.PayPeriod ?? "N/A";
            return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <style>
    body {{ font-family: Arial, sans-serif; font-size: 14px; color: #333; }}
    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
    h2 {{ color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 8px; }}
    table {{ width: 100%; border-collapse: collapse; margin-top: 12px; }}
    th {{ background: #3498db; color: white; padding: 8px 12px; text-align: left; }}
    td {{ padding: 7px 12px; border-bottom: 1px solid #eee; }}
    tr:nth-child(even) td {{ background: #f9f9f9; }}
    .section-title {{ background: #ecf0f1; font-weight: bold; padding: 8px 12px; margin-top: 16px; }}
    .total-row td {{ font-weight: bold; background: #eaf4fb; }}
    .net-row td {{ font-weight: bold; font-size: 16px; color: #27ae60; background: #eafaf1; }}
    .footer {{ margin-top: 24px; font-size: 12px; color: #888; }}
  </style>
</head>
<body>
<div class='container'>
  <h2>PHIẾU LƯƠNG THÁNG {period}</h2>
  <p>Kính gửi <strong>{employeeName}</strong>,</p>
  <p>Phòng Nhân sự gửi phiếu lương tháng {period} của bạn như sau:</p>

  <table>
    <tr><th colspan='2'>THÔNG TIN NHÂN VIÊN</th></tr>
    <tr><td>Mã nhân viên</td><td>{p.EmployeeCode}</td></tr>
    <tr><td>Họ và tên</td><td>{p.FullName}</td></tr>
    <tr><td>Phòng ban</td><td>{p.DepartmentName}</td></tr>
    <tr><td>Mã số thuế</td><td>{p.TaxCode ?? "Chưa có"}</td></tr>
  </table>

  <table>
    <tr><th colspan='2'>NGÀY CÔNG</th></tr>
    <tr><td>Ngày công chuẩn</td><td>{p.StandardWorkingDays} ngày</td></tr>
    <tr><td>Ngày công thực tế</td><td>{p.WorkingDays} ngày</td></tr>
    <tr><td>Nghỉ phép hưởng lương</td><td>{p.PaidLeaveDays} ngày</td></tr>
    <tr><td>Nghỉ không lương</td><td>{p.UnpaidLeaveDays} ngày</td></tr>
  </table>

  <table>
    <tr><th colspan='2'>THU NHẬP</th></tr>
    <tr><td>Lương cơ bản HĐ</td><td>{p.ContractBasicSalary:N0} đ</td></tr>
    <tr><td>Hệ số lương</td><td>{p.SalaryRatio:F2}</td></tr>
    <tr><td>Lương theo công thực tế</td><td>{p.Income.ActualSalary:N0} đ</td></tr>
    <tr><td>Phụ cấp tăng ca</td><td>{p.Income.OvertimePay:N0} đ</td></tr>
    <tr><td>Thưởng</td><td>{p.Income.Bonus:N0} đ</td></tr>
    <tr class='total-row'><td>Tổng thu nhập (Gross)</td><td>{p.GrossIncome:N0} đ</td></tr>
  </table>

  <table>
    <tr><th colspan='2'>KHẤU TRỪ</th></tr>
    <tr><td>BHXH (8%)</td><td>{p.Deductions.BhxhEmployee:N0} đ</td></tr>
    <tr><td>BHYT (1.5%)</td><td>{p.Deductions.BhytEmployee:N0} đ</td></tr>
    <tr><td>BHTN (1%)</td><td>{p.Deductions.BhtnEmployee:N0} đ</td></tr>
    <tr><td>Tổng bảo hiểm NV đóng</td><td>{p.Deductions.TotalInsurance:N0} đ</td></tr>
    <tr><td>Giảm trừ bản thân</td><td>{p.Deductions.PersonalDeduction:N0} đ</td></tr>
    <tr><td>Giảm trừ người phụ thuộc</td><td>{p.Deductions.DependentDeduction:N0} đ</td></tr>
    <tr><td>Thu nhập tính thuế ({p.Deductions.TaxType})</td><td>{p.Deductions.TaxableIncome:N0} đ</td></tr>
    <tr><td>Thuế TNCN</td><td>{p.Deductions.PersonalTax:N0} đ</td></tr>
    <tr><td>Khấu trừ khác</td><td>{p.Deductions.OtherDeduction:N0} đ</td></tr>
    <tr class='total-row'><td>Tổng khấu trừ</td><td>{p.TotalDeduction:N0} đ</td></tr>
  </table>

  <table>
    <tr class='net-row'><td>LƯƠNG THỰC LĨNH (NET)</td><td>{p.NetSalary:N0} đ</td></tr>
  </table>

  <p class='footer'>
    Email này được gửi tự động từ hệ thống HRM. Nếu có thắc mắc, vui lòng liên hệ Phòng Nhân sự.<br/>
    Thời gian gửi: {DateTime.Now:dd/MM/yyyy HH:mm}
  </p>
</div>
</body>
</html>";
        }
    }
}