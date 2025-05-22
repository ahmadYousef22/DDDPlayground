using DDDPlayground.Domain.Entities;
using DDDPlayground.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDPlayground.Infrastructure.Persistence.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", SchemaName.Identity);
            builder.HasKey(x => x.Id);
        }
    }
}
