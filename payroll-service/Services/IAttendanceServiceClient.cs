using payroll_service.DTOs;

namespace payroll_service.Services
{
    public interface IAttendanceServiceClient
    {
        Task<AttendanceDTO?> GetAttendanceForEmployeeAsync(string employeeId, string month);
        Task<AttendanceSummaryDTO?> GetMonthlyAttendanceSummaryAsync(string month);
        Task<string> GetHistoryAsync(string employeeId, int month, int year); // ✅ thêm dòng này
    }
}