namespace WorkAttendanceSystem.Application.DTOs
{
    public record EmployeeWithWarningsDto(
        Guid Id,
        string FullName,
        string Position,
        int MonthlyWarnings
    );
}
