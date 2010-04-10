using System;
using System.Collections.Generic;
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
        private readonly List<User> _users = new List<User>();
        private readonly Randomizer _randomizer = new Randomizer(new Random(0));

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
            AddTasks();
        }

        private void AddTasks()
        {
            Console.WriteLine("Adding tasks");

            foreach (var user in _users)
            {
                for (int i = 0; i < 5; ++i)
                {
                    var task = new Task
                                   {
                                       Title = "Task number " + (i + 1),
                                       Body =
                                           "This is an important task. One of many. In fact, I'm sure there will be many many more.",
                                       DueDate = _randomizer.RandomDate(),
                                       CreationDate = _randomizer.RandomDate(),
                                       Importance = _randomizer.RandomEnum<Importance>(),
                                       User = user
                                   };
                    _tasksRepository.Save(task);
                }
            }
        }

        

        private void AddUsers()
        {
            Console.WriteLine("Adding users");

            var user = new User
                           {
                               Name = "Aya Federman",
                               OpenId = "asdf",
                               SignupDate = DateTime.Now,
                               Email = "chaoticdawn@gmail.com"
                           };

            _users.Add(user);
            _userRepository.Save(user);
        }

        #endregion
    }
}