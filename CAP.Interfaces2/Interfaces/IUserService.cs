using CAP.Interfaces2.Models;

namespace CAP.Interfaces2;
public interface IUserService
{
    public ApplicationUser GetApplicationUser();
    public bool HasPermission(params string[] permissions);
    public bool IsAuthenticated();
}
