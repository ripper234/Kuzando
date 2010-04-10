using System;
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

        #region IDataSeeder Members

        public void Run()
        {
            Console.WriteLine("Populating database");

            DBUtils.ClearDatabase(_sessionFactory);
        }

        #endregion
    }
}