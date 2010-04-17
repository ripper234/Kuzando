using System;
using Castle.ActiveRecord;
using Kuzando.Model.Entities.DB;
using NHibernate;
using NHibernate.Criterion;

namespace Kuzando.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public override void Save(User user)
        {
            if (user.SignupDate == default(DateTime))
                throw new Exception("Missing signup date");

            if (user.OpenId == null)
                throw new Exception("Missing Open ID for user " + user);

            base.Save(user);
        }

        public User FindByOpenId(string openId)
        {
            return ActiveRecordMediator<User>.FindOne(Restrictions.Eq("OpenId", openId));
        }   
    }
}