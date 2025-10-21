using Cineflex.Models.Responses.Movie;
using Cineflex.Services.ApiServices;
using Cineflex_API.Model.Responses.Cinema;
using Cineflex_API.Model.Responses.Movie;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;


namespace Cineflex.Components.Pages
{
    public partial class MoviePage
    {
        [Inject] private IMovieService MovieService { get; set; } = null!;

        [Inject] private ICinemaRoomService CinemaRoomService { get; set; } = null!;

        [Inject] private ICinemaService CinemaService {get; set;} = null!;

        [Inject] private IMovieThemeService ThemeService { get; set; } = null!;

        [Inject] private IMovieGenreService GenreService { get; set; } = null!;

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        [Inject] IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter] public required Guid movieId { get; set; }


        private MovieResponse? Movie { get; set; }
        private List<ThemeResponse> Theme { get; set; } = new List<ThemeResponse>();
        private List<GenreResponse> Genre { get; set; } = new List<GenreResponse>();


        private List<CinemaRoomMovieResponse> availableRooms = new();
        private List<CinemaResponse> GetCinema = new();
        private List<DateTime> availableDates = new();


        private Guid? _selectedCinemaId = null;

        private bool showMore = false;
        private bool _hasselected = false;
        private bool _isLoggedIn = false;

        private string? formattedDate;
        private string backgroundClass = "start-color";
        private string _airTimeRoom = "";

        private int selectedDateIndex = 0;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await MovieService.ReadMovieById(movieId);
                Movie = response.Model;

                var themeResponse = await ThemeService.ReadByMovieId(movieId);

                var themeDetails = new List<ThemeResponse>();               
                if (themeResponse?.Model != null)
                {
                    foreach (var mt in themeResponse.Model)
                    {
                        var themeDetailResponse = await ThemeService.ReadbyId(mt.ThemeId);
                        if (themeDetailResponse?.Model != null)
                        {
                            themeDetails.Add(themeDetailResponse.Model);
                        }
                    }

                    Theme = themeDetails;
                }


                var genreResponse = await GenreService.ReadByMovieId(movieId);
                var genreDetails = new List<GenreResponse>();

                if (genreResponse?.Model != null && genreResponse.Model.Any())
                {
                    foreach (var movieGenre in genreResponse.Model)
                    {
                        var genreDetailResponse = await GenreService.ReadbyId(movieGenre.GenreId);
                        if (genreDetailResponse?.Model != null)
                        {
                            genreDetails.Add(genreDetailResponse.Model);
                        }
                    }
                    Genre = genreDetails;
                }



                formattedDate = Movie?.ReleaseDate.ToString("yyyy/MM");

                var cinemaResponse = await CinemaService.GetCinema();
                if (cinemaResponse.IsSuccesfull && cinemaResponse.Model != null)
                {
                    GetCinema = cinemaResponse.Model
                        .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                        .ToList();
                }

                var cinemaRoomResponse = await CinemaRoomService.GetRoomsByMovieId(movieId);
                if (cinemaRoomResponse.IsSuccesfull && cinemaRoomResponse.Model != null)
                {
                    availableRooms = cinemaRoomResponse.Model.Where(crm => crm.IsActive).ToList();

                    
                    availableDates = availableRooms
                        .Select(r => r.StartAirTime.Date)
                        .Distinct()
                        .OrderBy(d => d)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity?.IsAuthenticated == true)
                {
                    _isLoggedIn = true;
                    StateHasChanged();
                }


            }
        }


        private string ShortDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Movie?.Description))
                    return string.Empty;

                // Split into words and take first 15
                var words = Movie.Description.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length <= 15)
                    return Movie.Description;

                return string.Join(" ", words.Take(15));
            }
        }

        private string RemainingDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Movie?.Description))
                    return string.Empty;

                var words = Movie.Description.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (words.Length <= 15)
                    return string.Empty; // nothing left to show

                return string.Join(" ", words.Skip(15));
            }
        }

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

        private string GetBackgroundImage(string title)
        {
            string[] extensionsImage = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            string titleFilter = title.Replace(" ", "_");

            foreach (var ext in extensionsImage)
            {
                var originalPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "CoverMovie", $"{titleFilter}_Background_Cover{ext}");
                if (File.Exists(originalPath))
                {
                    // Uitvoerpad voor bijgesneden afbeelding
                    var croppedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "CoverMovie", $"{titleFilter}_Background_Cropped{ext}");

                    if (!File.Exists(croppedPath))
                    {
                        CropImageCenter(originalPath, croppedPath, 400); // Crop naar 400px hoog
                    }

                    return $"/CoverMovie/{titleFilter}_Background_Cropped{ext}";
                }
            }

            return "";
        }


        private void CropImageCenter(string inputPath, string outputPath, int targetHeight)
        {
            using var image = System.Drawing.Image.FromFile(inputPath);
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // Bepaal de hoogte die behouden blijft
            if (originalHeight <= targetHeight)
            {
                // Te klein, gewoon kopiëren
                File.Copy(inputPath, outputPath, true);
                return;
            }

            int y = (originalHeight - targetHeight) / 2; // Midden
            var cropRect = new System.Drawing.Rectangle(0, y, originalWidth, targetHeight);

            using var bmp = new System.Drawing.Bitmap(cropRect.Width, cropRect.Height);
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.DrawImage(image, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), cropRect, System.Drawing.GraphicsUnit.Pixel);
            }

            bmp.Save(outputPath);
        }




        private async Task ScrollDates(string direction)
        {
            await JSRuntime.InvokeVoidAsync("scrollDates", direction);
        }



        private void ToggleShowMore()
        {
            showMore = !showMore;
        }



        private List<CinemaRoomMovieResponse> GetRoomsForSelectedDate()
        {
            var selectedDate = DateTime.Now.AddDays(selectedDateIndex).Date;
            return availableRooms
                .Where(r => r.StartAirTime.Date == selectedDate)
                .OrderBy(r => r.StartAirTime)
                .ToList();
        }


        private async Task OnDateClick(int index, bool isAvailable)
        {
            if (!isAvailable) return;

            selectedDateIndex = index;
            _hasselected = true;

            // Wait for Blazor to render #bottom
            await InvokeAsync(StateHasChanged);

            // Call JS function to scroll
            await JS.InvokeVoidAsync("bottom");
        }



        private async Task OnCinemaChanged(Guid selectedCinemaId)
        {
            _selectedCinemaId = selectedCinemaId;

            var cinemaRoomResponse = await CinemaRoomService.GetRoomsByMovieId(movieId, _selectedCinemaId);

            if (cinemaRoomResponse.IsSuccesfull && cinemaRoomResponse.Model != null)
            {
                availableRooms = cinemaRoomResponse.Model
                    .Where(crm => crm.IsActive)
                    .ToList();

                availableDates = availableRooms
                    .Select(r => r.StartAirTime.Date)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();
            }

            StateHasChanged();
        }


        private void SelectDate(int index)
        {
            _hasselected = true;
            selectedDateIndex = index;

            // Haal alle tijden van de gekozen dag
            var selectedDay = DateTime.Now.AddDays(index).Date;
            var timesOfDay = availableDates.Where(d => d.Date == selectedDay).OrderBy(d => d).ToList();

            if (timesOfDay.Any())
            {
                // Kies bv. de eerste beschikbare tijd van die dag
                _airTimeRoom = string.Join(", ", timesOfDay.Select(t => t.ToString("HH:mm")));

            }
            else
            {
                _airTimeRoom = "Geen tijden beschikbaar";
            }

            StateHasChanged();
        }

        private async Task SelectTicket(CinemaRoomMovieResponse selectedRoom)
        {
            var selectedRoomId = selectedRoom.Id; 
            NavigationManager.NavigateTo($"/Seats/{selectedRoomId}/");
        }

    }
}