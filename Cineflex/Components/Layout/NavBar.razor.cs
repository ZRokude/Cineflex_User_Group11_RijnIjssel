using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Layout
{
    public partial class NavBar
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;


        private string GetLink(string title)
        {
            return title.ToLower();
        }
    }
}