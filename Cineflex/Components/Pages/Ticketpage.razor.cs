using Cineflex.Components.Pages.Dialog;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Models.Responses.Movie;
using Cineflex.Services.ApiServices;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;


namespace Cineflex.Components.Pages
{
    public partial class Ticketpage
    {

        [Parameter]
        public Guid accountId { get; set; }

        [Inject] private ITicketService TicketService { get; set; } = null!;
        [Inject] private IMovieService MovieService { get; set; } = null!;
        [Inject] private ICinemaRoomService CinemaRoomService { get; set; } = null!;
        [Inject] private ICinemaRoomMovieService CinemaRoomMovieService { get; set; } = null!; 
        [Inject] private IStringLocalizer<Ticketpage> Localizer { get; set; } = null!;
        [Inject] private ICinemaService CinemaService { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        private Dictionary<Guid, MovieResponse> movieCache = new();
        private Dictionary<Guid, CinemaRoomResponse> cinemaRoomCache = new();
        private Dictionary<Guid, CinemaRoomMovieResponse> cinemaRoomMovieCache = new();

        private List<TicketResponse>? tickets;
        private CinemaResponse? cinema;


        private string backgroundClass = "start-color";

        protected override async Task OnInitializedAsync()
        {
            var response = await TicketService.GetTicketByAccountId(accountId);
            if (response?.Model != null)
            {
                tickets = response.Model.ToList();

                // Haal alle CinemaRoomMovieIds op
                var cinemaRoomMovieIds = tickets
                    .Select(t => t.CinemaRoomMovieId)
                    .Distinct();

                // Laad alle CinemaRoomMovie data
                foreach (var cinemaRoomMovieId in cinemaRoomMovieIds)
                {
                    await LoadCinemaRoomMovieData(cinemaRoomMovieId);
                }

                // Nu we de CinemaRoomMovie data hebben, laad de movies en rooms
                var movieIds = cinemaRoomMovieCache.Values
                    .Select(crm => crm.MovieId)
                    .Distinct();

                foreach (var movieId in movieIds)
                {
                    await LoadMovieData(movieId);
                }

                var roomIds = cinemaRoomMovieCache.Values
                    .Select(crm => crm.CinemaRoomId)
                    .Distinct();

                foreach (var roomId in roomIds)
                {
                    await LoadCinemaRoomData(roomId);
                }
            }
        }

        private async Task LoadCinemaRoomMovieData(Guid cinemaRoomMovieId)
        {
            if (!cinemaRoomMovieCache.ContainsKey(cinemaRoomMovieId))
            {
                var result = await CinemaRoomMovieService.GetById(cinemaRoomMovieId);
                if (result.IsSuccesfull && result.Model != null)
                {
                    cinemaRoomMovieCache[cinemaRoomMovieId] = result.Model;
                    cinema = await GetCinemaAsync(cinemaRoomMovieId);
                }
            }
        }

        private async Task<CinemaResponse?> GetCinemaAsync(Guid movieId)
        {
            var response = await CinemaService.GetCinemaByMovieId(movieId);
            if (response?.Model == null || response.Model.Count == 0)
                return null;

            // Bijvoorbeeld de eerste cinema
            return response.Model.FirstOrDefault();
        }




        private async Task LoadMovieData(Guid movieId)
        {
            if (!movieCache.ContainsKey(movieId))
            {
                var movieResult = await MovieService.ReadMovieById(movieId);
                if (movieResult.IsSuccesfull && movieResult.Model != null)
                {
                    movieCache[movieId] = movieResult.Model;
                }
            }
        }

        private async Task LoadCinemaRoomData(Guid roomId)
        {
            if (!cinemaRoomCache.ContainsKey(roomId))
            {
                var roomResult = await CinemaRoomService.ReadByRoomId(roomId);
                if (roomResult.IsSuccesfull && roomResult.Model != null)
                {
                    cinemaRoomCache[roomId] = roomResult.Model;
                }
            }

            
        }

        // Haal CinemaRoomMovie op
        private CinemaRoomMovieResponse? GetCinemaRoomMovie(Guid cinemaRoomMovieId)
        {
            return cinemaRoomMovieCache.TryGetValue(cinemaRoomMovieId, out var crm) ? crm : null;
        }

        // Haal de hele movie op uit de cache via CinemaRoomMovieId
        private MovieResponse? GetMovie(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            if (cinemaRoomMovie == null) return null;

            return movieCache.TryGetValue(cinemaRoomMovie.MovieId, out var movie) ? movie : null;
        }

        // Haal cinema room op via CinemaRoomMovieId
        private CinemaRoomResponse? GetCinemaRoom(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            if (cinemaRoomMovie == null) return null;

            return cinemaRoomCache.TryGetValue(cinemaRoomMovie.CinemaRoomId, out var room) ? room : null;
        }

        // Haal alleen de naam op
        private string GetMovieName(Guid cinemaRoomMovieId)
        {
            return GetMovie(cinemaRoomMovieId)?.Name ?? "Laden...";
        }

        // Haal room nummer op
        private string GetRoomNumber(Guid cinemaRoomMovieId)
        {
            return GetCinemaRoom(cinemaRoomMovieId)?.Name.ToString() ?? "Laden...";
        }

        // Haal start tijd op
        private string GetStartTime(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            return cinemaRoomMovie?.StartAirTime.ToString("HH:mm") ?? "Laden...";
        }

        // Haal de image URL op voor een specifieke movie
        private string GetMovieImage(Guid cinemaRoomMovieId)
        {
            var movie = GetMovie(cinemaRoomMovieId);
            if (movie == null) return "";
            return GetImage(movie.Name);
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


        private async Task ClickOnTicket(Guid ticketId)
        {
            var options = new DialogOptions()
            {
                CloseButton = true,
                CloseOnEscapeKey = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.False, 
                FullWidth = true,          
            };


            var Ticketparameters = new DialogParameters<TicketDialog>
            {
                { x => x.ticketId, ticketId }
            };

            await DialogService.ShowAsync<TicketDialog>("Ticket Details", Ticketparameters, options);
        }
    }
}