using Test.Core.Models;

namespace Test2.Core;

public interface IManifest
{
    string ProjectName { get; }
    string Version { get; }
    string AssemblyName { get; }
    bool IsExpanded { get; set; } 
    List<MenuModel> Menus {get;set;}
}