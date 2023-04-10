using Microsoft.EntityFrameworkCore;
using Rtl.TvMaze.Domain.Entities;
using System.Reflection;

namespace Rtl.TvMaze.Persistence;

public class TvMazeDbContext : DbContext
{
    public TvMazeDbContext(DbContextOptions options) : base(options)
    {
    }

    protected TvMazeDbContext()
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }
    public virtual DbSet<TvShow> TvShows { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


