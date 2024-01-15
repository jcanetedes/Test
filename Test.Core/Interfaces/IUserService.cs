using Test2.Core.Models;

namespace Test2.Core.Interfaces;
public interface IUserService
{
    public ApplicationUser GetApplicationUser();
    public bool HasPermission(params string[] permissions);
    public bool IsAuthenticated();
}
