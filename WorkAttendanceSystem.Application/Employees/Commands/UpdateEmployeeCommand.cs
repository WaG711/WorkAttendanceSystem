using MediatR;
using WorkAttendanceSystem.Application.DTOs;

namespace WorkAttendanceSystem.Application.Employees.Commands
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string LastName,
        string FirstName,
        string MiddleName,
        string Position
    ) : IRequest<EmployeeDto>;
}
