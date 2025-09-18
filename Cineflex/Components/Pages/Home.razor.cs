using Cineflex.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages
{
    public partial class Home
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;    
    }
}