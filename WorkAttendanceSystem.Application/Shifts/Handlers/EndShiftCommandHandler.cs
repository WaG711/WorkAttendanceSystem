using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Shifts.Commands;
using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Shifts.Handlers
{
    public class EndShiftCommandHandler : IRequestHandler<EndShiftCommand, ShiftDto>
    {
        private readonly AppDbContext _dbContext;

        public EndShiftCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShiftDto> Handle(EndShiftCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken)
                ?? throw new DomainException("Employee not found");

            var activeShift = employee.Shifts.FirstOrDefault(s => !s.IsEnded)
                ?? throw new DomainException("Cannot end shift: no active shift found");

            employee.EndShift(activeShift.Id, request.EndTime);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ShiftDto(activeShift.Id, activeShift.Period.Start, activeShift.Period.End, activeShift.Hours?.Value, employee.Id);
        }
    }
}
