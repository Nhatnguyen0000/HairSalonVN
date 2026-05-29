namespace HairSalonVN.Web.Helpers;

using HairSalonVN.Database.Constants;

public static class SessionAuth
{
    public static bool IsLoggedIn(HttpContext ctx)
        => !string.IsNullOrEmpty(ctx.Session.GetString("AccessToken"));

    public static bool HasRole(HttpContext ctx, string role)
        => ctx.Session.GetString("UserRole") == role;

    public static bool IsAdmin(HttpContext ctx) => HasRole(ctx, Roles.Admin);
    public static bool IsStaff(HttpContext ctx) => HasRole(ctx, Roles.Staff);
}
