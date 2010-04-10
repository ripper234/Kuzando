using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Kuzando.Core.Bootsrap
{
    public static class DBUtils
    {
        public static void ClearDatabase(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                AddQuery(session, "truncate table tasks");
                AddQuery(session, "truncate table users");
                tx.Commit();
            }
        }

        private static void AddQuery(ISession session, string query)
        {
            session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}
