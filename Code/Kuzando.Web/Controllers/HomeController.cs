using System;
using System.Web.Mvc;
using Kuzando.Common;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Web.Controllers
{
    [HandleError]
    public class HomeController : KuzandoControllerBase
    {
        public HomeController(IRepository<User> userRepository) : base(userRepository)
        {
        }

        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return IndexNotLoggedIn();
            }

            return IndexLoggedIn(user);
        }

        /// <summary>
        /// Show hompage for a logged in user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private ActionResult IndexLoggedIn(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show homepage for a user that's not logged in
        /// </summary>
        /// <returns></returns>
        private ActionResult IndexNotLoggedIn()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}