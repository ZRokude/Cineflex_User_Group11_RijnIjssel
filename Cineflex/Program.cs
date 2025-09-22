using Cineflex.Components;
using Cineflex.Extensions;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        //builder.Services.AddTransient<MudLocalizer, ApplicationTranslation>();
        builder.Services.AddApplicationSevice();
        builder.Services.AddLocalizationServices();
        builder.Services.AddAuthenticationAndAuthorizationServices(builder.Configuration);
        builder.Services.AddClients(builder.Configuration);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();


        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }

}

