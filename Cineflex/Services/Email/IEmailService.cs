using Microsoft.AspNetCore.Authentication;

namespace Cineflex.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAsyncWithFirstLoginCode(string email, string code);

        Task SendPasswordChangedEmailAsync(string email, DateTime _timeNow);

        Task SendWelkomEmailAsync(string email);

        Task SendForgotPasswordEmailAsync(string email, string code);

        //Task SendMovieTicketEmailAsync(string email, Ticket ticket);
    }
}
