namespace Cineflex.Models.Responses.Movie
{
    public class ThemeMovieResponse
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid ThemeId { get; set; } = Guid.Empty;
    }
}
