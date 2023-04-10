using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.TvMaze.Domain.Entities;

namespace Rtl.TvMaze.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.Property(p => p.Name)
               .HasMaxLength(150)
               .IsRequired();

        builder
            .HasOne(x => x.TvShow)
            .WithMany(x => x.Cast)
            .HasForeignKey(x => x.ShowId);

    }
}

