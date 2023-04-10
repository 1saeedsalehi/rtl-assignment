using Rtl.TvMaze.Domain.Entities;

public static class TestData
{
    public static List<TvShow> GetShows()
    {
        return new List<TvShow> {
            new()
            {
                Id = 1,
                Name = "Show Test 1",
                Cast = new List<Artist>
                {
                    new Artist
                    {
                        Id = 1,
                        Name = "Artist 1",
                        Birthday = DateTime.Now.AddYears(-30).ToShortDateString()
                    },
                    new Artist
                    {
                        Id = 2,
                        Name = "Artist 2",
                        Birthday = DateTime.Now.AddYears(-20).ToShortDateString()
                    },
                    new Artist
                    {
                        Id = 3,
                        Name = "Artist 3",
                        Birthday = null
                    },
                }
            }};
    }
}
