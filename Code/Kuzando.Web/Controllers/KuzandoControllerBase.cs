using Kuzando.Common;
using Kuzando.Common.Web;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Web.Controllers
{
    public abstract class KuzandoControllerBase : UserAwareController<User>
    {
        protected KuzandoControllerBase(IRepository<User> userRepository) : base(userRepository)
        {
        }
    }
}