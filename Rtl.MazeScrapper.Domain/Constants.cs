namespace Rtl.MazeScrapper.Domain;

public static class Constants
{
    public const string TvMazeHttpClient = nameof(TvMazeHttpClient);
    public const string AllShowsCacheKey = "all-show-ids";
    public static string GetShowCacheKey(long id) => $"show-{id}";
}
