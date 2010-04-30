using System;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Web.Dtos
{
    // ReSharper disable MemberCanBePrivate.Global
    public class TaskDto
    {
        public string Text { get; private set; }
        public DateTime DueDate { get; private set; }
        public int PriorityInDay { get; private set; }
        public int Id { get; private set; }

        public TaskDto(Task task)
        {
            Text = task.Text;
            DueDate = task.DueDate;
            PriorityInDay = task.PriorityInDay;
            Id = task.Id;
        }
    }
    // ReSharper restore MemberCanBePrivate.Global
}