using Rtl.MazeScrapper.Domain.Exceptions;
using Rtl.TvMaze.Domain.Entities;

namespace Rtl.MazeScrapper.Domain.Entities;

public class TvShow : BaseEntity<long>
{
    public string Title { get; init; }
    public ICollection<Artist> Cast { get; init; }

    public TvShow()
    {
    }

    public TvShow(long id, string name, List<Artist> cast)
    {
        if (id <= 0) throw new DomainLogicException("id should be greater than 0", new ArgumentNullException(nameof(id)));
        Id = id;

        if (string.IsNullOrWhiteSpace(name)) throw new DomainLogicException("title could not be empty", new ArgumentNullException(nameof(name)));
        Title = name;

        Cast = cast;
    }
}

