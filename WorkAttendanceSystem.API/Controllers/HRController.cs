using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Employees.Commands;
using WorkAttendanceSystem.Application.Employees.Queries;

namespace WorkAttendanceSystem.API.Controllers
{
    [ApiController]
    [Route("api/hr")]
    public class HRController : Controller
    {
        private readonly IMediator _mediator;

        public HRController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeCommand command)
        {
            var employee = await _mediator.Send(command);
            return Ok(employee);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeDto>> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            var employee = await _mediator.Send(command);
            return Ok(employee);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeWithWarningsDto>>> GetAll([FromQuery] string? position)
        {
            var employees = await _mediator.Send(new GetEmployeesQuery(position));
            return Ok(employees);
        }

        [HttpGet("positions")]
        public async Task<ActionResult<List<string>>> GetPositions()
        {
            var positions = await _mediator.Send(new GetPositionsQuery());
            return Ok(positions);
        }
    }
}
