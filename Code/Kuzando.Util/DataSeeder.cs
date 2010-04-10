using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kuzando.Core.Bootsrap;
using NHibernate;

namespace Kuzando.Util
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public DataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Run()
        {
            Console.WriteLine("Populating database");

            DBUtils.ClearDatabase(_sessionFactory);
        }
    }
}
