using DDDPlayground.Domain.Intefaces.Base;
using DDDPlayground.Domain.Intefaces.Users;
using DDDPlayground.Infrastructure.Persistence.EFCore;
using DDDPlayground.Infrastructure.Repositories.Base;
using DDDPlayground.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDPlayground.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
