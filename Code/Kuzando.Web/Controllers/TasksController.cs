using System;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Show(DateTime from, DateTime to)
        {
            var range = new DateRange(from, to);
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, range);
            return SingleUserView(new TasksForDateRange(range, tasks));
        }

        public ActionResult Get(DateTime from, DateTime to)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, new DateRange(from, to));
            var serializableTasks = from task in tasks select new {task.Title, task.DueDate, task.PriorityInDay, task.Id};
            return Json(serializableTasks);
        }

        [HttpPost]
        public void UpdateTask(int taskId, DateTime newDate, int newPriorityInDay)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            _taskRepository.UpdateTask(user.Id, taskId, newDate, newPriorityInDay);
        }
    }
}