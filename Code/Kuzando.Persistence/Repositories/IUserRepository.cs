using Kuzando.Common;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByOpenId(string openId);
        void Update(User user);
    }
}