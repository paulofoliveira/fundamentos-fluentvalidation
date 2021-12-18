using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace DomainModel
{
    public class StudentName : ValueObject
    {
        public StudentName(string value)
        {
            Value = value;
        }

        public string Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
