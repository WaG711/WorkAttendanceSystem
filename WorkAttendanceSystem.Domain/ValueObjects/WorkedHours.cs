using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.Domain.ValueObjects
{
    public sealed class WorkedHours : ValueObject
    {
        public double Value { get; }

        private WorkedHours(double value)
        {
            if (value < 0)
            {
                throw new DomainException("Worked hours cannot be negative");
            }

            Value = value;
        }

        public static WorkedHours FromPeriod(ShiftPeriod period)
        {
            if (!period.End.HasValue)
            {
                throw new DomainException("Cannot calculate worked hours before shift ends");
            }

            var hours = (period.End.Value - period.Start).TotalHours;
            return new WorkedHours(hours);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => $"{Value:F2}h";
    }
}
