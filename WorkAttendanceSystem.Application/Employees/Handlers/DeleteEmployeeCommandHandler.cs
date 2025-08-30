using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkAttendanceSystem.Application.Employees.Commands;
using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Infrastructure.Persistence;

namespace WorkAttendanceSystem.Application.Employees.Handlers
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly AppDbContext _dbContext;

        public DeleteEmployeeCommandHandler(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
                ?? throw new DomainException("Employee not found");

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
