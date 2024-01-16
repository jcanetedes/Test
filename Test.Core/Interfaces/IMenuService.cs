using CAP.Interfaces2;
using CAP.Interfaces2.Models;
namespace Test.Core.Interfaces;

public interface IMenuService
{
    List<MenuModel> GetMenus(string pluginName);
    List<IManifest> GetPluginManifest();

    IManifest GetDefaultManifest();
    void RegisterManifest(IManifest manifest);
}
