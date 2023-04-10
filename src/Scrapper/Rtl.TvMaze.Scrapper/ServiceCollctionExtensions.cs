using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Polly;
using Polly.RateLimit;
using Rtl.TvMaze.Infrastructure;
using Rtl.TvMaze.Scrapper.HttpClients;
using System.Net;

namespace Rtl.TvMaze.Scrapper;

public static class StartupExtensions
{
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .HandleResult(response => response.StatusCode is
                HttpStatusCode.RequestTimeout or
                HttpStatusCode.ServiceUnavailable or
                HttpStatusCode.InternalServerError or
                HttpStatusCode.TooManyRequests)
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


                    return TimeSpan.FromSeconds(Math.Pow(x: 2, retryAttempt)) // exponential back-off
                           + TimeSpan.FromMilliseconds(Random.Shared.Next(minValue: 0, maxValue: 100)); // jitter
                },
                (_, _, _, _) => Task.CompletedTask);
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


        var retryPolicy = GetRetryPolicy();

        services.AddScoped<ITvMazeHttpClient, TvMazeHttpClient>();

        services.AddHttpClient(Constants.TvMazeHttpClient, options =>
        {
            options.BaseAddress = new Uri(baseUrl);
            options.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        })
            .AddPolicyHandler(retryPolicy);

        return services;

    }


}
