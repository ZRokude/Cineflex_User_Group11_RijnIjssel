using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MudBlazor;

namespace Cineflex.Components.Pages
{
    public partial class SeatSelector
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}