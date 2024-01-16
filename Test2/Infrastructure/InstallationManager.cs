using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace Test2.Infrastructure
{
    public class InstallationManager : IInstallationManager
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IWebHostEnvironment _environment;
        public InstallationManager(IHostApplicationLifetime hostApplicationLifetime, IWebHostEnvironment environment)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _environment = environment;
        }
        public void InstallPackages()
        {
            InstallPackages(_environment.WebRootPath, _environment.ContentRootPath);
        }

        public static bool InstallPackages(string webRootPath, string contentRootPath)
        {
            bool install = false;
            string binPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            string sourceFolder = Path.Combine(contentRootPath, "NugetPackages");
            if (!Directory.Exists(sourceFolder))
            {
                Directory.CreateDirectory(sourceFolder);
            }

            // move packages to secure /Packages folder
            foreach (var folderName in "Modules,Themes,Packages".Split(","))
            {
                string folder = Path.Combine(webRootPath, folderName);
                if (Directory.Exists(folder))
                {
                    foreach (var file in Directory.GetFiles(folder, "*.nupkg*"))
                    {
                        var destinationFile = Path.Combine(sourceFolder, Path.GetFileName(file));
                        if (File.Exists(destinationFile))
                        {
                            File.Delete(destinationFile);
                        }

                        if (destinationFile.ToLower().EndsWith(".nupkg.bak"))
                        {
                            // leave a copy in the current folder as it is distributed with the core framework
                            File.Copy(file, destinationFile);
                        }
                        else
                        {
                            // move to destination
                            File.Move(file, destinationFile);
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(folder);
                }
            }

            // iterate through Nuget packages in source folder
            foreach (string packagename in Directory.GetFiles(sourceFolder, "*.nupkg"))
            {
                // iterate through files
                using (ZipArchive archive = ZipFile.OpenRead(packagename))
                {
                    List<string> assets = new List<string>();
                    bool manifest = false;
                    string name = Path.GetFileNameWithoutExtension(packagename);

                    // deploy to appropriate locations
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string filename = "";

                        // evaluate entry root folder
                        switch (entry.FullName.Split('/')[0])
                        {
                            case "lib": // lib/net*/...
                                filename = ExtractFile(entry, binPath, 2);
                                break;
                            case "wwwroot": // wwwroot/...
                                filename = ExtractFile(entry, webRootPath, 1);
                                break;
                            case "runtimes": // runtimes/name/...
                                filename = ExtractFile(entry, binPath, 0);
                                break;
                            case "ref": // ref/net*/...
                                filename = ExtractFile(entry, Path.Combine(binPath, "ref"), 2);
                                break;
                            case "refs": // refs/net*/...
                                filename = ExtractFile(entry, Path.Combine(binPath, "refs"), 2);
                                break;
                            case "content": // content/...
                                filename = ExtractFile(entry, contentRootPath, 0);
                                break;
                        }

                        if (filename != "")
                        {
                            // ContentRootPath sometimes produces inconsistent path casing - so can't use string.Replace()
                            filename = Regex.Replace(filename, Regex.Escape(contentRootPath), "", RegexOptions.IgnoreCase);
                            assets.Add(filename);
                            if (!manifest && Path.GetExtension(filename) == ".log")
                            {
                                manifest = true;
                            }
                        }
                    }

                    // save dynamic list of assets
                    if (!manifest && assets.Count != 0)
                    {
                        string manifestpath = Path.Combine(sourceFolder, name + ".log");
                        if (File.Exists(manifestpath))
                        {
                            File.Delete(manifestpath);
                        }
                        if (!Directory.Exists(Path.GetDirectoryName(manifestpath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(manifestpath));
                        }
                        File.WriteAllText(manifestpath, JsonSerializer.Serialize(assets, new JsonSerializerOptions { WriteIndented = true }));
                    }
                }

                // remove package
                File.Delete(packagename);
                install = true;
            }

            return install;
        }

        private static string ExtractFile(ZipArchiveEntry entry, string folder, int ignoreLeadingSegments)
        {
            string[] segments = entry.FullName.Split('/'); // ZipArchiveEntries always use unix path separator
            string filename = Path.Combine(folder, string.Join(Path.DirectorySeparatorChar, segments, ignoreLeadingSegments, segments.Length - ignoreLeadingSegments));

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
                entry.ExtractToFile(filename, true);
            }
            catch
            {
                // an error occurred extracting the file
                filename = "";
            }
            return filename;
        }
    }
}
