using System;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using Kuzando.Common.Extensions;
using Kuzando.Common.Web;
using Kuzando.Model.Entities.DB;
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

            return RedirectToAction("ShowWeek", "Tasks");
        }

        [HttpPost]
        public void UpdateSetting(bool? hideDone)
        {
            var settings = GetCurrentUser().SettingsFlags;
            if (hideDone != null)
            {
                if ((bool)hideDone)
                    settings = settings.Add(UserSettings.HideDone);
                else
                    settings = settings.Remove(UserSettings.HideDone);
            }
            GetCurrentUser().SettingsFlags = settings;
            Users.Update(GetCurrentUser());
        }
    }
}
