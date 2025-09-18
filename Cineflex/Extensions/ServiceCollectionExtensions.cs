using Cineflex.Services;
using Cineflex.Services.Authentication;
using Cineflex.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;
using System.Text;
namespace Cineflex.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            services
                .AddMudTranslations()
                .AddTransient<MudLocalizer, Localizer>()
                .AddSingleton<IApplicationResourceManager, ApplicationResourceManager>()
                .AddSingleton<IApplicationResource, ApplicationResource>()
                .AddLocalization(options => options.ResourcesPath = "Resources");
            return services;
        }
        public static IServiceCollection AddAuthenticationAndAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCircuitServicesAccessor()
                .AddCascadingAuthenticationState()
                .AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>()
                .AddScoped<AuthenticationStateService>()
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(
                            o =>
                            {
                                var signingKey = configuration["Authentication:Bearer:SigningKey"];
                                SecurityKey? key = null;
                                if (!string.IsNullOrWhiteSpace(signingKey))
                                {
                                    key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey));
                                }
                                o.TokenValidationParameters.IssuerSigningKey = key;
                                o.TokenValidationParameters.ValidateIssuerSigningKey = key is not null;
                                o.TokenValidationParameters.ValidateLifetime = true;
                                o.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(60);
                                o.TokenValidationParameters.ValidAudience = null;
                                o.TokenValidationParameters.ValidateAudience = false;
                                o.TokenValidationParameters.ValidIssuer = null;
                                o.TokenValidationParameters.ValidateIssuer = false;
                                o.MapInboundClaims = false;
                                o.TokenValidationParameters.ValidateAudience = o.TokenValidationParameters.ValidAudience is not null;
                                o.TokenValidationParameters.ValidateIssuer = o.TokenValidationParameters.ValidIssuer is not null;
                            });

            return services;
        }
        public static IServiceCollection AddApplicationSevice(this IServiceCollection services)
        {
            services.AddMudServices()
                .AddMudTranslations()
                .AddScoped<NotifyService>();
            return services;
        }
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<BearerStateHandler>()
                .AddTransient<HttpRequestHandler<Program>>()
                .AddHttpClient(typeof(Program).AssemblyQualifiedName!, client =>
                {
                    client.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl is not configured."));
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddHttpMessageHandler<BearerStateHandler>();
            return services;
        }

    }
}
