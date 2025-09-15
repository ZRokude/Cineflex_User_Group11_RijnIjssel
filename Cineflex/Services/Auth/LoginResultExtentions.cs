using MudBlazor;

namespace Cineflex.Services.Auth
{
    public static class LoginResultExtentions
    {
        public static string ToMessage(this LoginResult result) => result switch
        {
            LoginResult.Success => "U bent succesvol ingelogd.",
            LoginResult.UserNotFound => "Ongeldige login, controleer uw inloggegevens!",
            LoginResult.Inactive => "Uw account is niet actief, activeer eerst uw account!",
            LoginResult.InvalidPassword => "Ongeldige login, controleer uw inloggegevens!",
            LoginResult.LockedOut => "Uw account is tijdelijk geblokkeerd. Probeer het later opnieuw!",
            LoginResult.LoginFailed => "Er is een fout opgetreden tijdens het inloggen. Probeer het later opnieuw!",
            LoginResult.RoleNotAllowed => "U heeft geen werknemers account, log in met een werknemers account!",
            _ => "Er is een onbekende fout opgetreden!"
        };

        public static Severity ToSeverity(this LoginResult result)
        {
            return result == LoginResult.Success ? Severity.Success : Severity.Warning;
        }
    }
}
