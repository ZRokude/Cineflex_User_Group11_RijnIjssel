using Cineflex.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Threading.Tasks;

namespace Cineflex.Components.Pages.Auth
{
    public partial class Login
    {

        [Inject] IUserService UserService { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;



        private MudForm _form = new();
        private string _backgroundClass = "start-color";
        private int _loginAttempts = 0;
        private bool _isLoading = false;
        private bool chairsVisible;


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
                NavigationManager.NavigateTo("/", true); // Redirect to home if already logged in
                Snackbar.Add("U bent al ingelogd");
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
                // Step 1: Call your UserService to authenticate and get JWT token
                var jwtToken = await UserService.Login(_loginFormModel.Email, _loginFormModel.Password);

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    // Step 2: Use DoLogin with the bearer token
                    await DoLogin(jwtToken);
                }
                else
                {
                    Snackbar.Add("Login mislukt. Controleer je inloggegevens.", Severity.Error);
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
                    NavigationManager.NavigateTo("/", forceLoad: true);
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

        private async Task NavigateToForgetPassword()
        {
            chairsVisible = false;
            await Task.Delay(500);
            NavigationManager.NavigateTo("/forgotPassword");
        }

        // Form model class for binding
        public class LoginFormModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}