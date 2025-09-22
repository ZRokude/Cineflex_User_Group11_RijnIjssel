using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Layout
{
    public partial class Navbar
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;
    }
}