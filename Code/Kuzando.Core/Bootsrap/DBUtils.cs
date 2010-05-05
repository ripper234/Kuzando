using NHibernate;

namespace Kuzando.Core.Bootsrap
{
    public static class DBUtils
    {
        public static void ClearDatabase(ISessionFactory sessionFactory)
        {
            using (ISession session = sessionFactory.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                AddQuery(session, "truncate table tasks");
                AddQuery(session, "delete from users");
                tx.Commit();
            }
        }

        private static void AddQuery(ISession session, string query)
        {
            session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}