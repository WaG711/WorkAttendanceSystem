using MediatR;

namespace WorkAttendanceSystem.Application.Employees.Commands
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<Unit>;
}
