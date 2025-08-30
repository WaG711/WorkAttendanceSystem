using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.Domain.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public string LastName { get; }
        public string FirstName { get; }
        public string MiddleName { get; }

        public FullName(string lastName, string firstName, string middleName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("LastName is required");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException("FirstName is required");
            }

            if (string.IsNullOrWhiteSpace(middleName))
            {
                throw new DomainException("MiddleName is required");
            }

            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LastName;
            yield return FirstName;
            yield return MiddleName;
        }

        public override string ToString() => $"{LastName} {FirstName} {MiddleName}";
    }
}
