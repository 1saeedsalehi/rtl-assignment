using Rtl.MazeScrapper.Domain.Exceptions;

namespace Rtl.MazeScrapper.Domain.Entities;

public class Artist
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string? Birthday { get; init; }

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

