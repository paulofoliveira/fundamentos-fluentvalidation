using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Address : Entity
    {
        private Address(string street, string city, State state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Street { get; }
        public string City { get; }
        public State State { get; }
        public string ZipCode { get; }

        public static Result<Address> Create(string street, string city, string state, string zipCode)
        {
            var stateObject = State.Create(state).Value;

            street = (street ?? string.Empty).Trim();
            city = (city ?? string.Empty).Trim();
            zipCode = (zipCode ?? string.Empty).Trim();

            if (street.Length < 1 || street.Length > 100)
                return Result.Failure<Address>("Endereço com tamanho inválido");

            if (city.Length < 1 || city.Length > 40)
                return Result.Failure<Address>("Cidade com tamanho inválido");

            if (state.Length < 1 || state.Length > 2)
                return Result.Failure<Address>("Estado com tamanho inválido");

            if (zipCode.Length < 1 || zipCode.Length > 5)
                return Result.Failure<Address>("CEP com tamanho inválido");

            return new Address(street, city, stateObject, zipCode);
        }
    }
}
