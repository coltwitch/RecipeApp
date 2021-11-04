using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.CLI.Models
{
    public class User : IUser
    {
        public string UserId { get; set; }
        public string Guid { get; set; }
    }
}
