using System;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;
using NUnit.Framework;

namespace Kuzando.Tests.Persistence
{
    [TestFixture]
    public class TasksRepositoryTests : IntegrationTestBase
    {
        private ITaskRepository _tasksRepository;
        private IUserRepository _userRepository;
        private User _user;

        public override void FixtureSetupCore()
        {
            _tasksRepository = Container.Resolve<ITaskRepository>();
            _userRepository = Container.Resolve<IUserRepository>();
        }

        public override void SetupCore()
        {
            _user = CreateUser();
            _userRepository.Save(_user);
        }

        [Test]
        public void GetByDueDateRange_OneTaskPerDay_SevenTasksAreReturned()
        {
            var x = DateTime.Now.Subtract(new DateTime(2010, 05, 2, 01, 20, 20));

            for (int day = 1; day < 20; ++day)
                _tasksRepository.Save(CreateTask(CreateDate(day)));

            var tasks = _tasksRepository.GetByDueDateRange(_user.Id, DateRange.CreateWeekRange(CreateDate(15)));
            Assert.AreEqual(7, tasks.Length);
        }

        #region Helpers

        private static DateTime CreateDate(int day)
        {
            return new DateTime(2010, 4, day, 18, 0, 0);
        }

        private Task CreateTask(DateTime dueDate)
        {
            return new Task
                       {
                           Text = "Some task",
                           CreationDate = DateTime.Now,
                           DueDate = dueDate,
                           User = _user,
                       };
        }

        private static User CreateUser()
        {
            return new User
                       {
                           Email = "some.user@gmail.com",
                           Name = "someuser",
                           OpenId = "open_id",
                           SignupDate = DateTime.Now,
                       };
        }

        #endregion
    }
}
