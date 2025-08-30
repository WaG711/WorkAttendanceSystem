using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Shifts.Commands;
using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Shifts.Handlers
{
    public class StartShiftCommandHandler : IRequestHandler<StartShiftCommand, ShiftDto>
    {
        private readonly AppDbContext _dbContext;

        public StartShiftCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShiftDto> Handle(StartShiftCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken)
                ?? throw new DomainException("Employee not found");


            if (employee.Shifts.Any(s => !s.IsEnded))
            {
                throw new DomainException("Cannot start new shift: previous shift not ended");
            }

            var shift = employee.StartShift(request.StartTime);

            _dbContext.Shifts.Add(shift);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ShiftDto(shift.Id, shift.Period.Start, shift.Period.End, shift.Hours?.Value, employee.Id);
        }
    }
}
