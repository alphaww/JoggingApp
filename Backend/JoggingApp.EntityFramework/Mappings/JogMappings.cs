using JoggingApp.Core;
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

            builder.HasOne(x => x.User);
        }
    }
}
