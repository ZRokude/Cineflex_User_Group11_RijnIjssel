using Cineflex.Services.ApiService;
using Cineflex_API.Model.Responses.Cinema;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MudBlazor;

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
        protected override async Task OnInitializedAsync()
        {
            CinemaRoomSeatResponses = await CinemaRoomSeatService.Get(CinemaRoomId);
            CinemaRoomMovieResponse = await CinemaRoomMovieService.GetByCinemaRoomId(CinemaRoomId);
            await TicketService.GetTicketByCinemaRoomId(CinemaRoomId);
            await base.OnInitializedAsync();
        }

    }
}