using MediatR;
using WorkAttendanceSystem.Application.DTOs;

namespace WorkAttendanceSystem.Application.Employees.Queries
{
    public record GetEmployeesQuery(string? Position = null) : IRequest<List<EmployeeWithWarningsDto>>;
}
