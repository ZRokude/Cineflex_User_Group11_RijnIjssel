using Cineflex.Resources;

namespace Cineflex.Services
{
    public interface IApplicationResourceManager
    {
        string? GetFromResource(string key);
    }

    public interface IApplicationResource
    {
        string? Get(string key);
    }

    public class ApplicationResourceManager(IApplicationResource applicationResource)
        : IApplicationResourceManager
    {
        public string? GetFromResource(string key)
        {
            string? splicedValue = null;
            if (key.Contains('-'))
            {
                splicedValue = key.Split('-')[^1];
                key = key.Split('-')[0];
            }

            var resource = applicationResource.Get(key);

            if (resource is null)
            {
                return null;
            }

            return splicedValue is null ? resource : string.Format(resource, splicedValue);
        }
    }

    public class ApplicationResource
        : IApplicationResource
    {
        public string? Get(string key)
        {
            return ApplicationTranslation.ResourceManager.GetString(key, ApplicationTranslation.Culture);
        }
    }
}
