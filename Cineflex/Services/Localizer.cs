using Cineflex.Resources;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Cineflex.Services
{
    public class Localizer(IApplicationResourceManager appResourceManager) : MudLocalizer
    {
        private readonly IApplicationResourceManager _appResourceManager = appResourceManager;
        public override LocalizedString this[string key]
            => new(key, _appResourceManager.GetFromResource(key) ?? key);

        public override LocalizedString this[string key, params object[] arguments]
        {
            get
            {
                var value = _appResourceManager.GetFromResource(key);
                return new LocalizedString(key, value is null ? key : string.Format(value, arguments));
            }
        }
        public string this[string key, bool raw = true]
            => _appResourceManager.GetFromResource(key) ?? key;
    }
}
