namespace WorkAttendanceSystem.Application.DTOs
{
    public record EmployeeDto(
        Guid Id,
        string LastName,
        string FirstName,
        string MiddleName,
        string Position
    );
}
