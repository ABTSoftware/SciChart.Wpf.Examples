using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MutipleAppDomainsExample
{
    internal class Program
    {
        internal static void Main()
        {
            InitAsync();
        }

        private static async void InitAsync()
        {
            Console.Write("Creating AppDomain #1...");
            var domainWrapper1 = CreateAppDomain(1);
            Console.WriteLine("Done");

            Console.Write("Creating AppDomain #2...");
            var domainWrapper2 = CreateAppDomain(2);
            Console.WriteLine("Done");
            Console.WriteLine();

            Console.Write("AppDomain #1 - Creating window...");
            domainWrapper1.Instance.CreateWindow();
            await WaitAsync(1000, () => domainWrapper1.Instance.IsLoaded);
            Console.WriteLine("Done");

            Console.Write("AppDomain #2 - Creating window...");
            domainWrapper2.Instance.CreateWindow();
            await WaitAsync(1000, () => domainWrapper2.Instance.IsLoaded);
            Console.WriteLine("Done");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Console.WriteLine();

            Console.Write("AppDomain #1 - Closing window...");
            domainWrapper1.Instance.CloseWindow();
            Console.WriteLine("Done");

            Console.Write("AppDomain #2 - Closing window...");
            domainWrapper2.Instance.CloseWindow();
            Console.WriteLine("Done");
            Console.WriteLine();

            Console.Write("Unloading AppDomains...");
            AppDomain.Unload(domainWrapper1.AppDomain);
            AppDomain.Unload(domainWrapper2.AppDomain);
            Console.WriteLine("Done");
        }

        private static AppDomainWrapper<ChartWindowLoader> CreateAppDomain(int appDomainId)
        {
            var type = typeof(ChartWindowLoader);
            var setup = new AppDomainSetup
            {
                ShadowCopyFiles = "False",
                ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            };

            var domain = AppDomain.CreateDomain($"AppDomain #{appDomainId}", null, setup);
            var instance = domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

            return new AppDomainWrapper<ChartWindowLoader>(domain, instance as ChartWindowLoader);
        }

        private static async Task WaitAsync(int delay, Func<bool> predicate)
        {
            if (predicate != null)
            {
                while (!predicate.Invoke())
                {
                    await Task.Delay(delay);
                }
            }
        }
    }

    internal class AppDomainWrapper<T> where T : MarshalByRefObject, new()
    {
        internal AppDomain AppDomain { get; }

        internal T Instance { get; }

        internal AppDomainWrapper(AppDomain appDomain, T instance)
        {
            AppDomain = appDomain;

            Instance = instance;
        }
    }
}