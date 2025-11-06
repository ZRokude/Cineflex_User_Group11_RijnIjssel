namespace Cineflex.Models.Commands.Movie
{
    public class ThemeMovieCommand
    {
        public Guid MovieId { get; set; } = Guid.Empty;

        public Guid ThemeId { get; set; } = Guid.Empty;
    }
}
