using FluentValidation.Results;

namespace DDDPlayground.Shared.Validation
{
    public interface IValidationService<T>
    {
        Task<ValidationResult> ValidateAsync(T instance);
    }
}
