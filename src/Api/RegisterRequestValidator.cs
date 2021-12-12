using FluentValidation;

namespace Api
{
    public class EditPersonalInfoRequestValidator : AbstractValidator<EditPersonalInfoRequest>
    {
        public EditPersonalInfoRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 200);

            //RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressDtoValidator());

            //RuleFor(x => x.Addresses).NotNull()
            //    .Must(x => x?.Length >= 1 && x.Length <= 3)
            //    .WithMessage("The number of address must be between and 3")
            //    .ForEach(address =>
            //    {
            //        address.NotNull();
            //        address.SetValidator(new AddressDtoValidator());
            //    });

            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesCollectionDtoValidator());
        }
    }
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // NotEmpty Rule considera valor nulo e vazio.
            // Regra aplicada no EmailAdress é a simplificada por padrão (.NET 5) por questão de ser impossível validar o e-mail com todas as possibilidades.

            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();

            // .When(x=> x.Address != null) checa se Address não é nula. Se condição é verdadeira pois no input poderia passar Address como null.

            // Inline validation rules

            //RuleFor(x => x.Address).NotNull();
            //RuleFor(x => x.Address.Street).NotEmpty().Length(0, 150).When(x => x.Address != null);
            //RuleFor(x => x.Address.City).NotEmpty().Length(0, 40).When(x => x.Address != null);
            //RuleFor(x => x.Address.State).NotEmpty().Length(0, 2).When(x => x.Address != null);
            //RuleFor(x => x.Address.ZipCode).NotEmpty().Length(0, 5).When(x => x.Address != null);

            // Para evitar duplicação de código o ideal é criar um Validator separado para AddressDto (Abaixo) tendo um código mais limpo, evitando duplicação e verbosidade.

            //RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressDtoValidator());

            //RuleFor(x => x.Addresses).NotNull()
            //.Must(x => x?.Length >= 1 && x.Length <= 3)
            //.WithMessage("The number of address must be between and 3")
            //.ForEach(address =>
            //{
            //    address.NotNull();
            //    address.SetValidator(new AddressDtoValidator());
            //});

            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesCollectionDtoValidator());
        }
    }
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street).NotEmpty().Length(0, 150);
            RuleFor(x => x.City).NotEmpty().Length(0, 40);
            RuleFor(x => x.State).NotEmpty().Length(0, 2);
            RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5);
        }
    }
    public class AddressesCollectionDtoValidator : AbstractValidator<AddressDto[]>
    {
        public AddressesCollectionDtoValidator()
        {
            RuleFor(x => x)
             .Must(x => x?.Length >= 1 && x.Length <= 3)
             .WithMessage("The number of address must be between and 3")
             .ForEach(address =>
             {
                 address.NotNull();
                 address.SetValidator(new AddressDtoValidator());
             });
        }
    }
}
