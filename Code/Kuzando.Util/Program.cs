using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kuzando.Core.Bootsrap;

namespace Kuzando.Util
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
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


    }
}
