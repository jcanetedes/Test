using CAP.Interfaces2;
using CAP.Interfaces2.Models;
using Test.Core.Interfaces;

namespace Test.Core.Services;

public class MenuService : IMenuService
{
    private readonly List<IManifest> manifests = new List<IManifest>();

    public IManifest GetDefaultManifest()
    {
        return manifests.FirstOrDefault()!;
    }

    public List<MenuModel> GetMenus(string pluginName)
    {
        List<MenuModel> menus = new List<MenuModel>();

        var manifest = manifests.FirstOrDefault(c => c.ProjectName == pluginName);

        if (manifest is not null)
        {
            menus = manifest.Menus;
        }

        return menus;
    }

    public List<IManifest> GetPluginManifest()
    {
        return manifests;
    }

    public void RegisterManifest(IManifest manifest)
    {
        manifests.Add(manifest);
    }
}
