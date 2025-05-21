using Microsoft.EntityFrameworkCore;

namespace DDDPlayground.Infrastructure.Persistence.EFCore
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
