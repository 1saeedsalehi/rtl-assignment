using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.MazeScrapper.Domain.Entities;

namespace Rtl.TvMaze.Persistence.Configurations;

public class TvShowConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.Property(t => t.Title)
               .HasMaxLength(200)
               .IsRequired();

        
    }
}

