using MediatR;
using WorkAttendanceSystem.Application.DTOs;

namespace WorkAttendanceSystem.Application.Employees.Commands
{
    public record CreateEmployeeCommand(
        string LastName,
        string FirstName,
        string MiddleName,
        string Position
    ) : IRequest<EmployeeDto>;
}
