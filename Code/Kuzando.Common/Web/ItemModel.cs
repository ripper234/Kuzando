namespace Kuzando.Common.Web
{
    public class ItemModel<T, TUser> : ModelBase<TUser>
    {
        public ItemModel(TUser loggedInUser, T item)
            : base(loggedInUser)
        {
            Item = item;
        }

        public T Item { get; set; }
    }
}
