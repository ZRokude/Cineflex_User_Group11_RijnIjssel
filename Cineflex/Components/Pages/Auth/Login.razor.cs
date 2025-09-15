using Cineflex.Services.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.Data;
using MudBlazor;


namespace Cineflex.Components.Pages.Auth
{
    public partial class Login
    {
        [Inject] IUserService UserService { get; set; } = null!;
        [Inject] CustomAuthStateProvider CustomAuth { get; set; } = null!;

        private MudForm _form = new();
        private string _backgroundClass = "start-color";
        private int _loginAttempts = 0;
        

        private bool _isLoading = false;

        private LoginFormModel _loginFormModel = new()
        {
            Email = string.Empty,
            Password = string.Empty
        };

        //JON: Password visibility toggle state
        private bool _isPasswordVisible = false;

        //JON: properties for password field visibility
        private InputType PasswordInputType => _isPasswordVisible ? InputType.Text : InputType.Password;
        private string PasswordInputIcon => _isPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        //JON: Toggles the visibility of the password field
        private void TogglePasswordVisibility() => _isPasswordVisible = !_isPasswordVisible;

        protected override async Task OnInitializedAsync()
        {
            var authState = await CustomAuth.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
                NavigationManager.NavigateTo("/login", true);
        }

        private async Task HandleLoginAsync()
        {
            _isLoading = true;

            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = _loginFormModel.Email,
                    Password = _loginFormModel.Password
                };
                await UserService.Login(loginRequest.Email, loginRequest.Password);

                var result = await CustomAuth.Login(loginRequest.Email, loginRequest.Password);

                Snackbar.Add(result.ToMessage() ?? "Er is een onbekende fout opgetreden!",
                    result.ToSeverity());

                if (result == LoginResult.Success)
                {
                    Snackbar.Add("Succesvol ingelogd");
                    NavigationManager.NavigateTo("/", true);
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        // Form model class for binding
        public class LoginFormModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}