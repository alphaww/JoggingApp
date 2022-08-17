using JoggingApp.Core;
using JoggingApp.Core.Jogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingApp.EntityFramework.Mappings
{
    public class JogMappings : IEntityTypeConfiguration<Jog>
    {
        public void Configure(EntityTypeBuilder<Jog> builder)
        {
            builder.ToTable(nameof(Jog));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date).IsRequired(true);
            builder.Property(x => x.Distance).IsRequired(true);
            builder.Property(x => x.Time).IsRequired(true);

            builder.HasIndex(x => x.Date).IncludeProperties(x => new { x.Id, x.Distance, x.Time, x.UserId }).IsUnique(false);

            builder.HasOne(x => x.User);

        }
    }
}
