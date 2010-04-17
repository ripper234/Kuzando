using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;

namespace Kuzando.Common.Web
{
    public abstract class UserAwareControllerBase : Controller
    {
        private User _currentUser;
        public IUserRepository Users { get; private set; }

        protected UserAwareControllerBase(IUserRepository userRepository)
        {
            Users = userRepository;
        }

        private ModelBase<User> CreateEmptyModel()
        {
            return new EmptyModel<User>(GetCurrentUser());
        }

        private ModelBase<User> CreateSingleModel<T>(T item)
        {
            return new ItemModel<T, User>(GetCurrentUser(), item);
        }

        private ModelBase<User> CreateMultipleModel<T>(T[] items)
        {
            return new ItemsModel<T, User>(GetCurrentUser(), items);
        }

        private ItemsModel<T, User> CreateMultipleModel<T>(IEnumerable<T> items)
        {
            return new ItemsModel<T, User>(GetCurrentUser(), items);
        }

        /// <summary>
        /// Reads the current user from the database.
        /// </summary>
        /// <returns></returns>
        protected User GetCurrentUser()
        {
            if (_currentUser != null)
                return _currentUser;

            var id = User.Identity.Name;
            if (string.IsNullOrEmpty(id))
                return null;

            _currentUser = Users.GetById(int.Parse(id));
            return _currentUser;
        }

        protected ActionResult MultipleUserView<T>(IEnumerable<T> items)
        {
            return MultipleUserView(items.ToList());
        }

        protected ActionResult MultipleUserView<T>(T[] items)
        {
            return MultipleUserView(items.ToList());
        }

        protected ActionResult MultipleUserView<T>(IList<T> items)
        {
            return View(CreateMultipleModel(items));
        }

        protected ActionResult SingleUserView<T>(T item)
        {
            return View(CreateSingleModel(item));
        }

        protected ActionResult EmptyUserView()
        {
            return View(CreateEmptyModel());
        }
    }
}
