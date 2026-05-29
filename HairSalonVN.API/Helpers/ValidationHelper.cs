using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HairSalonVN.API.Helpers;

public static partial class ValidationHelper
{
    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?:{}|<>]).{8,}$")]
    private static partial Regex StrongPasswordRegex();

    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (password.Length < 8) return false;
        if (!StrongPasswordRegex().IsMatch(password)) return false;
        return true;
    }

    public static string? GetPasswordStrengthMessage(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "Mật khẩu không được trống";
        if (password.Length < 8)
            return "Mật khẩu phải có ít nhất 8 ký tự";
        if (!password.Any(char.IsUpper))
            return "Mật khẩu phải chứa ít nhất 1 chữ hoa";
        if (!password.Any(char.IsDigit))
            return "Mật khẩu phải chứa ít nhất 1 chữ số";
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return "Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt";
        return null;
    }

    public static bool IsValidVietnamesePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
        if (digitsOnly.Length < 10 || digitsOnly.Length > 15) return false;
        return Regex.IsMatch(phone, @"^[\d\s\-\+\(\)\.]+$");
    }

    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch { return false; }
    }

    public static bool IsValidName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        if (name.Length < 2 || name.Length > 100) return false;
        return Regex.IsMatch(name, @"^[\p{L}\s'\-.]+$");
    }

    public static string SanitizeInput(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return Regex.Replace(input.Trim(), @"<[^>]*>", "").Trim();
    }

    public static bool IsSafeRedirectUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Relative, out _)
            && !url.StartsWith("//")
            && !url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase);
    }
}
