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
                const int tasksPerUseres = 10;
                for (int i = 0; i < tasksPerUseres; ++i)
                {
                    var task = new Task
                                   {
                                       Title = "Task number " + (i + 1) + " for user " + user.Id,
                                       Body =
                                           "This is an important task. One of many. In fact, I'm sure there will be many many more.",
                                       DueDate = _randomizer.RandomDate(),
                                       CreationDate = _randomizer.RandomDate(),
                                       Importance = _randomizer.RandomEnum<Importance>(),
                                       User = user,
                                       PriorityInDay = _randomizer.RandomInt(4),
                                   };
                    _tasksRepository.Save(task);
                }
            }
        }

        

        private void AddUsers()
        {
            Console.WriteLine("Adding users");

            AddUser("chaoticdawn", "todo", "chaoticdawn@gmail.com"); 
            AddUser("ripper234", "http://ripper234.com/", "ron.gross@gmail.com");
        }

        private void AddUser(string name, string openId, string email)
        {
            var user = new User
                           {
                               Name = name,
                               OpenId = openId,
                               SignupDate = DateTime.Now,
                               Email = email
                           };

            _users.Add(user);
            _userRepository.Save(user);
        }

        #endregion
    }
}