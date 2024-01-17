
@echo "building nuget"
nuget pack ModulePackage.nuspec 
XCOPY "*.nupkg" "..\Test2\NugetPackages\" /Y
PAUSE