using Cineflex.Models.Responses.Movie;
using MudBlazor;

namespace Cineflex.Components.Shared
{
    public partial class CustomCarousel
    {
        private MudCarousel<MovieResponse> _carousel = null!;
        private string GetImage(string title)
        {
            string[] extensionsImage = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            string imgurl = "";
            string titleFilter = title.Replace(" ", "_");
            foreach (var ext in extensionsImage)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "CoverMovie", $"{titleFilter}_cover{ext}");
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