using CAP.Interfaces2.Models;

namespace CAP.Interfaces2;
public interface IClientTokenService
{
    /// <summary>
    /// Get JWT
    /// </summary>
    /// <returns>AccessToken</returns>
    Task<AccessToken> GetTokenAsync();
}
