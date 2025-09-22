using Cineflex.Services.Email;
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
                return;
            }

            _isLoading = true; //JON: set isLoading to true so you can't use this multiple times
            StateHasChanged();

            try
            {
                // var user = await DbContext.Users.FirstOrDefaultAsync(e => e.Email == model.Email); JON: find the user in the db with the emial

                // if (user is null) JON:if the e-mail is not found
                // {
                //     Snackbar.Add("E-mailadres niet gevonden!", Severity.Error);
                //     return;
                // }

                backgroundClass = "end-color";
                _fadeOut = true;
                StateHasChanged();

                var code = new Random().Next(100000, 999999).ToString(); //JON: generate a code
                // user.Token = code;
                // user.TokenCreated = DateTime.UtcNow.AddHours(1); JON: make the time olny 1 hour from nowe
                // await DbContext.SaveChangesAsync(); JON: update the User in the db


                var _UserEmail = "jonathanhoefnagel@gmail.com"; //test

                 var emailService = new EmailService();
                 //await emailService.SendForgotPasswordEmailAsync(user.Email, code);
                await emailService.SendForgotPasswordEmailAsync(_UserEmail, code);


                Snackbar.Add("Code verstuurd naar uw e-mail!", Severity.Success);
                _SendCode = true;
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
                // var user = await DbContext.Users.FirstOrDefaultAsync(s => s.Email == model.Email && s.Token == model.ResetCode); JON: find the user int he db with the email and token

                // if (user is null || user.TokenCreated < DateTime.UtcNow) JON: check if the token is valid and the user is not null
                // {
                //     Snackbar.Add("Ongeldige of verlopen code!", Severity.Error);
                //     return;
                // }

                backgroundClass = "start-color"; //JON: set the background to normal for a smooth transition
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