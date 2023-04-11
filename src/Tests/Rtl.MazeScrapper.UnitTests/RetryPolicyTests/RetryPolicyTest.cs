using Microsoft.Extensions.DependencyInjection;
using Polly.RateLimit;
using Rtl.TvMaze.Scrapper;
using System.Net;

namespace Rtl.MazeScrapper.UnitTests.RetryPolicyTests;

public class RetryPolicyTest
{


    [Fact]
    public async Task GetRetryPolicy_RetryOnTooManyRequestsStatusCode()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);

        var retryPolicy = StartupExtensions.GetRetryPolicy();

        // Act
        var result = await retryPolicy.ExecuteAsync(() => Task.FromResult(response));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response, result);
    }

    [Fact]
    public async Task GetRetryPolicy_RetryOnRateLimitRejectedException()
    {
        // Arrange
        var exception = new RateLimitRejectedException(new TimeSpan(0, 0, 5));

        var retryPolicy = StartupExtensions.GetRetryPolicy();

        // Act
        var result = await retryPolicy.ExecuteAsync(() => throw exception);

        // Assert
        Assert.Null(result);
    }

}
