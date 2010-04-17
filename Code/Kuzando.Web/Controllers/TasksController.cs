using System;
using System.Web.Mvc;
using Kuzando.Common.Web;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;

namespace Kuzando.Web.Controllers
{
    public class TasksController : KuzandoControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(IUserRepository userRepository, ITaskRepository taskRepository) : base(userRepository)
        {
            _taskRepository = taskRepository;
        }

        public ActionResult Show()
        {
            var now = DateTime.Now;
            var range = DateRange.CreateWeekRange(now);
            var tasks = _taskRepository.GetByDueDateRange(GetCurrentUser().Id, range);
            return SingleUserView(new TasksForDateRange(range, tasks));
        }
    }
}