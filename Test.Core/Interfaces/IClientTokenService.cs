using Test.Core.Models;

namespace Test.Core.Interfaces;
public interface IClientTokenService
{
    /// <summary>
    /// Get JWT
    /// </summary>
    /// <returns>AccessToken</returns>
    Task<AccessToken> GetTokenAsync();
}
