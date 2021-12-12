using FluentValidation;

namespace Api
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // NotEmpty Rule considera valor nulo e vazio.
            // Regra aplicada no EmailAdress é a simplificada por padrão (.NET 5) por questão de ser impossível validar o e-mail com todas as possibilidades.

            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            RuleFor(x => x.Address).NotEmpty().Length(0, 150);
        }
    }
}
