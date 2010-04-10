using Kuzando.Model.Entities.DB;
using NHibernate;

namespace Kuzando.Persistence.Repositories
{
    public class TaskRepository : RepositoryBase<Task>, ITaskRepository
    {
        public TaskRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }
    }
}