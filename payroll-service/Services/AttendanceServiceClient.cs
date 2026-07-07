using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using payroll_service.DTOs;

namespace payroll_service.Services
{
    public class AttendanceServiceClient : IAttendanceServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AttendanceServiceClient> _logger;

        private string BaseUrl => _configuration["AttendanceService:BaseUrl"]?.TrimEnd('/') ?? "";
        private string JwtSecret => _configuration["Jwt:Secret"] ?? "";
        private string JwtIssuer => _configuration["Jwt:Issuer"] ?? "";
        private string JwtAudience => _configuration["Jwt:Audience"] ?? "";
        private int JwtExpiry => int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

        public AttendanceServiceClient(
            IConfiguration configuration,
            ILogger<AttendanceServiceClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // ================================================================
        // TẠO HTTP CLIENT (bỏ qua SSL tự ký)
        // ================================================================
        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return new HttpClient(handler);
        }

        // ================================================================
        // TẠO REQUEST KÈM JWT
        // ================================================================
        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            var req = new HttpRequestMessage(method, url);
            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", GenerateJwt());
            return req;
        }

        // ================================================================
        // LẤY CHẤM CÔNG 1 NHÂN VIÊN THEO THÁNG
        // Endpoint: GET /api/attendance/monthly-summary/{id}?month=6&year=2026
        // ================================================================
        public async Task<AttendanceDTO?> GetAttendanceForEmployeeAsync(string employeeId, string month)
        {
            // month truyền vào dạng "2026-06" → tách thành month=6, year=2026
            int monthInt = DateTime.Now.Month;
            int yearInt = DateTime.Now.Year;

            if (!string.IsNullOrEmpty(month) && month.Contains("-"))
            {
                var parts = month.Split('-');
                if (parts.Length == 2)
                {
                    int.TryParse(parts[0], out yearInt);
                    int.TryParse(parts[1], out monthInt);
                }
            }

            var url = $"{BaseUrl}/api/attendance/monthly-summary/{employeeId}?month={monthInt}&year={yearInt}";
            _logger.LogInformation("[AttendanceClient] GET employee → {Url}", url);

            try
            {
                using var client = CreateHttpClient();
                using var request = CreateRequest(HttpMethod.Get, url);
                using var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[AttendanceClient] HTTP {Status} — {Id} {Month}/{Year}",
                        (int)response.StatusCode, employeeId, monthInt, yearInt);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("[AttendanceClient] Response: {Json}", json);
                return ParseSingle(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AttendanceClient] Lỗi gọi Attendance — {Id}", employeeId);
                return null;
            }
        }

        // ================================================================
        // LẤY CHẤM CÔNG TOÀN BỘ NHÂN VIÊN THEO THÁNG
        // ================================================================
        public async Task<AttendanceSummaryDTO?> GetMonthlyAttendanceSummaryAsync(string month)
        {
            int monthInt = DateTime.Now.Month;
            int yearInt = DateTime.Now.Year;

            if (!string.IsNullOrEmpty(month) && month.Contains("-"))
            {
                var parts = month.Split('-');
                if (parts.Length == 2)
                {
                    int.TryParse(parts[0], out yearInt);
                    int.TryParse(parts[1], out monthInt);
                }
            }

            var url = $"{BaseUrl}/api/attendance/monthly-summary?month={monthInt}&year={yearInt}";
            _logger.LogInformation("[AttendanceClient] GET monthly → {Url}", url);

            try
            {
                using var client = CreateHttpClient();
                using var request = CreateRequest(HttpMethod.Get, url);
                using var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[AttendanceClient] HTTP {Status} — monthly {Month}/{Year}",
                        (int)response.StatusCode, monthInt, yearInt);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return ParseSummary(json, month);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AttendanceClient] Lỗi monthly — {Month}", month);
                return null;
            }
        }

        // ================================================================
        // LẤY LỊCH SỬ CHẤM CÔNG
        // Endpoint: GET /api/attendance/history/{employeeId}?month=6&year=2026
        // ================================================================
        public async Task<string> GetHistoryAsync(string employeeId, int month, int year)
        {
            var url = $"{BaseUrl}/api/attendance/history/{employeeId}?month={month}&year={year}";
            _logger.LogInformation("[AttendanceClient] GET history → {Url}", url);

            try
            {
                using var client = CreateHttpClient();
                using var request = CreateRequest(HttpMethod.Get, url);
                using var response = await client.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("[AttendanceClient] History response: {Json}", json);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AttendanceClient] Lỗi history — {Id}", employeeId);
                return JsonSerializer.Serialize(new { error = ex.Message });
            }
        }

        // ================================================================
        // HELPERS
        // ================================================================

        private string GenerateJwt()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "PayrollService"),
                new Claim(ClaimTypes.Role, "InternalService"),
            };

            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(JwtExpiry),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AttendanceDTO? ParseSingle(string json)
        {
            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // Unwrap envelope nếu có: { data: {...} } hoặc { result: {...} }
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("data", out var d)) root = d;
                    else if (root.TryGetProperty("result", out var r)) root = r;
                }

                return JsonSerializer.Deserialize<AttendanceDTO>(root.GetRawText(), opts);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "[AttendanceClient] JSON parse error (single)");
                return null;
            }
        }

        private AttendanceSummaryDTO? ParseSummary(string json, string month)
        {
            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                if (json.TrimStart().StartsWith("["))
                {
                    var list = JsonSerializer.Deserialize<List<AttendanceDTO>>(json, opts) ?? new();
                    return new AttendanceSummaryDTO { Month = month, Employees = list };
                }

                return JsonSerializer.Deserialize<AttendanceSummaryDTO>(json, opts);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "[AttendanceClient] JSON parse error (summary)");
                return null;
            }
        }
    }
}