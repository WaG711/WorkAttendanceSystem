using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Shifts.Commands;

namespace WorkAttendanceSystem.API.Controllers
{
    [ApiController]
    [Route("api/kpp")]
    public class KPPController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KPPController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("start")]
        public async Task<ActionResult<ShiftDto>> StartShift([FromBody] StartShiftRequest request)
        {
            var command = new StartShiftCommand(request.EmployeeId, request.StartTime);
            var shift = await _mediator.Send(command);
            return Ok(shift);
        }

        [HttpPost("end")]
        public async Task<ActionResult<ShiftDto>> EndShift([FromBody] EndShiftRequest request)
        {
            var command = new EndShiftCommand(request.EmployeeId, request.EndTime);
            var shift = await _mediator.Send(command);
            return Ok(shift);
        }
    }

    public record StartShiftRequest(Guid EmployeeId, DateTime StartTime);
    public record EndShiftRequest(Guid EmployeeId, DateTime EndTime);
}
