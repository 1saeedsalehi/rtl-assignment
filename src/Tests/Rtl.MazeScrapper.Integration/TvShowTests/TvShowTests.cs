using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Rtl.TvMaze.Application.Queries;
using System.Net;

namespace Rtl.MazeScrapper.Integration.TvShowTests
{
    public sealed class TvShowTests : TestBase
    {
       

        [Fact]
        public async Task GetTvShow_WithoutAnyData_ReturnsEmpty()
        {
            // Arrange
            HttpClient client = apiFactory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(TvShowEndpoint);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be(@"{""result"":[],""totalCount"":0}");

        }

        [Fact]
        public async Task GetTvShowQuery_WithFakeShowData_ShouldHaveCorrectCount()
        {
            // Arrange
            var shows = TestData.GetShows();
            await InsertToDatabase(shows);

            var query = new GetTvShowsQuery();

            // Act
            var result = await SendAsync(query);

            //Assert
            result.Count().Should().Be(shows.Count);
            result.First().Cast.Should().HaveCount(shows.First().Cast.Count);
        }

    }


}
