using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kuzando.Model.Entities.DB;
using StackUnderflow.Persistence.Repositories;
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
