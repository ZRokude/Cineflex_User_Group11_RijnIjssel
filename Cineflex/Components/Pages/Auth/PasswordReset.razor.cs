
using Cineflex.Services.ApiServices;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.User;
using Cinelexx.Services.Email;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cineflex.Components.Pages.Auth
{
    public partial class PasswordReset
    {
        [Inject] private ITokenService TokenService { get; set; } = null!;
        [Inject] private IUserService UserService { get; set; } = null!;


        //<---------------->Model
        private ResetModel model = new();
        [Parameter] public required string Email { get; set; }
        [Parameter] public required string Token { get; set; }




        private AccountResponse? _user;

        private string backgroundClass = "start-color";

        //<---------------------->Bools
        private bool isValidToken = true;
        private bool isNewPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;



        // Validation error messages
        private string newPasswordError = string.Empty;
        private string confirmPasswordError = string.Empty;

        DateTime _timeNow = DateTime.Now;

        //<----------------->Show password
        private InputType NewPasswordInputType => isNewPasswordVisible ? InputType.Text : InputType.Password;
        private InputType ConfirmPasswordInputType => isConfirmPasswordVisible ? InputType.Text : InputType.Password;

        private string NewPasswordVisibilityIcon => isNewPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        private string ConfirmPasswordVisibilityIcon => isConfirmPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        protected override async Task OnInitializedAsync()
        {
            backgroundClass = "start-color";
            try
            {
                // Gebruik Email en Token parameters die je al hebt
                var tokenResponse = await TokenService.ValidateToken(Email, Token);

                if (!tokenResponse.IsSuccesfull || tokenResponse.Model == null)
                {
                    isValidToken = false;
                    Snackbar.Add("Ongeldige of verlopen resetlink.", Severity.Error);
                    await Task.Delay(1000);
                    NavigationManager.NavigateTo("/forgotPassword");
                    return;
                }

                var validToken = tokenResponse.Model; 

                // Haal gebruiker op
                var userResponse = await UserService.GetAccountByEmail(Email);
                if (!userResponse.IsSuccesfull || userResponse.Model == null)  
                {
                    isValidToken = false;
                    Snackbar.Add("Gebruiker niet gevonden.", Severity.Error);
                    await Task.Delay(1000);
                    NavigationManager.NavigateTo("/forgotPassword");
                    return;
                }

                _user = userResponse.Model;
                isValidToken = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnInitializedAsync: {ex.Message}");
                isValidToken = false;
                Snackbar.Add("Er is een fout opgetreden bij het valideren van de resetlink.", Severity.Error);
            }
        }

        // Validation Methods
        private void ValidateNewPassword()
        {
            //JON: set the errormessage to empty
            newPasswordError = string.Empty;

            //JON: check for the password
            if (string.IsNullOrWhiteSpace(model.NewPassword))
            {
                newPasswordError = "Nieuw wachtwoord mag niet leeg zijn.";
                return;
            }

            //JON: check for the password
            if (!IsPasswordValid(model.NewPassword))
            {
                newPasswordError = "Wachtwoord voldoet niet aan alle vereisten.";
                return;
            }

            // Trigger confirm password validation if it has a value
            if (!string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ValidateConfirmPassword();
            }
        }

        private void ValidateConfirmPassword()
        {
            //JON: check for the password
            confirmPasswordError = string.Empty;

            //JON: check for the password
            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                confirmPasswordError = "Bevestig uw wachtwoord.";
                return;
            }
            //JON: check for the password
            if (model.NewPassword != model.ConfirmPassword)
            {
                confirmPasswordError = "Wachtwoorden komen niet overeen.";
            }
        }

        private bool IsFormValid()
        {
            //JON: check if the form is valid
            return string.IsNullOrEmpty(newPasswordError) &&
                   string.IsNullOrEmpty(confirmPasswordError) &&
                   !string.IsNullOrEmpty(model.NewPassword) &&
                   !string.IsNullOrEmpty(model.ConfirmPassword) &&
                   IsPasswordValid(model.NewPassword) &&
                   model.NewPassword == model.ConfirmPassword;
        }

        //JON: Password Strength Methods
        private bool HasMinLength() => !string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Length >= 15;
        private bool HasUpperCase() => !string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Any(char.IsUpper);
        private bool HasLowerCase() => !string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Any(char.IsLower);
        private bool HasDigit() => !string.IsNullOrEmpty(model.NewPassword) && model.NewPassword.Any(char.IsDigit);

        private int GetPasswordStrengthPercentage()
        {
            //JON: if the password is empty reutrn 0
            if (string.IsNullOrEmpty(model.NewPassword)) return 0;


            int score = 0;
            if (HasMinLength()) score += 25;
            if (HasUpperCase()) score += 25;
            if (HasLowerCase()) score += 25;
            if (HasDigit()) score += 25;

            return score; //JON: return the score of the password
        }

        private Color GetPasswordStrengthColor()
        {
            //JON: make the password strengt collor
            var percentage = GetPasswordStrengthPercentage();
            return percentage switch
            {
                <= 25 => Color.Error,
                <= 50 => Color.Warning,
                <= 75 => Color.Info,
                100 => Color.Success,
                _ => Color.Default //JON: if it can't find the score
            };
        }

        private string GetPasswordStrengthText()
        {
            //JON: make the password strengt text
            var percentage = GetPasswordStrengthPercentage();
            return percentage switch
            {
                <= 25 => "Zwak",
                <= 50 => "Matig",
                <= 75 => "Goed",
                100 => "Sterk",
                _ => "Geen"//JON: if it can't find the score
            };
        }

        private async Task ResetPassword()
        {
            //JON: Final validation before submission
            ValidateNewPassword();
            ValidateConfirmPassword();


            if (!IsFormValid())
            {
                Snackbar.Add("Controleer de invoer en probeer opnieuw.", Severity.Error);
                return;
            }

            if (_user is null)// JON: if it can't find the User
             {
                Snackbar.Add("Onverwachte fout: gebruiker niet gevonden.", Severity.Error);
                return;
            }

            try
            {


                // Reset wachtwoord via API
                var resetResponse = await UserService.ResetPassword(_user.Id, model.NewPassword);

                if (!resetResponse.IsSuccesfull)
                {
                    Snackbar.Add("Er is een fout opgetreden bij het wijzigen van het wachtwoord.", Severity.Error);
                    return;
                }

                Snackbar.Add("Wachtwoord succesvol gewijzigd!", Severity.Success);
                await Task.Delay(2000);

                var emailService = new EmailService();
                await emailService.SendPasswordChangedEmailAsync(_user.Email, _timeNow); //JON: send the mail

                NavigationManager.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetPassword: {ex}");
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }
        }

        private bool IsPasswordValid(string password)
        {
            //JON:  Validation: At least one digit, one lowercase, one uppercase, and 15+ characters
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{15,}$");
            return regex.IsMatch(password);
        }

        //JON: toggle for the password hider
        private void ToggleNewPasswordVisibility()
        {
            isNewPasswordVisible = !isNewPasswordVisible;
        }

        //JON: toggle for the password hider
        private void ToggleConfirmPasswordVisibility()
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;
        }

        //JON: naviagte to the loign page
        private void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/Login");
        }

        private class ResetModel
        {
            public string NewPassword { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}