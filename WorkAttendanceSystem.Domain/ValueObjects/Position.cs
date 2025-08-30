using WorkAttendanceSystem.Domain.Exceptions;

namespace WorkAttendanceSystem.Domain.ValueObjects
{
    public sealed class Position : ValueObject
    {
        public static readonly Position Manager = new("Manager");
        public static readonly Position Engineer = new("Engineer");
        public static readonly Position CandleTester = new("Candle Tester");

        private static readonly IReadOnlyCollection<Position> _all =
        [
            Manager, Engineer, CandleTester
        ];

        public string Name { get; }

        private Position(string name) => Name = name;

        public static Position FromName(string name)
        {
            var pos = _all.FirstOrDefault(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            if (pos == null)
            {
                throw new DomainException($"Invalid position: {name}");
            }

            return pos;
        }

        public static IReadOnlyCollection<Position> List() => _all;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }

        public override string ToString() => Name;
    }
}
