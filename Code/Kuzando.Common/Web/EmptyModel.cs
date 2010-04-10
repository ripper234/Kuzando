using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kuzando.Common.Web
{
    public class EmptyModel<TUser> : ModelBase<TUser>
    {
        public EmptyModel(TUser loggedInUser)
            : base(loggedInUser)
        {
        }
    }
}
