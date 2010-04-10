using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Kuzando.Core.Bootsrap;

namespace Kuzando.Util
{
    class Program
    {
        private static void Main()
        {

            try
            {
                var dependentAssemblies = GetDependentAssemblies("Castle.MicroKernel", @"C:\Work\Kuzando\Code\Kuzando.Util\bin\Debug");
                foreach (var assembly in dependentAssemblies)
                {
                    Console.WriteLine(assembly.FullName);
                }
                Console.WriteLine();
                Console.WriteLine();

                var container = Bootstrapper.Instance.CreateContainer(typeof(Program).Assembly);

                // run it
                container.Resolve<IDataSeeder>().Run();
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                    e = e.InnerException;
                }
            }
        }

        public static IEnumerable<Assembly> GetDependentAssemblies(string assemblyName, string assembliesPath)
        {

            var assembliesPaths = Directory.GetFiles(assembliesPath, "*.dll");

            return assembliesPaths
                .Select(Assembly.ReflectionOnlyLoadFrom)
                .Where(folderAssembly =>
                {
                    var fullNames = folderAssembly.GetReferencedAssemblies()
                                               .Select(name => name.FullName);
                    return fullNames.Where(x => x.ToLower().Contains(assemblyName.ToLower())).Count() > 0;
                });
        }
    }


}
