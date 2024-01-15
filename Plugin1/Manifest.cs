using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Models;
using Test2.Core;
using Test2.Core.Interfaces;

namespace Plugin1;

public class Manifest : IManifest
{
    public string ProjectName => "Plugin1";
    public string Version => "1.0.0";
    public string AssemblyName => "Plugin 1";
    public bool IsExpanded { get; set; } = false;
    public List<MenuModel> Menus { get; set; } = new List<MenuModel>()
    {
        new MenuModel(){ MenuName = "Plugin 1", ComponentName = "Index"},
        new MenuModel(){ MenuName = "Sub Menu", ComponentName = "SubMenu"}
    };
}