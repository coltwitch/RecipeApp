using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Models
{
    public interface IUser
    {
        public string Guid { get; set; }
        public string UserId { get; set; }
    }
}
