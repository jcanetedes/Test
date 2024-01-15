using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Models;
using Test2.Core;

namespace Test.Core.Interfaces;

public interface IMenuService
{
    List<MenuModel> GetMenus(string pluginName);
    List<IManifest> GetPluginManifest();
    void RegisterManifest(IManifest manifest);
}
