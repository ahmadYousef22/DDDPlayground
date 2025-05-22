using DDDPlayground.Domain.Intefaces.Base;
using DDDPlayground.Domain.Intefaces.Users;
using DDDPlayground.Infrastructure.Persistence.EFCore;
using DDDPlayground.Infrastructure.Repositories.Base;
using DDDPlayground.Infrastructure.Repositories.Users;
using DDDPlayground.Infrastructure.Validation;
using DDDPlayground.Shared.Enums;
using DDDPlayground.Shared.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDPlayground.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString"), 
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", SchemaName.Migrations); 
            }));

            services.AddValidatorsFromAssembly(typeof(ServiceRegistration).Assembly);

            #region DI
            services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            return services;
        }
    }
}
