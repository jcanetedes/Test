namespace Test.Core;

public interface IManifest
{
    string ProjectName { get; }
    string Version { get; }
    string AssemblyName { get; }
}