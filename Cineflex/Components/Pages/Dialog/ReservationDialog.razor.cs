using Cineflex.Components.Pages.Dialog.Share;
using Cineflex.Models;
using Cineflex.Models.Commands.Cinema;
using Cineflex.Services.ApiServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages.Dialog
{
    public partial class ReservationDialog
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] private TicketService TicketService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public List<TicketCommand> TicketCommands { get; set; }
        [Parameter] public List<int> SeatNumbers { get; set; }
        [Parameter] public AccountClaim AccountClaim { get; set; }
        [Parameter] public string MovieName { get; set; }
        [Parameter] public int RoomNumber { get; set; }
        [Parameter] public DateTime StartTime { get; set; }
        [Parameter] public DateTime EndTime { get; set; }

        public async Task DoReserve()
        {
            foreach(var command in TicketCommands)
            {
                await TicketService.CreateTicket(command);  
            }
        }
        private async Task CheckBeforeReserve()
        {
            var options = new DialogOptions() { CloseButton = true, CloseOnEscapeKey = true, BackdropClick = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["ConfirmDialog_Title"], options);
            var result = dialog.Result;
            if(!result.IsCanceled)
            {
                await DoReserve();
            }
        }

    }
}