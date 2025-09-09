using MudBlazor.Services;
using MudBlazor.Translations;
namespace Cineflex.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSevice(this IServiceCollection services)
        {
            services.AddMudServices()
                .AddMudTranslations();
            return services;
        }

    }
}
