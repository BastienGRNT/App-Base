namespace AppBase.Auth.Token;

public class JwtSettings
{
    public static string SecretKey { get; set; }
    public static int ExpiryMinutes { get; set; }
    public static string Issuer { get; set; }
    public static string Audience { get; set; }
}