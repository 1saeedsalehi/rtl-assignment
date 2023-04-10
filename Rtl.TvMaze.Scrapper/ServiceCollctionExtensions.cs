using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.RateLimit;
using Rtl.MazeScrapper.Application.HttpClients;
using Rtl.MazeScrapper.Domain;
using System.Net;

namespace Rtl.MazeScrapper;

public static class StartupExtensions
{
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>(x => x.StatusCode is null ||
                                               x.StatusCode is HttpStatusCode.RequestTimeout or
                                                               HttpStatusCode.ServiceUnavailable or
                                                               HttpStatusCode.InternalServerError or
                                                               HttpStatusCode.TooManyRequests or not
                                                               HttpStatusCode.NotFound)

            .Or<RateLimitRejectedException>()
            .WaitAndRetryForeverAsync(
                (retryAttempt, delegateResult, _) =>
                {
                    //get wait delta from response headers if it's possible
                    var retryDelta = delegateResult.Result?.Headers.RetryAfter?.Delta;

                    if (retryDelta is not null)
                    {
                        return retryDelta.Value + TimeSpan.FromMilliseconds(value: 100);
                    }

                    if (delegateResult.Exception is RateLimitRejectedException rateLimitRejectedException)
                    {
                        return rateLimitRejectedException.RetryAfter + TimeSpan.FromMilliseconds(value: 100);
                    }

                    if (retryAttempt > 5)
                    {
                        throw delegateResult.Exception ?? new Exception(message: "Retry attempt is greater than 5");
                    }

                    return TimeSpan.FromSeconds(Math.Pow(x: 2, retryAttempt)) // exponential back-off
                           + TimeSpan.FromMilliseconds(Random.Shared.Next(minValue: 0, maxValue: 100)); // jitter
                },
                (_, _, _, _) => Task.CompletedTask);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRateLimitPolicy(int numberOfExections, TimeSpan perTimeSpan)
    {
        return Policy.RateLimitAsync<HttpResponseMessage>(numberOfExections, perTimeSpan);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMinutes(3));
    }
    /// <summary>
    /// Adds Http client services for scrapping the TvMaze api
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddTvMazeClient(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<string>("TvMaze:BaseUrl");

        //if (string.IsNullOrEmpty(baseUrl))
        //    throw new ArgumentNullException(nameof(baseUrl));

        var retryPolicy = GetRetryPolicy();
        var rateLimitPolicy = GetRateLimitPolicy(numberOfExections: 100, TimeSpan.FromSeconds(value: 10));
        var timeOutPolicy = GetTimeoutPolicy();
        var resilienceStrategy = Policy.WrapAsync(retryPolicy, rateLimitPolicy, timeOutPolicy);

        services.AddScoped<ITvMazeHttpClient, TvMazeHttpClient>();

        services.AddHttpClient(Constants.TvMazeHttpClient, options =>
        {
            options.BaseAddress = new Uri(baseUrl);
        })
            .SetHandlerLifetime(TimeSpan.FromMinutes(15))
            .AddPolicyHandler(retryPolicy);

        return services;

    }
}
