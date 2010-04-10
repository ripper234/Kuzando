using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kuzando.Common.Web
{
    public abstract class UserAwareController<TUser> : Controller where TUser : class
    {
        private TUser _currentUser;
        public IRepository<TUser> Users { get; private set; }

        protected UserAwareController(IRepository<TUser> userRepository)
        {
            Users = userRepository;
        }

        private ModelBase<TUser> CreateEmptyModel()
        {
            return new EmptyModel<TUser>(GetCurrentUser());
        }

        private ModelBase<TUser> CreateSingleModel<T>(T item)
        {
            return new ItemModel<T, TUser>(GetCurrentUser(), item);
        }

        private ModelBase<TUser> CreateMultipleModel<T>(T[] items)
        {
            return new ItemsModel<T, TUser>(GetCurrentUser(), items);
        }

        private ModelBase<TUser> CreateMultipleModel<T>(IEnumerable<T> items)
        {
            return new ItemsModel<T, TUser>(GetCurrentUser(), items);
        }

        /// <summary>
        /// Reads the current user from the database.
        /// </summary>
        /// <returns></returns>
        protected TUser GetCurrentUser()
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
