using Rtl.TvMaze.Domain.Entities;
using Rtl.TvMaze.Domain.Exceptions;
using System.Text.Json;

namespace Rtl.MazeScrapper.UnitTests.DomainTests;

public class ArtistTests
{
    [Fact]
    public void Constructor_WithNullName_ThrowsException()
    {
        // Arrange
        string name = null;
        long showId = 1;
        string birthday = "1990-01-01";

        // Act and Assert
        Assert.Throws<DomainLogicException>(() => new Artist(showId, name, birthday));
    }

    [Fact]
    public void Constructor_WithWhiteSpaceName_ThrowsException()
    {
        // Arrange
        string name = "   ";
        long showId = 1;
        string birthday = "1990-01-01";

        // Act and Assert
        Assert.Throws<DomainLogicException>(() => new Artist(showId, name, birthday));
    }

    [Fact]
    public void Constructor_WithValidData_CreatesArtistObject()
    {
        // Arrange
        string name = "John Doe";
        long showId = 1;
        string birthday = "1990-01-01";

        // Act
        var artist = new Artist(showId, name, birthday);

        // Assert
        Assert.Equal(showId, artist.ShowId);
        Assert.Equal(name, artist.Name);
        Assert.Equal(birthday, artist.Birthday);
    }

    [Fact]
    public void ShowId_IsNotSerialized()
    {
        // Arrange
        var artist = new Artist();

        // Act
        var json = JsonSerializer.Serialize(artist);

        // Assert
        Assert.DoesNotContain("ShowId", json);
    }

    [Fact]
    public void TvShow_IsSerialized()
    {
        // Arrange
        var artist = new Artist { TvShow = new TvShow() };

        // Act
        var json = JsonSerializer.Serialize(artist);

        // Assert
        Assert.Contains("TvShow", json);
    }
}
