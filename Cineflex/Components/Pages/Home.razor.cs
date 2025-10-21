using Cineflex.Components.Pages.Dialog;
using Cineflex.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
using MudBlazor;
namespace Cineflex.Components.Pages
{
    public partial class Home
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] IDialogService DialogService { get; set; } = default!;
        private async Task SeatSelectorTest(Guid cinemaRoomId)
        {
            var parameters = new DialogParameters<SeatSelectorDialog>();
            parameters.Add(c=>c.CinemaRoomId, cinemaRoomId);
            var options = new DialogOptions() { CloseButton = true, CloseOnEscapeKey = true, BackdropClick = true, MaxWidth=MaxWidth.Large };
            await DialogService.ShowAsync<SeatSelectorDialog>(Localizer["SeatSelector_Page"], parameters, options);
            
        }
    }
}