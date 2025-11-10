using Cineflex.Models.Commands.User;
using Cineflex.Services.ApiServices;
using Cineflex.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Cineflex.Components.Pages.Auth
{
    public partial class Login
    {
        [Inject] ILoginService Loginservice { get; set; } = null!;
        [Inject] IUserService UserService { get; set; } = null!;

        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;



        private MudForm _form = new();
        private string _backgroundClass = "start-color";
        private bool _isLoading = false;
        private bool chairsVisible;

        private bool _isLoggedIn = false;


        private LoginFormModel _loginFormModel = new()
        {
            Email = string.Empty,
            Password = string.Empty
        };

        // JON: Password visibility toggle state
        private bool _isPasswordVisible = false;

        // JON: properties for password field visibility
        private InputType PasswordInputType => _isPasswordVisible ? InputType.Text : InputType.Password;
        private string PasswordInputIcon => _isPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        // JON: Toggles the visibility of the password field
        private void TogglePasswordVisibility() => _isPasswordVisible = !_isPasswordVisible;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _isLoggedIn = true;
                return;
            }                  
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Wait a tiny bit for page load effect
                await Task.Delay(100);
                chairsVisible = true;
                StateHasChanged();
            }
        }

        private async Task HandleLoginAsync()
        {
            _isLoading = true;
            try
            {
                
                LoginCommand command = new LoginCommand
                {
                    Email = _loginFormModel.Email,
                    Password = _loginFormModel.Password
                };

                var result = await Loginservice.Login(command);
                if(result.IsSuccesfull)
                {
                    var response = result.Model;
                    if (!string.IsNullOrEmpty(response!.Token))
                    {
                        await DoLogin(response!.Token);
                    }
                    else
                    {
                        Snackbar.Add("Login mislukt. Controleer uw inloggegevens.", Severity.Error);
                    }
                }
                if (result.StatusCode == 500 && !result.IsSuccesfull)//If the user is not found
                {
                    Snackbar.Add("Gebruiker niet gevonden, Controleer uw inloggegevens.");
                    return;
                }


            }
            catch (Exception ex)
            {
                Snackbar.Add($"Er is een fout opgetreden: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        public async Task DoLogin(string bearer)
        {
            if (bearer is not null)
            {
                await ((PersistingAuthenticationStateProvider)AuthStateProvider).SignOut();
                var result = await ((PersistingAuthenticationStateProvider)AuthStateProvider).SignIn(bearer);

                if (result.IsAuthenticated)
                {
                    chairsVisible = false;
                    await Task.Delay(500);
                    NavigationManager.NavigateTo("/", forceLoad: true);

                    await Task.Delay(250);
                    _isLoggedIn = true;
                    return;
                }

                    Snackbar.Add($"Authenticatie mislukt: {result.ErrorCode}", Severity.Error);
            }
        }

        private async Task NavigateToRegister()
        {
            chairsVisible = false;
            await Task.Delay(500);
            NavigationManager.NavigateTo("/register");
        }


        private async Task NavigateToHome()
        {
            chairsVisible = false;
            await Task.Delay(500);
            NavigationManager.NavigateTo("/");
        }

        private async Task NavigateToForgetPassword()
        {
            chairsVisible = false;
            await Task.Delay(500);
            NavigationManager.NavigateTo("/forgotPassword");
        }


        private async Task LogOutAsync()
        {
            await ((PersistingAuthenticationStateProvider)AuthStateProvider).SignOut();

            Snackbar.Add("U bent uitgelogd");
            _isLoggedIn = false;
            StateHasChanged();
            return;
        }

        // Form model class for binding
        public class LoginFormModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}