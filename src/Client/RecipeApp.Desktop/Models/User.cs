using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Desktop.Models
{
    public class User : IUser
    {
        public string Guid { get; set; }
        public string UserId { get; set; }

        public static User FromInterface(IUser user)
        {
            if (string.IsNullOrEmpty(user.Guid))
            {
                user.Guid = System.Guid.NewGuid().ToString();
            }
            var output = new User()
            {
                Guid = user.Guid,
                UserId = user.UserId,
            };
            return output;
        }
    }
}
