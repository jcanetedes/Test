using Test.Core.Models;

namespace Test.Core.Interfaces;
public interface IUserService
{
    public ApplicationUser GetApplicationUser();
    public bool HasPermission(params string[] permissions);
    public bool IsAuthenticated();
}
