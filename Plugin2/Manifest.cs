using CAP.Interfaces2;
using CAP.Interfaces2.Models;

namespace Plugin2;


public class Manifest : IManifest
{
    public string ProjectName => "Plugin2";
    public string Version => "1.0.0";
    public string AssemblyName => "Plugin 2";

    public List<MenuModel> Menus { get; set; } = new List<MenuModel>()
    {
        new MenuModel(){ MenuName = "Sub Menu 1", ComponentName = "SubMenu"},
        new MenuModel(){ MenuName = "Sub Menu 2", ComponentName = "SubMenu2"}
    };
    public bool IsExpanded { get; set; } = false;

    public string DefaultPage => "Index";
}
