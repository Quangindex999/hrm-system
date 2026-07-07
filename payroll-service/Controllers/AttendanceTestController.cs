using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payroll_service.Services;
using payroll_service.DTOs;
using Microsoft.Extensions.Logging;

namespace payroll_service.Controllers
{
    [ApiController]
    [Route("api/test")]
    [Authorize]
    public class AttendanceTestController : ControllerBase
    {
        private readonly IAttendanceServiceClient _attendanceClient;
        private readonly IPayrollCalculatorService _payrollCalculator;
        private readonly ILogger<AttendanceTestController> _logger;

        public AttendanceTestController(
            IAttendanceServiceClient attendanceClient,
            IPayrollCalculatorService payrollCalculator,
            ILogger<AttendanceTestController> logger)
        {
            _attendanceClient = attendanceClient;
            _payrollCalculator = payrollCalculator;
            _logger = logger;
        }

        /// <summary>Test: lấy chấm công toàn bộ nhân viên theo tháng</summary>
        [HttpGet("attendance-monthly")]
        public async Task<IActionResult> GetMonthlyAttendance([FromQuery] string month = "2025-05")
        {
            var result = await _attendanceClient.GetMonthlyAttendanceSummaryAsync(month);
            if (result == null)
                return StatusCode(502, new { error = "Không lấy được dữ liệu từ Attendance Service" });
            return Ok(result);
        }

        /// <summary>Test: lấy chấm công 1 nhân viên</summary>
        [HttpGet("attendance-employee")]
        public async Task<IActionResult> GetEmployeeAttendance(
            [FromQuery] string employeeId = "EMP001",
            [FromQuery] string month = "2025-05")
        {
            var result = await _attendanceClient.GetAttendanceForEmployeeAsync(employeeId, month);
            if (result == null)
                return StatusCode(502, new { error = "Không lấy được dữ liệu chấm công" });
            return Ok(result);
        }

        /// <summary>Test: full flow HR + Attendance → tính lương</summary>
        [HttpGet("payroll-calculate")]
        public async Task<IActionResult> CalculatePayroll(
            [FromQuery] string employeeId = "EMP001",
            [FromQuery] string month = "2025-05")
        {
            var result = await _payrollCalculator.CalculateAsync(employeeId, month);
            return Ok(result);
        }
    }
}