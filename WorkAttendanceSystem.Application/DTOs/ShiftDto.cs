namespace WorkAttendanceSystem.Application.DTOs
{
    public record ShiftDto(
        Guid Id,
        DateTime StartTime,
        DateTime? EndTime,
        double? WorkedHours,
        Guid EmployeeId
    );
}
