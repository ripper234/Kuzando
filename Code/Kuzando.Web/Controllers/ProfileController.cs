using System;
using System.Web.Mvc;
using Kuzando.Common.Web;
using Kuzando.Persistence.Repositories;
using Kuzando.Web.Models;

namespace Kuzando.Web.Controllers
{
    public class ProfileController : UserAwareControllerBase
    {
        //
        // GET: /User/

        public ProfileController(IUserRepository userRepository) : base(userRepository)
        {
        }

        public ActionResult Index()
        {
            return SingleUserView(new Profile(GetCurrentUser()));
        }

        public ActionResult Edit(Profile item)
        {
            if (item.Username == null)
                throw new Exception("Username can't be null");

            var user = GetCurrentUser();
            user.Name = item.Username;
            Users.Update(user);

            return RedirectToAction("Show", "Tasks");
        }
    }
}
