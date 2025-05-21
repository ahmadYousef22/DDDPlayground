using DDDPlayground.Domain.Entities;
using DDDPlayground.Domain.Intefaces.Base;

namespace DDDPlayground.Domain.Intefaces.Users
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
