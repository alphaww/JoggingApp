using JoggingApp.Core.Jogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingApp.EntityFramework.Mappings
{
    public class JogLocationMappings : IEntityTypeConfiguration<JogLocation>
    {
        public void Configure(EntityTypeBuilder<JogLocation> builder)
        {
            builder.ToTable(nameof(JogLocation));
            builder.HasKey(x => x.JogId);

            builder.Property(x => x.Longitude).IsRequired(true);
            builder.Property(x => x.Latitude).IsRequired(true);

            builder.HasOne(x => x.Jog).WithOne(x => x.JogLocation);

        }
    }
}
