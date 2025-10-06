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
        [Parameter] public Guid CinemaRoomId { get; set; } = Guid.Empty;
        private List<CinemaRoomSeatResponse> CinemaRoomSeatResponses { get; set; } = new();
        private List<TicketResponse> TicketResponses { get; set; } = new();
        private CinemaRoomMovieResponse CinemaRoomMovieResponse { get; set; } = new();
        private int currentSeat = 0;
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
                if(ticketResult != null && ticketResult.Model?.Count() > 0) TicketResponses = ticketResult.Model.ToList();
            }
        }
        private bool IsSeatTaken()
        {
            currentSeat++;
            if(TicketResponses.Any(c=>c.SeatNumber == currentSeat))
            {
                return true;
            }
            return false;
        }

    }
}