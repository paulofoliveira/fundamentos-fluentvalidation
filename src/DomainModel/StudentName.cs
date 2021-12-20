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

        public static Result<StudentName> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Failure<StudentName>("Valor é obrigatório");

            var email = input.Trim();

            if (email.Length > 200)
                return Result.Failure<StudentName>("Valor informado é muito grande");

            return Result.Success(new StudentName(email));
        }
    }
}
