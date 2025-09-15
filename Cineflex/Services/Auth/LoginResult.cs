namespace Cineflex.Services.Auth
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
