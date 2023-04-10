using Rtl.TvMaze.Domain.Exceptions;

namespace Rtl.TvMaze.Domain.Entities;

public class TvShow : BaseEntity<long>
{
    public string Name { get; init; }
    public ICollection<Artist> Cast { get; init; }

    public TvShow()
    {
    }

    public TvShow(string name, List<Artist> cast)
    {

        if (string.IsNullOrWhiteSpace(name)) throw new DomainLogicException("title could not be empty", new ArgumentNullException(nameof(name)));
        Name = name;

        Cast = cast;
    }
}

