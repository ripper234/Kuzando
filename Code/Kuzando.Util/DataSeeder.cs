using System;
using Kuzando.Core.Bootsrap;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;
using NHibernate;

namespace Kuzando.Util
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _tasksRepository;

        public DataSeeder(ISessionFactory sessionFactory, IUserRepository userRepository, ITaskRepository tasksRepository)
        {
            _sessionFactory = sessionFactory;
            _tasksRepository = tasksRepository;
            _userRepository = userRepository;
        }

        #region IDataSeeder Members

        public void Run()
        {
            Console.WriteLine("Populating database");

            DBUtils.ClearDatabase(_sessionFactory);

            AddUsers();
        }

        private void AddUsers()
        {
            var user = new User();
            user.Name = "Aya Federman";
            user.OpenId = "asdf";
            user.SignupDate = DateTime.Now;
            user.Email = "chaoticdawn@gmail.com";
            
            _userRepository.Save(user);
        }

        #endregion
    }
}