using MediatR;
using WorkAttendanceSystem.Application.DTOs;

namespace WorkAttendanceSystem.Application.Shifts.Commands
{
    public record EndShiftCommand(Guid EmployeeId, DateTime EndTime) : IRequest<ShiftDto>;
}
