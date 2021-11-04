using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.API.Models
{
    public class UserRequest : IUser
    {
        public string Guid { get; set; }
        public string UserId { get; set; }
    }
}
