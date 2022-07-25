using JoggingApp.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingApp.EntityFramework.Mappings
{
    public class UserActivationTokenMappings : IEntityTypeConfiguration<UserActivationToken>
    {
        public void Configure(EntityTypeBuilder<UserActivationToken> builder)
        {
            builder.ToTable(nameof(UserActivationToken));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.ActivationTokens).HasForeignKey(x => x.UserId);
        }
    }
}
