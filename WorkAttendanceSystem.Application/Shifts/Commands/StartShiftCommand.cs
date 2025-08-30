using MediatR;
using WorkAttendanceSystem.Application.DTOs;

namespace WorkAttendanceSystem.Application.Shifts.Commands
{
    public record StartShiftCommand(Guid EmployeeId, DateTime StartTime) : IRequest<ShiftDto>;
}
