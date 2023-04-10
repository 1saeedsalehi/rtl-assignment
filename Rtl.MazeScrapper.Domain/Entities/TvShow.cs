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

    public TvShow(string name, List<Artist> cast)
    {

        if (string.IsNullOrWhiteSpace(name)) throw new DomainLogicException("title could not be empty", new ArgumentNullException(nameof(name)));
        Title = name;

        Cast = cast;
    }
}

