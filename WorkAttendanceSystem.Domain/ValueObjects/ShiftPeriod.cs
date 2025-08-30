using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.Domain.ValueObjects
{
    public sealed class ShiftPeriod : ValueObject
    {
        public DateTime Start { get; }
        public DateTime? End { get; }

        public ShiftPeriod(DateTime start, DateTime? end = null)
        {
            if (end.HasValue && end.Value < start)
            {
                throw new DomainException("Shift end cannot be earlier than start");
            }

            Start = start;
            End = end;
        }

        public ShiftPeriod EndShift(DateTime endTime)
        {
            if (End.HasValue)
            {
                throw new DomainException("Shift already ended");
            }

            return new ShiftPeriod(Start, endTime);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start;
            yield return End ?? DateTime.MinValue;
        }
    }
}
