using Cineflex.Components.Pages.Dialog;
using Cineflex.Services.ApiService;
using Cineflex_API.Model.Responses.Cinema;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MudBlazor;
using System.Diagnostics.Eventing.Reader;

namespace Cineflex.Components.Pages
{
    public partial class SeatSelector
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ICinemaRoomSeatService CinemaRoomSeatService { get; set; } = default!;
        [Inject] private ITicketService TicketService { get; set; } = default!;
        [Inject] private ICinemaRoomMovieService CinemaRoomMovieService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Parameter] public Guid CinemaRoomId { get; set; } = Guid.Empty;
        private List<CinemaRoomSeatResponse> CinemaRoomSeatResponses { get; set; } = new();
        private List<TicketResponse> TicketResponses { get; set; } = new();
        private CinemaRoomMovieResponse CinemaRoomMovieResponse { get; set; } = new();
        private int currentSeat = 0;
        private List<int> ChoosedSeatList { get; set; } = new();
        private Dictionary<int, bool> IsDisabledButtonList { get; set; } = new();
        private Dictionary<int, MudBlazor.Color> ColorButtonList { get; set; } = new();
        private bool isShowNumber { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            await DoApiService();
            await base.OnInitializedAsync();
        }
        private async Task DoApiService()
        {
            var cinemaRoomSeatResult = await CinemaRoomSeatService.GetByCinemaRoomId(CinemaRoomId);
            if (cinemaRoomSeatResult.IsSuccesfull)
            {
                CinemaRoomSeatResponses = cinemaRoomSeatResult.Model!.ToList();
            }
            var cinemaRoomMovieResult = await CinemaRoomMovieService.GetByCinemaRoomId(CinemaRoomId);
            if (cinemaRoomMovieResult.IsSuccesfull)
            {
                CinemaRoomMovieResponse = cinemaRoomMovieResult.Model!.First();
            }
            var ticketResult = await TicketService.GetTicketByCinemaRoomId(CinemaRoomMovieResponse.Id);
            if (ticketResult.IsSuccesfull)
            {
                if (ticketResult != null && ticketResult.Model?.Count() > 0) TicketResponses = ticketResult.Model.ToList();
            }
        }
        private bool IsSeatTaken(int seatNumber)
        {
            if (TicketResponses.Any(c => c.SeatNumber == seatNumber))
            {
                if(!IsDisabledButtonList.ContainsKey(seatNumber))
                    IsDisabledButtonList.TryAdd(seatNumber, true);
                if (!ColorButtonList.ContainsKey(seatNumber))
                    ColorButtonList.Add(seatNumber, MudBlazor.Color.Transparent);
                return true;
            }
            if (!IsDisabledButtonList.ContainsKey(seatNumber))
                IsDisabledButtonList.TryAdd(seatNumber, false);
            if (!ColorButtonList.ContainsKey(seatNumber))
                ColorButtonList.Add(seatNumber, MudBlazor.Color.Dark);
            return false;
        }
        private Task OnClick(int seatNumber)
        {
            if (!ChoosedSeatList.Contains(seatNumber))
            {
                ChoosedSeatList.Add(seatNumber);
                ColorButtonList[seatNumber] = MudBlazor.Color.Success;
            }
            else
            {
                ChoosedSeatList.Remove(seatNumber);
                ColorButtonList[seatNumber] = MudBlazor.Color.Transparent;
            }
            return Task.CompletedTask;
        }
        private int SeatNumber(int seatRowNumber, int currentSeatIndex)
        {
            var totalPreviousSeat = CinemaRoomSeatResponses.Where(c => c.RowNumber < seatRowNumber).Sum(c => c.TotalRowSeatNumber);
            return totalPreviousSeat + currentSeatIndex ;
        }


        private async Task Reservation()
        {
            var options = new DialogOptions() { CloseButton = true, FullScreen = true, CloseOnEscapeKey = true, BackdropClick = true };

            var ChosenSeatListparameters = new DialogParameters
            {
                { "ChoosedSeatList", ChoosedSeatList },
                { "InformationFilm", CinemaRoomMovieResponse  }

            };
            await DialogService.ShowAsync<ReservationDialog>(Localizer["Payment_Page"], ChosenSeatListparameters, options);
        }
    }
}