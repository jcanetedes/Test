using Test2.Core.Models;

namespace Test2.Core.Interfaces;
public interface IClientTokenService
{
    /// <summary>
    /// Get JWT
    /// </summary>
    /// <returns>AccessToken</returns>
    Task<AccessToken> GetTokenAsync();
}
