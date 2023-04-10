using Rtl.MazeScrapper.Domain.Exceptions;
using Rtl.TvMaze.Domain.Entities;

namespace Rtl.MazeScrapper.Domain.Entities;

public class Artist : BaseEntity<long>
{
    public string Name { get; init; }
    public string? Birthday { get; init; }

    public TvShow TvShow { get; set; }

    public Artist()
    {
    }

    public Artist(long id, string name, string birthday)
    {
        if (id <= 0)
            throw new DomainLogicException("id should be greater than 0", new ArgumentNullException(nameof(id)));
        Id = id;

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainLogicException("name could not be empty", new ArgumentNullException(nameof(name)));
        Name = name;

        Birthday = birthday;
    }
}

