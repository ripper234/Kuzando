using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kuzando.Model.Entities.DB;
using StackUnderflow.Persistence.Repositories;

namespace Kuzando.Persistence.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
    }
}
