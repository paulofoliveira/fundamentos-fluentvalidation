using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DomainModel
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<Email> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Failure<Email>("Valor é obrigatório");

            var email = input.Trim();

            if (email.Length > 150)
                return Result.Failure<Email>("Valor informado é muito grande");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Result.Failure<Email>("Email é inválido");

            return Result.Success(new Email(email));
        }
    }
}
