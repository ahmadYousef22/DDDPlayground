using DDDPlayground.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace DDDPlayground.Infrastructure.Validation
{
    public class ValidationService<T> : IValidationService<T>
    {
        private readonly IValidator<T> _validator;

        public ValidationService(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            return await _validator.ValidateAsync(instance);
        }
    }
}
