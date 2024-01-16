using CAP.Interfaces2;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using Test.Core.Interfaces;
using Test.Core.Services;

namespace Test2;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOqtane(this IServiceCollection services)
    {
        LoadAssemblies();
        services.AddOqtaneServices();

        return services;
    }
    private static void LoadAssemblies()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        if (assemblyPath == null) return;

        AssemblyLoadContext.Default.Resolving += ResolveDependencies;

        var assembliesFolder = new DirectoryInfo(assemblyPath);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // iterate through Oqtane assemblies in /bin ( filter is narrow to optimize loading process )
        foreach (var dll in assembliesFolder.EnumerateFiles($"*.dll", SearchOption.TopDirectoryOnly).Where(f => f.Name.Contains("Plugin", StringComparison.OrdinalIgnoreCase)))
        {
            AssemblyName assemblyName;
            try
            {
                assemblyName = AssemblyName.GetAssemblyName(dll.FullName);
            }
            catch
            {
                Debug.WriteLine($"Oqtane Error: Cannot Get Assembly Name For {dll.Name}");
                continue;
            }

            if (!assemblies.Any(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.GetName())))
            {
                AssemblyLoadContext.Default.LoadOqtaneAssembly(dll, assemblyName);
            }
        }
    }

    private static IServiceCollection AddOqtaneServices(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var hostedServiceType = typeof(IHostedService);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName != null && (a.FullName.Contains("Plugin", StringComparison.OrdinalIgnoreCase)));
        IMenuService menuService = new MenuService();
        foreach (var assembly in assemblies)
        {
            // dynamically register module scoped services (ie. client service classes)
            var implementationTypes = assembly.GetInterfaces<IService>();
            foreach (var implementationType in implementationTypes)
            {
                if (implementationType.AssemblyQualifiedName != null)
                {
                    var serviceType = Type.GetType(implementationType.AssemblyQualifiedName.Replace(implementationType.Name, $"I{implementationType.Name}"));
                    services.AddScoped(serviceType ?? implementationType, implementationType);
                }
            }

            var implementationManifests = assembly.GetInterfaces<IManifest>();

            foreach (var implementationManifest in implementationManifests)
            {
                if (implementationManifest.AssemblyQualifiedName != null)
                {
                    if (implementationManifest is not null)
                    {
                        var manifestInstance = Activator.CreateInstance(implementationManifest);
                        IManifest? iManifest = manifestInstance as IManifest;
                        if (iManifest is not null)
                        {
                            menuService.RegisterManifest(iManifest);
                        }
                    }
                }
            }


            // dynamically register module transient services (ie. server DBContext, repository classes)
            //implementationTypes = assembly.GetInterfaces<ITransientService>();
            //foreach (var implementationType in implementationTypes)
            //{
            //    if (implementationType.AssemblyQualifiedName != null)
            //    {
            //        var serviceType = Type.GetType(implementationType.AssemblyQualifiedName.Replace(implementationType.Name, $"I{implementationType.Name}"));
            //        services.AddTransient(serviceType ?? implementationType, implementationType);
            //    }
            //}

            // dynamically register hosted services
            var serviceTypes = assembly.GetTypes(hostedServiceType);
            foreach (var serviceType in serviceTypes)
            {
                if (!services.Any(item => item.ServiceType == serviceType))
                {
                    services.AddSingleton(hostedServiceType, serviceType);
                }
            }

            // dynamically register server startup services
            //assembly.GetInstances<IServerStartup>()
            //    .ToList()
            //    .ForEach(x => x.ConfigureServices(services));

            //// dynamically register client startup services (these services will only be used when running on Blazor Server)
            //assembly.GetInstances<IClientStartup>()
            //    .ToList()
            //    .ForEach(x => x.ConfigureServices(services));
        }
        services.AddSingleton(menuService);
        return services;
    }

    private static Assembly ResolveDependencies(AssemblyLoadContext context, AssemblyName name)
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + Path.DirectorySeparatorChar + name.Name + ".dll";
        if (System.IO.File.Exists(assemblyPath))
        {
            return context.LoadFromStream(new MemoryStream(System.IO.File.ReadAllBytes(assemblyPath)));
        }
        else
        {
            return null;
        }
    }

    public static void LoadOqtaneAssembly(this AssemblyLoadContext loadContext, FileInfo dll, AssemblyName assemblyName)
    {
        try
        {
            var pdb = Path.ChangeExtension(dll.FullName, ".pdb");
            Assembly assembly = null;

            // load assembly ( and symbols ) from stream to prevent locking files ( as long as dependencies are in /bin they will load as well )
            if (File.Exists(pdb))
            {
                assembly = loadContext.LoadFromStream(new MemoryStream(File.ReadAllBytes(dll.FullName)), new MemoryStream(File.ReadAllBytes(pdb)));
            }
            else
            {
                assembly = loadContext.LoadFromStream(new MemoryStream(File.ReadAllBytes(dll.FullName)));
            }
            Debug.WriteLine($"Oqtane Info: Loaded Assembly {assemblyName}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Oqtane Error: Unable To Load Assembly {assemblyName} - {ex}");
        }
    }

    public static IEnumerable<Type> GetInterfaces<TInterfaceType>(this Assembly assembly)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        return assembly.GetTypes(typeof(TInterfaceType));
    }

    public static IEnumerable<Type> GetTypes(this Assembly assembly, Type interfaceType)
    {
        if (assembly is null)
        {
            throw new ArgumentNullException(nameof(assembly));
        }

        if (interfaceType is null)
        {
            throw new ArgumentNullException(nameof(interfaceType));
        }

        return assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && interfaceType.IsAssignableFrom(x));
    }
}