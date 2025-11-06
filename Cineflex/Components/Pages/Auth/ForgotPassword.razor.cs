using Cineflex.Models.Responses.User;
using Cineflex.Services.ApiServices;
using Cineflex.Services.Email;
using Cineflex_API.Model.Commands.User;
using Cinelexx.Services.Email;

using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Cineflex.Components.Pages.Auth
{
    public partial class ForgotPassword
    {
        //<--------------------> Model
        private ForgotPasswordModel model = new();
        private MudForm form = new();
        private EmailAddressAttribute emailValidator = new();

        [Inject] IUserService UserService { get; set; } = default!;
        [Inject] ITokenService TokenService { get; set; } = default!;

        private string backgroundClass = "start-color";

        //<-------------------------->bools
        private bool _SendCode = false;
        private bool _fadeOut = false;
        private bool _isLoading = false;
        private bool _isVerifying = false;

        private string lampClass = "lamp-hidden";

        private string emailError = string.Empty;
        private string codeError = string.Empty;



        protected override async Task OnInitializedAsync()
        {
            backgroundClass = "start-color"; //JON: set the background to the startcolor
            await base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(100);
                lampClass = "lamp-visible";
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }


        // Validation Methods
        private void ValidateEmail()
        {
            //JON: set the errormessage to empty
            emailError = string.Empty;

            //JON: check the email
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                emailError = "E-mailadres is verplicht.";
                return;
            }

            //JON: check the email
            if (!IsValidEmailFormat())
            {
                emailError = "Voer een geldig e-mailadres in.";
                return;
            }

        }

        private void ValidateCode()
        {
            // JON: check the code that was send
            codeError = string.Empty;

            if (string.IsNullOrWhiteSpace(model.ResetCode))
            {
                codeError = "Code is verplicht.";
                return;
            }

            // JON: check the code if its 6 digits
            if (!IsValidCodeFormat())
            {
                codeError = "Code moet precies 6 cijfers bevatten.";
                return;
            }
        }

        //JON: validate the email
        private bool IsValidEmailFormat()
        {
            return !string.IsNullOrEmpty(model.Email) && emailValidator.IsValid(model.Email);
        }

        //JON: validate the code for the lengts
        private bool IsValidCodeFormat()
        {
            return !string.IsNullOrEmpty(model.ResetCode) &&
                   model.ResetCode.Length == 6 &&
                   model.ResetCode.All(char.IsDigit);
        }

        //JON: validate the email
        private bool IsEmailValid()
        {
            ValidateEmail();
            return string.IsNullOrEmpty(emailError) && !string.IsNullOrEmpty(model.Email);
        }

        //JON: validate the code again
        private bool IsCodeValid()
        {
            return string.IsNullOrEmpty(codeError) && !string.IsNullOrEmpty(model.ResetCode);
        }


        private async Task SendMail()
        {
            ValidateEmail(); //JON: double check the email

            if (!IsEmailValid())
            {
                Snackbar.Add("Controleer uw e-mailadres en probeer opnieuw.", Severity.Error);
                return; // Return hier zonder _SendCode aan te passen!
            }

            _isLoading = true; //JON: set isLoading to true so you can't use this multiple times
            StateHasChanged();

            try
            {
                backgroundClass = "end-color";
                _fadeOut = true;
                StateHasChanged();

                var userResponse = await UserService.GetAccountByEmail(model.Email);

                if (!userResponse.IsSuccesfull || userResponse.Model == null)
                {
                    Snackbar.Add("E-mailadres niet gevondeEn!", Severity.Error);
                   
                    backgroundClass = "start-color";
                    _fadeOut = false;
                    StateHasChanged();
                    return;
                }

                var user = userResponse.Model;

                var code = new Random().Next(100000, 999999).ToString();
                var tokenResponse = await TokenService.CreateToken(new TokenResponse
                {
                    UserId = user.Id,
                    Value = code,
                    Expiration = DateTime.UtcNow.AddHours(1),
                    IsActive = true
                });

                if (!tokenResponse.IsSuccesfull)
                {
                    Snackbar.Add("Er is een fout opgetreden bij het aanmaken van de token.", Severity.Error);
                    // Reset de UI state
                    backgroundClass = "start-color";
                    _fadeOut = false;
                    StateHasChanged();
                    return;
                }

                var emailService = new EmailService();
                await emailService.SendForgotPasswordEmailAsync(model.Email, code);

                // ALLEEN HIER _SendCode = true zetten bij succes!
                Snackbar.Add("Code verstuurd naar uw e-mail!", Severity.Success);
                _SendCode = true;
            }
            catch (Exception ex)
            {
                // Bij een exception ook de UI resetten
                Snackbar.Add("Er is een fout opgetreden. Probeer het opnieuw.", Severity.Error);
                backgroundClass = "start-color";
                _fadeOut = false;
                // NIET: _SendCode = false;
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task VerifyCode()
        {
            if (!IsCodeValid()) //JON: if the code is not valid
            {
                Snackbar.Add("Controleer uw code en probeer opnieuw.", Severity.Error);
                return;
            }

            _isVerifying = true; //JON: for the loading animation
            StateHasChanged();

            try
            {
                // Gebruik Email en Token parameters die je al hebt
                var tokenResponse = await TokenService.ValidateToken(model.Email, model.ResetCode);

                if (!tokenResponse.IsSuccesfull || tokenResponse.Model == null) //JON: checks if the token is still good
                {
                    Snackbar.Add("Ongeldige herstelcode.", Severity.Error);
                    return;
                }

                backgroundClass = "start-color"; //JON: set the background to normal for a smooth transition
                lampClass = "lamp-hidden";
                _fadeOut = false;
                StateHasChanged();
                await Task.Delay(500); //JON: small delay so the animation can play
                NavigationManager.NavigateTo($"/passwordreset/{model.Email}/{model.ResetCode}");//JON: navigate to the passwordreset page
            }
            finally
            {
                _isVerifying = false;
                StateHasChanged();
            }
        }

        private async Task OnReturnToLoginClick() //JON: if you clicked on return to login
        {
            if (_SendCode)
            {
                backgroundClass = "start-color";
                _fadeOut = false;
                lampClass = "lamp-hidden";
                await Task.Delay(500);
            }
            lampClass = "lamp-hidden";
            await Task.Delay(500);
            StateHasChanged();
            NavigationManager.NavigateTo("/Login");
        }

        private async Task OnReturnToPasswordClick()//JON: if you have not made a password
        {
            backgroundClass = "start-color";
            _fadeOut = false;
            lampClass = "lamp-hidden";
            StateHasChanged();
            await Task.Delay(500);
            NavigationManager.NavigateTo("/firstlogin");
        }

        private class ForgotPasswordModel
        {
            [Required(ErrorMessage = "E-mailadres is verplicht")]
            [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Code is verplicht")]
            [StringLength(6, MinimumLength = 6, ErrorMessage = "Code moet 6 cijfers bevatten")]
            public string ResetCode { get; set; } = string.Empty;
        }
    }
}