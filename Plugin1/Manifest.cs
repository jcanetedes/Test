using Test.Core;

namespace Plugin1;

internal class Manifest : IManifest
{
    public string GetAppName()
    {
        return "Adaptive Registration";
    }
}