using RecipeApp.Base.Interfaces.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RecipeApp.API.Models
{
    public class MealPlanRequest : IMealPlan
    {
        public string Guid { get; set; }
        public ICollection<RecipeRequest> Recipes { get; set; }
        public UserRequest User { get; set; }
        public ICollection<ShoppingListItemRequest> ShoppingList { get; set; }

        IEnumerable<IShoppingListItem> IMealPlan.ShoppingList
        {
            get => ShoppingList;
            set => ShoppingList = (ICollection<ShoppingListItemRequest>)value;
        }
        IUser IMealPlan.User
        {
            get => User;
            set => User = (UserRequest)value;
        }
        IEnumerable<IRecipe> IMealPlan.Recipes
        {
            get => Recipes;
            set => Recipes = (ICollection<RecipeRequest>)value;
        }
    }
}
