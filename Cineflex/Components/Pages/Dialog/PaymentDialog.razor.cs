using Cineflex.Services.ApiServices;
using Cinelexx.Services.Email;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Text.Json;
using Cineflex.Models;
using MudBlazor;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Models.Responses.Movie;
using Cineflex.Models.Commands.Cinema;

namespace Cineflex.Components.Pages.Dialog
{
    public partial class PaymentDialog
    {

        [Inject] IJSRuntime JSRuntime { get; set; } = null!;

        [Inject] private ICinemaRoomMovieService CinemaRoomMovieService { get; set; } = default!;
        [Inject] private IMovieService MovieService { get; set; } = null!;
        [Inject] private ICinemaRoomService CinemaRoomService { get; set; } = null!;

        [Inject] private ITicketService TicketService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;



        [Parameter] public List<int> ChoosedSeatList { get; set; } = new();
        [Parameter] public CinemaRoomMovieResponse CinemaRoomMovieResponse { get; set; } = null!;
        private CinemaRoomResponse? CinemaRoomResponse { get; set; }

        private PaymentCardModel CardModel { get; set; } = new();
        private List<string> RoomInformation { get; set; } = new();
        private MovieResponse? MovieResponse { get; set; }


        private string _backgroundClass = "start-color";
        private string SelectedPaymentMethod { get; set; } = "IDEAL";
        private string MovieName = "";
        private string AccountEmail = "";
        private string? RoomNumber;
        private string CardNumberError;
        private string ExpiryError;
        private string CVVError;



        private double PricePerSeat = 7.5;
        private double PriceTotal = 0;

        private int AmountSeatChosen = 0;

        private bool _isLoading = false;

        private Guid AccountId;
        private Guid RoomNumberId;
        private Guid MovieId;
        private Guid RoomId;






        protected override async Task OnInitializedAsync()
        {
            if (CinemaRoomMovieResponse == null)
            {
                Console.WriteLine("CinemaRoomMovieResponse is NULL!");
                return;
            }

            AmountSeatChosen = ChoosedSeatList.Count;

            PriceTotal = AmountSeatChosen * PricePerSeat;

            RoomId = CinemaRoomMovieResponse.CinemaRoomId;
            MovieId = CinemaRoomMovieResponse.MovieId;


            RoomInformation.Clear();
            RoomInformation.Add($"{CinemaRoomMovieResponse.StartAirTime:M-dd HH:mm}");

            await GetCurrentUserIdAsync();


            await DoApiService();
            await base.OnInitializedAsync();
        }


        private async Task DoApiService()
        {
            var cinemaRoomMovieResult = await CinemaRoomMovieService.GetByCinemaRoomId(RoomId);
            if (cinemaRoomMovieResult.IsSuccesfull)
            {
                CinemaRoomMovieResponse = cinemaRoomMovieResult.Model!.First();
                CinemaRoomMovieResponse.CinemaRoomId = RoomNumberId;

                var cinemaRoomResult = await CinemaRoomService.ReadByRoomId(RoomId);
                if (cinemaRoomResult.IsSuccesfull)
                {
                    CinemaRoomResponse = cinemaRoomResult.Model!;
                    RoomNumber = CinemaRoomResponse.Name;
                }
            }

            var movieResult = await MovieService.ReadMovieById(MovieId);
            if (movieResult.IsSuccesfull)
            {
                MovieResponse = movieResult.Model!;
                MovieName = MovieResponse.Name;
            }


        }

        private async Task ScrollDates()
        {
            await JSRuntime.InvokeVoidAsync("scrollDates", "CARD");
        }

        private bool ValidateCardNumber()
        {
            CardNumberError = null;
            var digits = new string(CardModel.Number.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(digits) || digits.Length < 12 || digits.Length > 19)
            {
                CardNumberError = "Invalid card number.";
                return false;
            }

            if (!IsLuhnValid(digits))
            {
                CardNumberError = "Card number failed validation.";
                return false;
            }

            return true;
        }

        private bool ValidateExpiry()
        {
            ExpiryError = null;
            if (string.IsNullOrWhiteSpace(CardModel.Expiry))
            {
                ExpiryError = "Expiry date required.";
                return false;
            }

            var parts = CardModel.Expiry.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
            {
                ExpiryError = "Use MM/YY format.";
                return false;
            }

            if (month < 1 || month > 12)
            {
                ExpiryError = "Invalid month.";
                return false;
            }

            if (year < 100) year += 2000; // assume YY format

            if (new DateTime(year, month, DateTime.DaysInMonth(year, month)) < DateTime.UtcNow)
            {
                ExpiryError = "Card is expired.";
                return false;
            }

            return true;
        }

        private bool ValidateCVV()
        {
            CVVError = null;
            if (string.IsNullOrWhiteSpace(CardModel.CVV) || !CardModel.CVV.All(char.IsDigit) || (CardModel.CVV.Length != 3 && CardModel.CVV.Length != 4))
            {
                CVVError = "Invalid CVV.";
                return false;
            }
            return true;
        }

        private bool IsLuhnValid(string digits)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int n = digits[i] - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        private bool ValidateCardForm() =>
            ValidateCardNumber() & ValidateExpiry() & ValidateCVV();
        private void SubmitCard()
        {
            if (ValidateCardForm())
            {
                Console.WriteLine("Card is valid!");
                Console.WriteLine($"Number: {CardModel.Number}, Expiry: {CardModel.Expiry}, CVV: {CardModel.CVV}");
                // Proceed with reservation/payment
            }
            else
            {
                Console.WriteLine("Card validation failed!");
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
                    AccountEmail = account.Email;
                    return account?.Id;
                }
            }

            return null;
        }


        private async Task buyTicker()
        {
            try
            {
                _isLoading = true;

                if (SelectedPaymentMethod == "CARD")
                {
                    SubmitCard();
                }

                var createdIds = new List<Guid>();
                Console.WriteLine("is trying to buy a ticket");

                // Maak alle tickets aan
                foreach (var seatNumber in ChoosedSeatList)
                {
                    var ticketCommand = new TicketCommand
                    {
                        Id = Guid.NewGuid(),
                        AccounId = AccountId,
                        CinemaRoomMovieId = MovieId,
                        SeatNumber = seatNumber

                    };
                    await TicketService.CreateTicket(ticketCommand);
                    createdIds.Add(ticketCommand.Id);
                }

                // Haal alle tickets op
                var tickets = new List<TicketResponse>();
                foreach (var ticketId in createdIds)
                {
                    var response = await TicketService.GetTicketById(ticketId);
                    if (response?.IsSuccesfull == true && response.Model != null)
                    {
                        tickets.Add(response.Model);
                    }
                }

                // Haal CinemaRoomMovie data op
                var cinemaRoomMovieResponse = await CinemaRoomMovieService.GetById(MovieId);
                if (cinemaRoomMovieResponse?.IsSuccesfull == true && cinemaRoomMovieResponse.Model != null)
                {
                    var cinemaRoomMovie = cinemaRoomMovieResponse.Model;

                    // Haal movie data op
                    var movieResponse = await MovieService.ReadMovieById(cinemaRoomMovie.MovieId);

                    // Haal room data op
                    var roomResponse = await CinemaRoomService.ReadByRoomId(cinemaRoomMovie.CinemaRoomId);

                    if (movieResponse?.IsSuccesfull == true && roomResponse?.IsSuccesfull == true)
                    {
                        // Maak een lijst met verrijkte ticket data voor de email
                        var ticketDetails = tickets.Select(t => new TicketEmailData
                        {
                            Name = movieResponse.Model.Name,
                            Datetime = cinemaRoomMovie.StartAirTime.ToString("dd-MM-yyyy HH:mm"),
                            Room = roomResponse.Model.Name.ToString(),
                            Seat = t.SeatNumber.ToString()
                            
                        }).ToList();

                        // Stuur email met alle tickets
                        var emailService = new EmailService();
                        await emailService.SendMovieTicketEmailAsync(AccountEmail, ticketDetails);
                    }
                }

                // Zet ID's in sessionStorage als JSON
                await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", "recentTickets",
                    System.Text.Json.JsonSerializer.Serialize(createdIds));

                _isLoading = false;
                NavigationManager.NavigateTo("/newtickets");
            }
            catch (Exception ex)
            {
                _isLoading = false;
                Console.WriteLine($"{ex.Message}");
            }
        }







        public class PaymentCardModel
        {
            public string Number { get; set; } = string.Empty;
            public string Expiry { get; set; } = string.Empty;
            public string CVV { get; set; } = string.Empty;
        }   
        public class AccountClaim
        {
            public Guid Id { get; set; }
            public string Email { get; set; } = string.Empty;
        }


    }

}

