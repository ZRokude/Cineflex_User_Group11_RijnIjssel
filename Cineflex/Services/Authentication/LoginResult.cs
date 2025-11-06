namespace Cineflex.Services.Authentication
{
    public enum LoginResult
    {
        Success,
        UserNotFound,
        Inactive,
        InvalidPassword,
        LockedOut,
        RoleNotAllowed,
        LoginFailed
    }
}
