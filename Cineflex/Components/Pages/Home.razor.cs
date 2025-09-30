using Cineflex.Models.Dto.Movie;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages
{
    public partial class Home
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;

        private MudCarousel<MovieDto> _carousel = null!;
        private MudCarousel<object> _carouselMovieList = null!;
        private List<MovieDto> MoviesHighlight { get; set; } = new();
        private List<MovieDto> MovieList { get; set; } = new();
        private string[] extensionsImage = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        protected override Task OnInitializedAsync()
        {
            MoviesHighlight.Add(new MovieDto { Name = "Inception", Description = "A mind-bending thriller by Christopher Nolan."});
            MoviesHighlight.Add(new MovieDto { Name = "MovieTest", Description = "A mind-bending thriller by Christopher Nolan." });
            MovieList.Add(new MovieDto { Name = "Inception", Description = "A mind-bending thriller by Christopher Nolan." });
            MovieList.Add(new MovieDto { Name = "MovieTest", Description = "A mind-bending thriller by Christopher Nolan." });
            MovieList.Add(new MovieDto { Name = "Inception", Description = "A mind-bending thriller by Christopher Nolan." });
            MovieList.Add(new MovieDto { Name = "MovieTest", Description = "A mind-bending thriller by Christopher Nolan." }); 
            MovieList.Add(new MovieDto { Name = "MovieTest", Description = "A mind-bending thriller by Christopher Nolan." }); MovieList.Add(new MovieDto { Name = "MovieTest", Description = "A mind-bending thriller by Christopher Nolan." });
            return base.OnInitializedAsync();
        }
        private string GetImage(string title)
        {
            string imgurl = "";
            foreach(var ext in extensionsImage)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot","CoverMovie", $"{title}_cover{ext}");
                if (File.Exists(path))
                {
                    imgurl = $"/CoverMovie/{title}_cover{ext}";
                    break;
                }
            }
            return imgurl;
        }
    }
}