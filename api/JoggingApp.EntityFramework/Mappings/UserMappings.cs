using JoggingApp.Core;
using JoggingApp.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingApp.EntityFramework.Mappings
{
    public class UserMappings : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Password).IsRequired();

            builder.HasIndex(x => x.Email).IncludeProperties(x => new { x.Id, x.Password, x.State }).IsUnique();
            builder.HasMany(x => x.ActivationTokens).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
