using DDDPlayground.Domain.Entities;
using DDDPlayground.Shared.Validation;

namespace DDDPlayground.Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IValidationService<User> _validator;
        public UserAppService(IValidationService<User> validator)
        {
            _validator = validator;
        }
    }
}
