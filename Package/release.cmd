"..\Packages\nuget.exe" pack ModulePackage.nuspec 
@echo "building nuget"
XCOPY "*.nupkg" "..\Test2\NugetPackages\" /Y
PAUSE