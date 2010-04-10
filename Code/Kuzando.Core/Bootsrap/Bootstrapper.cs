#region

using System.Reflection;
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Kuzando.Common.IoC;
using Kuzando.Persistence.Repositories;

#endregion

namespace Kuzando.Core.Bootsrap
{
    public class Bootstrapper
    {
        public static readonly Bootstrapper Instance = new Bootstrapper();

        /// <summary>
        ///   Singleton, use Bootstrapper.Instance
        /// </summary>
        private Bootstrapper()
        {
        }

        public WindsorContainer CreateContainer(params Assembly[] extreaAssemblies)
        {
            var container = new WindsorContainer(new XmlInterpreter(new FileResource("castle.xml")));
            container.AutoWireServicesIn(typeof(IUserRepository).Assembly);

            foreach (var assembly in extreaAssemblies)
            {
                container.AutoWireServicesIn(assembly);
            }

            return container;
        }
    }
}