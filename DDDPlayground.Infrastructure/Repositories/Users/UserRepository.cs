using DDDPlayground.Domain.Entities;
using DDDPlayground.Domain.Intefaces.Users;
using DDDPlayground.Infrastructure.Persistence.EFCore;
using DDDPlayground.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DDDPlayground.Infrastructure.Repositories.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(DatabaseContext context) : base(context)
        {
        }
        public Task<User?> GetByUsernameAsync(string username)
        {
            return _context.Set<User>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Name == username);
        }
    }
}
