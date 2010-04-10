#region

using System;
using Castle.Core;

#endregion

namespace Kuzando.Common.IoC
{
    /// <summary>
    ///   Specifying hints for container registration.
    ///   When used in conjunction with the AutoWireServicesIn extension method - allow overriding the defaults
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public class ImplementedByAttribute : Attribute
    {
        // todo - http://stackoverflow.com/questions/2612841/what-happened-to-castle-core-lifestyletype-in-1-3-0
        public ImplementedByAttribute(Type serviceType)
        {
            ServiceType = serviceType;

            // default
            Lifestyle = LifestyleType.Singleton;
        }

        /// <summary>
        ///   The component's lifestyle. The default is Singleton
        /// </summary>
        public LifestyleType Lifestyle { get; set; }

        /// <summary>
        ///   The type of the service (usually an interface) this component is implementing
        /// </summary>
        public Type ServiceType { get; set; }
    }
}