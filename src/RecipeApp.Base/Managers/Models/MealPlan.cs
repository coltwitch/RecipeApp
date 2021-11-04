using RecipeApp.Base.Interfaces.Models;
using System.Collections.Generic;

namespace RecipeApp.Base.Managers.Models
{
    public class MealPlan : IMealPlan
    {
        public string Guid { get; set; }
        public IEnumerable<IRecipe> Recipes { get; set; }
        public IEnumerable<IShoppingListItem> ShoppingList { get; set; }
        public IUser User { get; set; }
        public MealPlan(IMealPlan mealPlan)
        {
            Guid = mealPlan.Guid;
            Recipes = mealPlan.Recipes;
            ShoppingList = mealPlan.ShoppingList;
            User = mealPlan.User;
        }
    }
}
