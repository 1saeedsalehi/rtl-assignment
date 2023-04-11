using Rtl.TvMaze.Domain.Entities;
using Rtl.TvMaze.Domain.Exceptions;

namespace Rtl.MazeScrapper.UnitTests.DomainTests;

public class TvShowTests
{
    [Fact]
    public void Constructor_WithNullName_ThrowsException()
    {
        // Arrange
        string name = null;
        var cast = new List<Artist>();

        // Act and Assert
        Assert.Throws<DomainLogicException>(() => new TvShow(name, cast));
    }

    [Fact]
    public void Constructor_WithWhiteSpaceName_ThrowsException()
    {
        // Arrange
        string name = "   ";
        var cast = new List<Artist>();

        // Act and Assert
        Assert.Throws<DomainLogicException>(() => new TvShow(name, cast));
    }

    [Fact]
    public void Constructor_WithValidData_CreatesTvShowObject()
    {
        // Arrange
        string name = "Game of Thrones";
        var cast = new List<Artist> { new Artist(), new Artist(), new Artist() };

        // Act
        var tvShow = new TvShow(name, cast);

        // Assert
        Assert.Equal(name, tvShow.Name);
        Assert.Equal(cast, tvShow.Cast);
    }

    [Fact]
    public void Cast_IsNotNull()
    {
        // Arrange
        var tvShow = new TvShow();

        // Assert
        Assert.NotNull(tvShow.Cast);
    }
}
