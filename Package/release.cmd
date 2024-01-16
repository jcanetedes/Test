"..\Packages\nuget.exe" pack ModulePackage.nuspec 
XCOPY "*.nupkg" "..\Test2\NugetPackages\" /Y
PAUSE