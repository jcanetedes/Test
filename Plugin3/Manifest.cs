using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core;

namespace Plugin3
{
    public class Manifest : ComponentBase, IManifest
    {
        public string ProjectName => "DynamicLibrary3";
        public string Version => "1.0.0";
        public string AssemblyName => "DynamicLibrary3";
    }

}
