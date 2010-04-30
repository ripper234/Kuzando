using System;
using System.Linq;
using System.Web.Mvc;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;
using Kuzando.Web.Dtos;

namespace Kuzando.Web.Controllers
{
    public class TasksController : KuzandoControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(IUserRepository userRepository, ITaskRepository taskRepository) : base(userRepository)
        {
            _taskRepository = taskRepository;
        }

        //[Authorize]
        public ActionResult Show(DateTime from, DateTime to)
        {
            var range = new DateRange(from, to);
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, range);
            return SingleUserView(new TasksForDateRange(range, tasks));
        }

        //[Authorize]
        public ActionResult Get(DateTime from, DateTime to)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, new DateRange(from, to));
            var serializableTasks = from task in tasks select new TaskDto(task);
            return Json(serializableTasks);
        }

        [HttpPost]
        public void UpdateTaskDatePriority(int taskId, DateTime newDate, int newPriorityInDay)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            _taskRepository.UpdateTaskDatePriority(user.Id, taskId, newDate, newPriorityInDay);
        }

        [HttpPost]
        public void UpdateTaskText(int taskId, string newText)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            _taskRepository.UpdateTaskText(user.Id, taskId, newText);
        }

        [HttpPost]
        public void Delete(int taskId)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            _taskRepository.Delete(user.Id, taskId);
        }

        [HttpPost]
        public JsonResult CreateNewTask(int priority, DateTime dueDate)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            var task = new Task
                           {
                               CreationDate = DateTime.Now,
                               DueDate = dueDate,
                               User = user,
                               PriorityInDay = priority,
                           };

            _taskRepository.Save(task);
            return Json(new TaskDto(task));
        }
    }
}