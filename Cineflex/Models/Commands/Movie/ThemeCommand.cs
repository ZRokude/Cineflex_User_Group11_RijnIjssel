namespace Cineflex.Models.Commands.Movie
{
    public class ThemeCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
