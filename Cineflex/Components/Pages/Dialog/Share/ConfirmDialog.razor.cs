using Microsoft.AspNetCore.Components;
using MimeKit.Cryptography;
using MudBlazor;

namespace Cineflex.Components.Pages.Dialog.Share
{
    public partial class ConfirmDialog
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        private Task Confirm()
        {
            MudDialog.Close(DialogResult.Ok(true));
            return Task.CompletedTask;
        }
        private Task Cancel()
        {
            MudDialog.Close(DialogResult.Cancel());
            return Task.CompletedTask;
        }
    }
}