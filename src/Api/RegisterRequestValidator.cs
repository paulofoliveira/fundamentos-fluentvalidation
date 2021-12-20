using DomainModel;
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
            // CascadeMode = CascadeMode.Stop; Ativar CascadeMode à nível de Validator

            // NotEmpty Rule considera valor nulo e vazio.
            // Regra aplicada no EmailAdress é a simplificada por padrão (.NET 5) por questão de ser impossível validar o e-mail com todas as possibilidades.

            //RuleFor(x => x.Name)
            //.Cascade(CascadeMode.Stop) // Ativar CascadeMode à nível de propriedade
            //.NotEmpty()
            //.Length(0, 200);

            RuleFor(x => x.Name).MustBeValueObject(StudentName.Create).When(x => x.Name != null);

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

            // Aplicando Herança (Inheritance)

            //RuleFor(x => x.Phone)
            //    .SetInheritanceValidator(x =>
            //    {
            //        x.Add(new USPhoneNumberDtoValidator());
            //        x.Add(new InternationalPhoneNumberDtoValidator());
            //    });

            // Aplicando RuleSet

            //RuleSet("Email", () =>
            //{
            //    RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            //});

            // Aplicando condições de validação

            //RuleFor(x => x.Phone).NotEmpty()
            //    .Must(x => Regex.IsMatch(x, "^[2-9][0-9]{9}$"))
            //    .When(x => x.Phone != null, ApplyConditionTo.CurrentValidator)
            //    .WithMessage("The phone number is incorrect");

            //When(x => x.Email != null, () =>
            // {
            //     RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            //     RuleFor(x => x.Phone).Null();
            // })
            //.Otherwise(() =>
            //{
            //    RuleFor(x => x.Phone).NotEmpty().Matches("^[2-9][0-9]{9}$");
            //    RuleFor(x => x.Email).Null();
            //});

            // When( ... é aplicado a todas as checagens. ApplyConditionTo.CurrentValidator deve ser passado como argumento para a Rule em questão

            When(x => x.Phone == null, () => { RuleFor(x => x.Email).NotEmpty(); });
            When(x => x.Email == null, () => { RuleFor(x => x.Phone).NotEmpty(); });

            //RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress().When(x => x.Email != null);

            // Validação combinada entre FluentValidation + Value Object com extensão implementada (MustBeValueObject)

            RuleFor(x => x.Email).MustBeValueObject(Email.Create).When(x => x.Email != null);

            RuleFor(x => x.Phone).NotEmpty().Matches("^[2-9][0-9]{9}$").When(x => x.Phone != null);
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
             //.Must(x => x?.Length >= 1 && x.Length <= 3)
             //.WithMessage("The number of address must be between and 3")
             .ListMustContainNumberOfItems(1, 3)
             .ForEach(address =>
             {
                 address.NotNull();
                 address.SetValidator(new AddressDtoValidator());
             });
        }
    }

    //public class USPhoneNumberDtoValidator : AbstractValidator<UsPhoneNumberDto>
    //{
    //    public USPhoneNumberDtoValidator()
    //    {
    //    }
    //}

    //public class InternationalPhoneNumberDtoValidator : AbstractValidator<InternationalPhoneNumberDto>
    //{
    //    public InternationalPhoneNumberDtoValidator()
    //    {
    //    }
    //}
}
