using Kuzando.Model.Entities.DB;
using NHibernate;

namespace Kuzando.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }
    }
}