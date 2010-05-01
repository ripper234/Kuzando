using Kuzando.Model.Entities.DB;

namespace Kuzando.Web.Models
{
    /// <summary>
    /// Represents the editable fields in the user profile
    /// </summary>
    public class Profile
    {
        public Profile()
        {}

        public Profile(User user)
        {
            Username = user.Name;
        }

        public string Username { get; set; }
    }
}
