namespace payroll_service.DTOs
{
    public class AttendanceDTO
    {
        public string? EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        // Ngày công — map đúng field nhóm 2 trả về
        public int WorkingDays { get; set; }
        public int PresentDays { get; set; }
        public int LeaveDays { get; set; }
        public int AbsentDays { get; set; }
        public int StandardWorkingDays { get; set; } = 26;

        // Tăng ca
        public double OvertimeHours { get; set; }
        public double OvertimeNormalHours { get; set; }
        public double OvertimeWeekendHours { get; set; }
        public double OvertimeHolidayHours { get; set; }
        public int PaidLeaveDays { get; set; }
        public int UnpaidLeaveDays { get; set; }

        // Đi muộn
        public int LateArrivals { get; set; }
        public int TotalLateMinutes { get; set; }

        public string? Status { get; set; }
        public DateTime? CalculatedAt { get; set; }

        // ── Compat methods cho PayrollCalculatorService ──
        public double GetStandardDays() => StandardWorkingDays;
        public double GetOvertimeNormal() => OvertimeNormalHours;
        public double GetOvertimeWeekend() => OvertimeWeekendHours;
        public double GetOvertimeHoliday() => OvertimeHolidayHours;
        public double GetPaidLeaveDays() => PaidLeaveDays;
        public double GetUnpaidLeaveDays() => UnpaidLeaveDays;
    }

    public class AttendanceSummaryDTO
    {
        public string Month { get; set; } = string.Empty;
        public List<AttendanceDTO> Employees { get; set; } = new();
    }
}