using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Kuzando.Model.Entities.DB
{
    [ActiveRecord]
    public class User
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [HasMany]
        public virtual IList<Task> Tasks{ get; set; }

        [Property]
        public DateTime SignupDate { get; set; }

        [Property]
        public string OpenId { get; set; }

        [Property]
        public string Email { get; set; }
    }
}
