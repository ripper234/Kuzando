using System.Collections.Generic;

namespace Kuzando.Common.Web
{
    public class ItemsModel<T, TUser> : ModelBase<TUser>
    {
        public ItemsModel(TUser loggedInUser, IEnumerable<T> items)
            : base(loggedInUser)
        {
            Items = items;
        }

        public IEnumerable<T> Items { get; set; }
    }
}