using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.MazeScrapper.Domain.Entities;

namespace Rtl.TvMaze.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.Property(p => p.Name)
               .HasMaxLength(150)
               .IsRequired();

    }
}

