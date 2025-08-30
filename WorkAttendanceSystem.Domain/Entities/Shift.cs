using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Domain.ValueObjects;

namespace WorkAttendanceSystem.Domain.Entities
{
    public class Shift
    {
        public Guid Id { get; private set; }
        public ShiftPeriod Period { get; private set; }
        public WorkedHours? Hours { get; private set; }

        public Guid EmployeeId { get; private set; }
        public Employee Employee { get; private set; }

        public bool IsEnded => Period.End.HasValue;

        protected Shift() { }

        internal Shift(Guid id, ShiftPeriod period, Employee employee)
        {
            Id = id;
            Period = period ?? throw new DomainException("Shift period is required");
            Employee = employee ?? throw new DomainException("Employee is required");
            EmployeeId = employee.Id;
        }

        public void EndShift(DateTime endTime)
        {
            Period = Period.EndShift(endTime);
            Hours = WorkedHours.FromPeriod(Period);
        }
    }
}
