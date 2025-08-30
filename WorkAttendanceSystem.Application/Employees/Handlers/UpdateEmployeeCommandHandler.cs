using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Employees.Commands;
using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Domain.ValueObjects;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Employees.Handlers
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        private readonly AppDbContext _dbContext;

        public UpdateEmployeeCommandHandler(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
                ?? throw new DomainException("Employee not found");

            employee.ChangeName(new FullName(request.LastName, request.FirstName, request.MiddleName));
            employee.ChangePosition(Position.FromName(request.Position));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new EmployeeDto(employee.Id, employee.Name.LastName, employee.Name.FirstName, employee.Name.MiddleName, employee.Position.Name);
        }
    }
}
