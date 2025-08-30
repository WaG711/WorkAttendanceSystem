using WorkAttendanceSystem.Domain.Exceptions;
using WorkAttendanceSystem.Domain.ValueObjects;

namespace WorkAttendanceSystem.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public FullName Name { get; private set; }
        public Position Position { get; private set; }

        private readonly List<Shift> _shifts = [];
        public IReadOnlyCollection<Shift> Shifts => _shifts.AsReadOnly();

        protected Employee() { }

        public Employee(FullName name, Position position)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new DomainException("Name is required");
            Position = position ?? throw new DomainException("Position is required");
        }

        public void ChangeName(FullName newName)
        {
            Name = newName ?? throw new DomainException("Name is required");
        }

        public void ChangePosition(Position newPosition)
        {
            Position = newPosition ?? throw new DomainException("Position is required");
        }

        public Shift StartShift(DateTime startTime)
        {
            if (_shifts.Any(s => !s.IsEnded))
            {
                throw new DomainException("Employee already has an active shift");
            }

            var shift = new Shift(Guid.NewGuid(), new ShiftPeriod(startTime), this);
            _shifts.Add(shift);
            return shift;
        }

        public void EndShift(Guid shiftId, DateTime endTime)
        {
            var shift = _shifts.FirstOrDefault(s => s.Id == shiftId)
                ?? throw new DomainException("Shift not found");

            shift.EndShift(endTime);
        }
    }
}
