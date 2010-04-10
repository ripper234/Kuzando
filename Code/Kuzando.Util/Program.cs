using System;
using Castle.Windsor;
using Kuzando.Core.Bootsrap;

namespace Kuzando.Util
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                WindsorContainer container = Bootstrapper.Instance.CreateContainer(typeof (Program).Assembly);

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
    }
}