using MediatR;
using WorkAttendanceSystem.Application.DTOs;
using WorkAttendanceSystem.Application.Employees.Commands;
using WorkAttendanceSystem.Domain.Entities;
using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Domain.ValueObjects;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Employees.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly AppDbContext _dbContext;

        public CreateEmployeeCommandHandler(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.Position))
            {
                throw new DomainException("LastName, FirstName and Position are required");
            }

            var fullName = new FullName(request.LastName, request.FirstName, request.MiddleName);
            var position = Position.FromName(request.Position);

            var employee = new Employee(fullName, position);

            await _dbContext.Employees.AddAsync(employee, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new EmployeeDto(employee.Id, fullName.LastName, fullName.FirstName, fullName.MiddleName, position.Name);
        }
    }
}
