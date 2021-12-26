using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;

namespace DomainModel
{
    public class State : ValueObject
    {
        //public static readonly State SP = new("SP");
        //public static readonly State RJ = new("RJ");
        //public static readonly State MG = new("MG");

        //public static readonly State[] All = { SP, RJ, MG };

        private State(string value)
        {
            Value = value;
        }

        public string Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<State, Error> Create(string input, string[] allStates)
        {
            if (string.IsNullOrEmpty(input))
                return Errors.General.ValueIsRequired();

            var name = input.Trim().ToUpper();

            if (name.Length > 2)
                return Errors.General.InvalidLength("State");

            if (!allStates.Any(state => state == name))
                return Errors.General.ValueIsInvalid();

            return new State(name);
        }
    }
}
