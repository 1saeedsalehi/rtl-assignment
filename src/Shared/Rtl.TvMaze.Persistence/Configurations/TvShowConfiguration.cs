using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rtl.TvMaze.Domain.Entities;

namespace Rtl.TvMaze.Persistence.Configurations;

public class TvShowConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.Property(t => t.Name)
               .HasMaxLength(200)
               .IsRequired();

        
    }
}

