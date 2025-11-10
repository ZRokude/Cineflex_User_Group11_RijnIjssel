using Cineflex.Models.Responses.Cinema;
using Cineflex.Models.Responses.Movie;
using Cineflex.Services.ApiServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Text.Json;
using static Cineflex.Components.Pages.Dialog.PaymentDialog;

namespace Cineflex.Components.Pages
{
    public partial class NewTicketsPage
    {
        private List<TicketResponse> tickets = new();

        [Inject] private ITicketService TicketService { get; set; } = null!;
        [Inject] private IMovieService MovieService { get; set; } = null!;
        [Inject] private ICinemaRoomService CinemaRoomService { get; set; } = null!;
        [Inject] private ICinemaRoomMovieService CinemaRoomMovieService { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private Dictionary<Guid, MovieResponse> movieCache = new();
        private Dictionary<Guid, CinemaRoomResponse> cinemaRoomCache = new();
        private Dictionary<Guid, CinemaRoomMovieResponse> cinemaRoomMovieCache = new(); // Cache voor koppeltabel
        private Guid AccountId;

        private string backgroundClass = "start-color";
        private bool IslogedIn = false;



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                var json = await JS.InvokeAsync<string>("sessionStorage.getItem", "recentTickets");

                if (!string.IsNullOrEmpty(json))
                {
                    var ids = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(json);
                    var response = await TicketService.GetTicketsByIds(ids);
                    tickets = response.Model?.ToList() ?? new();
                }
                else
                {
                    tickets = new();
                }

                await GetCurrentUserIdAsync();

                // ✅ Stap 2: Laad aanvullende data (nu tickets bekend zijn)
                await LoadAllRelatedData();

                // ✅ Stap 3: Herlaad UI
                StateHasChanged();
            }
        }

        private async Task<Guid?> GetCurrentUserIdAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var accountClaim = user.FindFirst("Account");
                if (accountClaim != null)
                {
                    var account = JsonSerializer.Deserialize<AccountClaim>(accountClaim.Value);
                    AccountId = account.Id;
                    IslogedIn = true;
                    return account?.Id;
                }
            }

            return null;
        }

        private async Task LoadAllRelatedData()
        {
            // Haal alle CinemaRoomMovieIds op
            var cinemaRoomMovieIds = tickets
                .Select(t => t.CinemaRoomMovieId)
                .Distinct();

            // Laad alle CinemaRoomMovie data
            foreach (var cinemaRoomMovieId in cinemaRoomMovieIds)
                await LoadCinemaRoomMovieData(cinemaRoomMovieId);

            // Nu we de CinemaRoomMovie data hebben, laad de movies en rooms
            var movieIds = cinemaRoomMovieCache.Values
                .Select(crm => crm.MovieId)
                .Distinct();

            foreach (var movieId in movieIds)
                await LoadMovieData(movieId);

            var roomIds = cinemaRoomMovieCache.Values
                .Select(crm => crm.CinemaRoomId)
                .Distinct();

            foreach (var roomId in roomIds)
                await LoadCinemaRoomData(roomId);
        }

        private async Task LoadCinemaRoomMovieData(Guid cinemaRoomMovieId)
        {
            if (!cinemaRoomMovieCache.ContainsKey(cinemaRoomMovieId))
            {
                var result = await CinemaRoomMovieService.GetById(cinemaRoomMovieId);
                if (result.IsSuccesfull && result.Model != null)
                    cinemaRoomMovieCache[cinemaRoomMovieId] = result.Model;
            }
        }

        private async Task LoadMovieData(Guid movieId)
        {
            if (!movieCache.ContainsKey(movieId))
            {
                var movieResult = await MovieService.ReadMovieById(movieId);
                if (movieResult.IsSuccesfull && movieResult.Model != null)
                    movieCache[movieId] = movieResult.Model;
            }
        }

        private async Task LoadCinemaRoomData(Guid roomId)
        {
            if (!cinemaRoomCache.ContainsKey(roomId))
            {
                var roomResult = await CinemaRoomService.ReadByRoomId(roomId);
                if (roomResult.IsSuccesfull && roomResult.Model != null)
                    cinemaRoomCache[roomId] = roomResult.Model;
            }
        }

        private CinemaRoomMovieResponse? GetCinemaRoomMovie(Guid cinemaRoomMovieId) =>
            cinemaRoomMovieCache.TryGetValue(cinemaRoomMovieId, out var crm) ? crm : null;

        private MovieResponse? GetMovie(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            return cinemaRoomMovie != null && movieCache.TryGetValue(cinemaRoomMovie.MovieId, out var movie)
                ? movie
                : null;
        }

        private CinemaRoomResponse? GetCinemaRoom(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            return cinemaRoomMovie != null && cinemaRoomCache.TryGetValue(cinemaRoomMovie.CinemaRoomId, out var room)
                ? room
                : null;
        }

        private string GetMovieName(Guid cinemaRoomMovieId) =>
            GetMovie(cinemaRoomMovieId)?.Name ?? "Laden...";

        private string GetRoomNumber(Guid cinemaRoomMovieId) =>
            GetCinemaRoom(cinemaRoomMovieId)?.Name.ToString() ?? "Laden...";

        private string GetStartTime(Guid cinemaRoomMovieId)
        {
            var cinemaRoomMovie = GetCinemaRoomMovie(cinemaRoomMovieId);
            return cinemaRoomMovie?.StartAirTime.ToString("HH:mm") ?? "Laden...";
        }

        private string GetMovieImage(Guid cinemaRoomMovieId)
        {
            var movie = GetMovie(cinemaRoomMovieId);
            return movie == null ? "" : GetImage(movie.Name);
        }

        private string GetImage(string title)
        {
            string[] extensionsImage = [".jpg", ".jpeg", ".png", ".webp"];
            string titleFilter = title.Replace(" ", "_");

            foreach (var ext in extensionsImage)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "CoverMovie", $"{titleFilter}_cover{ext}");
                if (File.Exists(path))
                    return $"/CoverMovie/{titleFilter}_cover{ext}";
            }
            return "";
        }

        void GoToTickets()
        {
            NavigationManager.NavigateTo($"/ticketpage/{AccountId}");
        }
    }
}
