using System;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Show(DateTime from, DateTime to)
        {
            var range = new DateRange(from, to);
            var tasks = _taskRepository.GetByDueDateRange(GetCurrentUser().Id, range);
            return SingleUserView(new TasksForDateRange(range, tasks));
        }

        public JsonResult Get(DateTime from, DateTime to)
        {
            var tasks = _taskRepository.GetByDueDateRange(GetCurrentUser().Id, new DateRange(from, to));
            var serializableTasks = from task in tasks select new {task.Title, task.DueDate, task.PriorityInDay, task.Id};
            return Json(serializableTasks);
        }

        [HttpPost]
        public void UpdateDueDate(int taskId, DateTime newDate)
        {
            var user = GetCurrentUser();
            _taskRepository.UpdateDate(user.Id, taskId, newDate);
        }
    }
}