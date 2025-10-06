using Cineflex.Models.Dto.Movie;
using Cineflex_API.Model.Commands.Movie;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages
{
    public partial class Home
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;

        private MudCarousel<MovieResponse> _carousel = null!;
        private MudCarousel<object> _carouselMovieList = null!;
        private List<MovieResponse> MoviesHighlight { get; set; } = new();
        private List<MovieResponse> MovieList { get; set; } = new();
        private string[] extensionsImage = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        protected override Task OnInitializedAsync()
        {
            
            return base.OnInitializedAsync();
        }
        private async Task DoApiService()
        {
            var movieHighlightResult = await 
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