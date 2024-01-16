using CAP.Interfaces2.Models;

namespace CAP.Interfaces2;

public interface IManifest
{
    string ProjectName { get; }
    string DefaultPage { get; }
    string Version { get; }
    string AssemblyName { get; }
    bool IsExpanded { get; set; } 
    List<MenuModel> Menus {get;set;}
}