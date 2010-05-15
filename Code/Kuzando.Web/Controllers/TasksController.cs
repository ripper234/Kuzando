using System;
using System.Linq;
using System.Web.Mvc;
using Kuzando.Common;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;
using Kuzando.Web.Dtos;

namespace Kuzando.Web.Controllers
{
    // ReSharper disable UnusedMember.Global
    public class TasksController : KuzandoControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(IUserRepository userRepository, ITaskRepository taskRepository) : base(userRepository)
        {
            _taskRepository = taskRepository;
        }

        //[Authorize]
        public ActionResult ShowWeek(int? from)
        {
            var dateRange = GetDateRange(from);

            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, dateRange);
            return SingleUserView(new TasksForDateRange(dateRange, tasks));
        }

        //[Authorize]
        public ActionResult Get(int? fromDate)
        {
            var dateRange = GetDateRange(fromDate);
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Authentication");

            var tasks = _taskRepository.GetByDueDateRange(currentUser.Id, dateRange);
            var serializableTasks = from task in tasks select new TaskDto(task);
            return Json(serializableTasks);
        }

        [HttpPost]
        public void UpdateTaskDatePriority(int taskId, int newDate, int newPriorityInDay)
        {
            var newDateTime = DateTimeExtensions.DaysSince1970ToDateTime(newDate);
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            _taskRepository.UpdateTaskDatePriority(user.Id, taskId, newDateTime, newPriorityInDay);
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
        public JsonResult CreateNewTask(int priority, int dueDate)
        {
            var dueDateTime = DateTimeExtensions.DaysSince1970ToDateTime(dueDate);
            var user = GetCurrentUser();
            if (user == null)
                throw new Exception("Must be signed in to update task");

            var task = new Task
                           {
                               CreationDate = DateTime.Now,
                               DueDate = dueDateTime,
                               User = user,
                               PriorityInDay = priority,
                           };

            _taskRepository.Save(task);
            return Json(new TaskDto(task));
        }

        private static DateRange GetDateRange(int? fromDate)
        {
            var from = fromDate != null ? DateTimeExtensions.DaysSince1970ToDateTime(fromDate.Value) : DateTime.Now;
            from = from.GetLastMidnight();
            return DateRange.CreateWeekRange(from);
        }
    }

    // ReSharper restore UnusedMember.Global
}