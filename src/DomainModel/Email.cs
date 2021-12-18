using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace DomainModel
{
    public class Email : ValueObject
    {
        public Email(string value)
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
