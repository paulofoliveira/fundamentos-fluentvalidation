using CSharpFunctionalExtensions;
using DomainModel;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Api
{
    public static class CustomValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
                   this IRuleBuilder<T, string> ruleBuilder,
                   Func<string, Result<TValueObject, Error>> factoryMethod)
                   where TValueObject : ValueObject
        {
            return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error.Message);
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainNumberOfItems<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (min.HasValue && list.Count < min)
                {
                    context.AddFailure($"The list must contain {min.Value} items or more. It contains {list.Count} items.");
                }

                if (max.HasValue && list.Count > max)
                {
                    context.AddFailure($"The list must contain {max.Value} items or fewer. It contains {list.Count} items.");
                }
            });
        }
    }
}
