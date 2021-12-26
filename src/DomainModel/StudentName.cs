using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace DomainModel
{
    public class StudentName : ValueObject
    {
        private StudentName(string value)
        {
            Value = value;
        }

        public string Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<StudentName, Error> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Errors.General.ValueIsRequired();

            var email = input.Trim();

            if (email.Length > 200)
                return Errors.General.InvalidLength("Email");

            return new StudentName(email);
        }
    }
}
