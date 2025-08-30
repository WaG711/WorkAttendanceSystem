using MediatR;

namespace WorkAttendanceSystem.Application.Employees.Queries
{
    public record GetPositionsQuery() : IRequest<List<string>>;
}
