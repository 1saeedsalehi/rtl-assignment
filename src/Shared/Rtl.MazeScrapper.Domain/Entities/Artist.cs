using Rtl.MazeScrapper.Domain.Exceptions;
using Rtl.TvMaze.Domain.Entities;

namespace Rtl.MazeScrapper.Domain.Entities;

public class Artist : BaseEntity<long>
{
    public long ShowId { get; }
    public string Name { get; init; }
    public string? Birthday { get; init; }

    public TvShow TvShow { get; set; }

    public Artist()
    {
    }

    public Artist(long showId, string name, string birthday)
    {
        
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainLogicException("name could not be empty", new ArgumentNullException(nameof(name)));

        ShowId = showId;
        Name = name;
        Birthday = birthday;
    }
}

