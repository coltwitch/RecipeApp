using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Resource.Models
{
    public class MealPlanRecipe
    {
        public string Guid { get; set; }
        public string MealPlanGuid { get; set; }
        public string RecipeGuid { get; set; }
    }
}
