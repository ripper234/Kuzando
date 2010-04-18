using System.Web.Mvc;
using Kuzando.Common;
using Kuzando.Common.Web;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;

namespace Kuzando.Web.Controllers
{
    public abstract class KuzandoControllerBase : UserAwareControllerBase
    {
        protected KuzandoControllerBase(IUserRepository userRepository)
            : base(userRepository)
        {
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}