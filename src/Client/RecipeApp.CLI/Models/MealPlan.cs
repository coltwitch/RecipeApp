using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.CLI.Models
{
    public class MealPlan : IMealPlan
    {
        public string Guid { get; set; }
        public IEnumerable<IRecipe> Recipes { get; set; }
        public IUser User { get; set; }
        public IEnumerable<IShoppingListItem> ShoppingList { get; set; }
    }
}
