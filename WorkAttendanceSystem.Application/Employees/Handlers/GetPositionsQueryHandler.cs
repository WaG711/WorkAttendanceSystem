using MediatR;
using WorkAttendanceSystem.Application.Employees.Queries;
using WorkAttendanceSystem.Domain.ValueObjects;

namespace WorkAttendanceSystem.Application.Employees.Handlers
{
    public class GetPositionsQueryHandler : IRequestHandler<GetPositionsQuery, List<string>>
    {
        public Task<List<string>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
        {
            var positions = Position.List().Select(p => p.Name).ToList();
            return Task.FromResult(positions);
        }
    }
}
