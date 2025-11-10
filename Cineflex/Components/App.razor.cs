using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cineflex.Components
{
    public partial class App
    {
        [Inject] IFileVersionProvider fileVersionProvider { get; set; } = default!;
        string PathBase = "/";
        private string FileVersionhref(string path)
        {
            return (fileVersionProvider.AddFileVersionToPath(PathBase, path));
        }
    }
}