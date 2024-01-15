namespace Test2.Core.Models;
public class AccessToken
{
    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTimeOffset Expires { get; set; } = DateTimeOffset.UtcNow;
    

    public bool IsExpired => Expires <= DateTimeOffset.UtcNow;
}
