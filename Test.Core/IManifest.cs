namespace Test2.Core;

public interface IManifest
{
    string ProjectName { get; }
    string Version { get; }
    string AssemblyName { get; }
}