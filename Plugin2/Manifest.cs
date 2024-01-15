using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test2.Core;

namespace Plugin2;


public class Manifest : ComponentBase, IManifest
{
    public string ProjectName => "DynamicLibrary1";
    public string Version => "1.0.0";
    public string AssemblyName => "DynamicLibrary1";
}
