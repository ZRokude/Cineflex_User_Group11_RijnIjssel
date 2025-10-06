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

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        [Inject] IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter] public required Guid movieId { get; set; }


        private MovieResponse? Movie { get; set; }


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
        private int _MovieAge = 0;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await MovieService.ReadMovieById(movieId);
                Movie = response.Model;

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
                        .Select(r => r.AirTime.Date)
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
                .Where(r => r.AirTime.Date == selectedDate)
                .OrderBy(r => r.AirTime)
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
                    .Select(r => r.AirTime.Date)
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
            var selectedTime = selectedRoom.AirTime;
            var formattedTime = selectedTime.ToString("yyyy-MM-ddTHH:mm:ss");

            NavigationManager.NavigateTo($"/SelectSeats/{movieId}/{formattedTime}");
        }

    }
}