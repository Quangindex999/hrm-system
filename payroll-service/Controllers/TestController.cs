// ============================================================
// TEST CONTROLLER
// Endpoint test kết nối với HR Service & Attendance Service
// ============================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payroll_service.Services;

namespace payroll_service.Controllers;

[ApiController]
[Route("api/v1/payroll/[controller]")]
public class TestController : ControllerBase
{
    private readonly IHrServiceClient _hrClient;
    private readonly IAttendanceServiceClient _attendanceClient;
    private readonly ILogger<TestController> _logger;

    public TestController(
        IHrServiceClient hrClient,
        IAttendanceServiceClient attendanceClient,
        ILogger<TestController> logger)
    {
        _hrClient = hrClient;
        _attendanceClient = attendanceClient;
        _logger = logger;
    }

    /// <summary>
    /// Kiểm tra service đang chạy (không cần xác thực)
    /// GET /api/v1/payroll/test/ping
    /// </summary>
    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        _logger.LogInformation("[TestController] GET ping");
        return Ok(new
        {
            status = "OK",
            service = "Payroll Service",
            time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            message = "Payroll Service đang chạy bình thường"
        });
    }

    /// <summary>
    /// Lấy danh sách nhân viên từ HR Service
    /// GET /api/v1/payroll/test/hr-employees
    /// </summary>
    [AllowAnonymous]
    [HttpGet("hr-employees")]
    public async Task<IActionResult> GetHrEmployees()
    {
        _logger.LogInformation("[TestController] GET hr-employees");
        try
        {
            var result = await _hrClient.GetEmployeesAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                _logger.LogWarning("[TestController] HR Service trả về dữ liệu rỗng");
                return NoContent();
            }

            _logger.LogInformation("[TestController] Lấy dữ liệu HR Service thành công");
            return Content(result, "application/json");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "[TestController] Không thể kết nối đến HR Service");
            return StatusCode(503, new
            {
                status = "Error",
                message = "Không thể kết nối đến HR Service",
                detail = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[TestController] Lỗi khi gọi HR Service");
            return StatusCode(500, new
            {
                status = "Error",
                message = "Lỗi nội bộ server",
                detail = ex.Message
            });
        }
    }

    /// <summary>
    /// Lấy tóm tắt chấm công tháng của 1 nhân viên từ Attendance Service
    /// GET /api/v1/payroll/test/attendance?employeeId=xxx&month=6&year=2026
    /// </summary>
    [AllowAnonymous]
    [HttpGet("attendance")]
    public async Task<IActionResult> GetAttendance(
        [FromQuery] string employeeId = "00000000-0000-0000-0000-000000000001",
        [FromQuery] int month = 6,
        [FromQuery] int year = 2026)
    {
        _logger.LogInformation(
            "[TestController] GET attendance — employeeId={Id}, month={Month}, year={Year}",
            employeeId, month, year);
        try
        {
            var monthStr = $"{year}-{month:D2}";
            var result = await _attendanceClient.GetAttendanceForEmployeeAsync(employeeId, monthStr);

            if (result == null)
            {
                return NotFound(new
                {
                    status = "NotFound",
                    message = $"Không có dữ liệu chấm công cho nhân viên {employeeId} tháng {month}/{year}"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[TestController] Lỗi khi gọi Attendance Service");
            return StatusCode(500, new
            {
                status = "Error",
                message = "Lỗi khi kết nối Attendance Service",
                detail = ex.Message
            });
        }
    }

    /// <summary>
    /// Lấy tóm tắt chấm công cả tháng từ Attendance Service
    /// GET /api/v1/payroll/test/attendance-monthly?month=2026-06
    /// </summary>
    [AllowAnonymous]
    [HttpGet("attendance-monthly")]
    public async Task<IActionResult> GetMonthlyAttendance(
        [FromQuery] string month = "2026-06")
    {
        _logger.LogInformation("[TestController] GET attendance-monthly — month={Month}", month);
        try
        {
            var result = await _attendanceClient.GetMonthlyAttendanceSummaryAsync(month);

            if (result == null)
            {
                return NotFound(new
                {
                    status = "NotFound",
                    message = $"Không có dữ liệu chấm công tháng {month}"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[TestController] Lỗi khi gọi Attendance monthly");
            return StatusCode(500, new
            {
                status = "Error",
                message = "Lỗi khi kết nối Attendance Service",
                detail = ex.Message
            });
        }
    }

    /// <summary>
    /// Test kết nối tất cả service cùng lúc
    /// GET /api/v1/payroll/test/check-all
    /// </summary>
    [AllowAnonymous]
    [HttpGet("check-all")]
    public async Task<IActionResult> CheckAll()
    {
        _logger.LogInformation("[TestController] GET check-all");
        // Test HR Service
        string hrStatus = "OK";
        string hrMessage = "Kết nối thành công";
        try
        {
            var hr = await _hrClient.GetEmployeesAsync();
            if (string.IsNullOrWhiteSpace(hr))
            {
                hrStatus = "Warning";
                hrMessage = "Kết nối được nhưng không có dữ liệu";
            }
        }
        catch (Exception ex)
        {
            hrStatus = "Error";
            hrMessage = ex.Message;
        }

        // Test Attendance Service
        string attStatus = "OK";
        string attMessage = "Kết nối thành công";
        try
        {
            var att = await _attendanceClient.GetMonthlyAttendanceSummaryAsync("2026-06");
            if (att == null)
            {
                attStatus = "Warning";
                attMessage = "Kết nối được nhưng không có dữ liệu";
            }
        }
        catch (Exception ex)
        {
            attStatus = "Error";
            attMessage = ex.Message;
        }

        return Ok(new
        {
            checkedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            services = new[]
            {
                new
                {
                    name = "Payroll Service (Nhóm 3)",
                    status = "OK",
                    message = "Đang chạy bình thường"
                },
                new
                {
                    name = "HR Core Service (Nhóm 1)",
                    status = hrStatus,
                    message = hrMessage
                },
                new
                {
                    name = "Attendance Service (Nhóm 2)",
                    status = attStatus,
                    message = attMessage
                }
            }
        });
    }
}