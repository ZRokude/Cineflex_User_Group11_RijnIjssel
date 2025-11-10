using Cineflex.Models.Responses.Cinema;
using Cineflex.Models.Responses.Movie;
using Cineflex.Services.ApiServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Text.Json;
using static Cineflex.Components.Pages.Dialog.PaymentDialog;


namespace Cineflex.Components.Pages.Dialog
{
    public partial class TicketDialog
    {
        [Parameter]
        public Guid ticketId { get; set; }

        [Parameter]
        public Guid accountId { get; set; }

        [Inject] private ITicketService TicketService { get; set; } = null!;
        [Inject] private IMovieService MovieService { get; set; } = null!;
        [Inject] private ICinemaRoomService CinemaRoomService { get; set; } = null!;
        [Inject] private ICinemaService CinemaService { get; set; } = null!;
        [Inject] private ICinemaRoomMovieService CinemaRoomMovieService { get; set; } = null!; 
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;


        private Dictionary<Guid, MovieResponse> movieCache = new();
        private Dictionary<Guid, CinemaRoomResponse> cinemaRoomCache = new();
        private Dictionary<Guid, CinemaRoomMovieResponse> cinemaRoomMovieCache = new(); // Cache voor koppeltabel


        private TicketResponse? ticket;
        private CinemaResponse? cinema;

        private Guid _cinemaRoomMovieId;

        private string backgroundClass = "start-color";
        public string QRCodeText { get; set; }
        public Guid AccountId { get; private set; }
        public string AccountEmail { get; private set; }

        public string QRByte = "";

        protected override async Task OnInitializedAsync()
        {
            var response = await TicketService.GetTicketById(ticketId);
            if (response?.Model != null)
            {
                ticket = response.Model;  

                if (ticket != null) 
                {
                    // Laad CinemaRoomMovie data voor dit ene ticket
                    await LoadCinemaRoomMovieData(ticket.CinemaRoomMovieId);

                    var cinemaRoomMovie = GetCinemaRoomMovie(ticket.CinemaRoomMovieId);
                    if (cinemaRoomMovie != null)
                    {
                        await LoadMovieData(cinemaRoomMovie.MovieId);
                        await LoadCinemaRoomData(cinemaRoomMovie.CinemaRoomId);
                        cinema = await GetCinemaAsync(cinemaRoomMovie.MovieId);
                        _cinemaRoomMovieId = ticket.CinemaRoomMovieId;
                    }

                    await GetCurrentUserIdAsync();
                    GenerateQRCode();

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
                }
            }
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

        private async Task<CinemaResponse?> GetCinemaAsync(Guid movieId)
        {
            var response = await CinemaService.GetCinemaByMovieId(movieId);
            if (response?.Model == null || response.Model.Count == 0)
                return null;

            // Bijvoorbeeld de eerste cinema
            return response.Model.FirstOrDefault();
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
                    AccountEmail = account.Email;
                    return account?.Id;
                }
            }

            return null;
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

        public void GenerateQRCode()
        {
            if (ticket != null)
            {

                string qrText = $"📅 Ticket: {GetStartTime(_cinemaRoomMovieId)}| 💺 Seatnumber: {ticket.SeatNumber}|UnderName: {AccountEmail}";

                using MemoryStream ms = new();
                QRCodeGenerator qrCodeGenerate = new();
                QRCodeData qrCodeData = qrCodeGenerate.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new(qrCodeData);
                using Bitmap qrBitMap = qrCode.GetGraphic(20);
                qrBitMap.Save(ms, ImageFormat.Png);
                string base64 = Convert.ToBase64String(ms.ToArray());
                QRByte = string.Format("data:image/png;base64,{0}", base64);
            }
        }
    }
}