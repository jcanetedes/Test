using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interfaces;
using Test.Core.Models;
using Test2.Core;

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

        var manifest = manifests.FirstOrDefault(c=>c.ProjectName == pluginName);

        if(manifest is not null)
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
