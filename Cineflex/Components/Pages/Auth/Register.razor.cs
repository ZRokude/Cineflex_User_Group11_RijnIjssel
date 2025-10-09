using Cineflex_API.Model.Commands.User;

using Cinelexx.Services.Email;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Cineflex.Components.Pages.Auth
{
    public partial class Register
    {
        // Form reference for validation
        private RegisterModel model = new();
        private EmailAddressAttribute emailValidator = new();


        [Inject] IUserService UserService { get; set; } = default!;

        private string _backgroundClass = "start-color";
        private string emailError = string.Empty;


        //<---------------------->Bools
        private bool isValidToken = false;
        private bool isNewPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;
        private bool _isLoading = false;
        private bool _isEmailUniqe = true;
        private DateTime? _UserBD;
        DateTime _timeNow = DateTime.Now;

        //JON Login model containing user input
        public class RegisterModel
        {
            public string Email { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string MiddleName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string PostCode { get; set; } = string.Empty;

            public string Address { get; set; } = string.Empty;

            public DateTime _UserBD { get; set; }

        }

        //JON: Password visibility toggle state
        private bool _isPasswordVisible = false;


        private DateTime DateTody  = DateTime.Now;
        private int _countBetweeDates = 0;

        //JON: properties for password field visibility
        private InputType PasswordInputType => _isPasswordVisible ? InputType.Text : InputType.Password;
        private string PasswordInputIcon => _isPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        // Validation error messages
        private string newPasswordError = string.Empty;
        private string confirmPasswordError = string.Empty;



        //<----------------->Show password
        private InputType NewPasswordInputType => isNewPasswordVisible ? InputType.Text : InputType.Password;
        private InputType ConfirmPasswordInputType => isConfirmPasswordVisible ? InputType.Text : InputType.Password;

        private string NewPasswordVisibilityIcon => isNewPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;

        private string ConfirmPasswordVisibilityIcon => isConfirmPasswordVisible
            ? Icons.Material.Filled.VisibilityOff
            : Icons.Material.Filled.Visibility;


        //JON: Toggles the visibility of the password field
        private void TogglePasswordVisibility() => _isPasswordVisible = !_isPasswordVisible;


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

        //JON: toggle for the password hider
        private void ToggleConfirmPasswordVisibility()
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;
        }

        //JON: toggle for the password hider
        private void ToggleNewPasswordVisibility()
        {
            isNewPasswordVisible = !isNewPasswordVisible;
        }
        private bool IsPasswordValid(string password)
        {
            //JON:  Validation: At least one digit, one lowercase, one uppercase, and 15+ characters
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{15,}$");
            return regex.IsMatch(password);
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

        // Validation Methods
        private async Task ValidateEmail()
        {
            // set the errormessage to empty
            emailError = string.Empty;

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                emailError = "E-mailadres is verplicht.";
                return;
            }

            if (!IsValidEmailFormat())
            {
                emailError = "Voer een geldig e-mailadres in.";
                return;
            }

        }


        //JON: validate the email
        private bool IsEmailValid()
        {
            ValidateEmail();
            return string.IsNullOrEmpty(emailError) && !string.IsNullOrEmpty(model.Email);
        }

        private bool IsValidEmailFormat()
        {
            return !string.IsNullOrEmpty(model.Email) && emailValidator.IsValid(model.Email);
        }


        


        private async Task MakeAccount()
        {
            _isLoading = true;



            DateTime today = DateTime.Today;
            TimeSpan totalDays = (today - _UserBD) ?? TimeSpan.Zero;
            _countBetweeDates = (int)totalDays.TotalDays;

            if (_countBetweeDates < 4380) //JON: 4380 is 12 jaar
            {
                Snackbar.Add("U moet ouder zijn als 12 jaar om een account aan te maken");
                return;
            }

            var hasher = new PasswordHasher<AccountCommand>(); //JON: the hasher of blazor

            await ValidateEmail();

            AccountCommand newAccountCommands = new AccountCommand()
            {
                Email = model.Email,
                Password = model.NewPassword
            };

            try
            {
                // Use the UserService to create the account
                var result = await UserService.CreateAccount(newAccountCommands);

                if (result.Model != null)
                {
                    CredentialCommand credentialCommand = new CredentialCommand
                    {
                        FirstName = model.FirstName,
                        MidName = model.MiddleName,
                        LastName = model.LastName,
                        Address = model.Address,
                        PostCode = model.PostCode,
                        DateBirth = model._UserBD,
                        AccountId = result.Model.Id
                    };

                    var credentialResult = await UserService.CreateCredentialsAccount(credentialCommand);
                    if (credentialResult != null)
                    {
                        var emailService = new EmailService();
                        await emailService.SendWelkomEmailAsync(model.Email);
                        _isLoading = false;
                        Snackbar.Add("Account succesfol toegevoegd");
                        NavigationManager.NavigateTo("/login");
                    }
                    else
                    {
                        Snackbar.Add("Fout opgetreden bij het aanmaken van uw account, probeer opnieuw");
                        return;
                    }

                }
                else
                {
                    if (result.StatusCode == 409)//JON: if the emial is already exised in the db
                    {
                        _isLoading = false;
                        Snackbar.Add("Dit Email adres word al gebruikt");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar?.Add(ex.Message);
                return;
            }

            

        }
    }
}