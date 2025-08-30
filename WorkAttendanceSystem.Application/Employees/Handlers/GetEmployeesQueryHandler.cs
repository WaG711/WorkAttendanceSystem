using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Employees.Queries;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Employees.Handlers
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeWithWarningsDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetEmployeesQueryHandler(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<List<EmployeeWithWarningsDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Employees
            .Include(e => e.Shifts)
            .AsQueryable();

            if (!string.IsNullOrEmpty(request.Position))
            {
                query = query.Where(e => e.Position.Name == request.Position);
            }

            var employees = await query.ToListAsync(cancellationToken);

            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

            var result = employees.Select(e =>
            {
                int warnings = e.Shifts
                    .Where(s => s.Period.Start >= startOfMonth && s.Period.Start <= endOfMonth)
                    .Count(s =>
                    {
                        var start = s.Period.Start;
                        var end = s.Period.End ?? start;

                        if (e.Position.Name != "Candle Tester")
                        {
                            if (start.TimeOfDay > TimeSpan.FromHours(9) || end.TimeOfDay < TimeSpan.FromHours(18))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (end.TimeOfDay < TimeSpan.FromHours(21))
                            {
                                return true;
                            }
                        }

                        return false;
                    });

                return new EmployeeWithWarningsDto(
                    e.Id,
                    e.Name.ToString(),
                    e.Position.Name,
                    warnings
                );
            }).ToList();

            return result;
        }
    }
}
