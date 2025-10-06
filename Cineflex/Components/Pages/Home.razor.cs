using Cineflex.Services.ApiService;
using Cineflex_API.Model.Commands.Movie;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages
{
    public partial class Home
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] private IMovieService MovieService { get; set; } = default!;

        private MudCarousel<MovieResponse> _carousel = null!;
        private MudCarousel<object> _carouselMovieList = null!;
        private List<MovieResponse> MoviesHighlight { get; set; } = new();
        private List<MovieResponse> MovieList { get; set; } = new();
        private string[] extensionsImage = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        protected override async Task OnInitializedAsync()
        {
            await DoApiService();
        }
        private async Task DoApiService()
        {
            var movieHighlightResult = await MovieService.GetAll();
            if (movieHighlightResult.IsSuccesfull)
            {
                MoviesHighlight = movieHighlightResult.Model!.ToList();
                MovieList = movieHighlightResult.Model!.ToList();
            }
        }
        private string GetImage(string title)
        {
            string imgurl = "";
            string titleFilter = title.Replace(" ", "_");
            foreach (var ext in extensionsImage)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot","CoverMovie", $"{titleFilter}_cover{ext}");
                if (File.Exists(path))
                {
                    imgurl = $"/CoverMovie/{titleFilter}_cover{ext}";
                    break;
                }
            }
            return imgurl;
        }
    }
}