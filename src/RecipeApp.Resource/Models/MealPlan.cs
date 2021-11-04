using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipeApp.Resource.Models
{
    public class MealPlan : IMealPlan
    {
        public string Guid { get; set; }
        public User User { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<ShoppingListItem> ShoppingList { get; set; } 
        
        [JsonIgnore]
        [NotMapped]
        IUser IMealPlan.User
        {
            get { return User; }
            set { User = (User)value;  }
        }

        [JsonIgnore]
        [NotMapped]
        IEnumerable<IRecipe> IMealPlan.Recipes
        {
            get { return Recipes; }
            set { Recipes = (ICollection<Recipe>)value; }
        }

        [JsonIgnore]
        [NotMapped]
        IEnumerable<IShoppingListItem> IMealPlan.ShoppingList
        {
            get { return ShoppingList; }
            set { ShoppingList = (ICollection<ShoppingListItem>) value; }
        }

        public static MealPlan FromInterface(IMealPlan mealPlan)
        {
            if (string.IsNullOrEmpty(mealPlan.Guid))
            {
                mealPlan.Guid = System.Guid.NewGuid().ToString();
            }
            if (mealPlan.ShoppingList == null)
            {
                mealPlan.ShoppingList = new List<IShoppingListItem>();
            }
            var output = new MealPlan()
            {
                Guid = mealPlan.Guid,
                User = User.FromInterface(mealPlan.User),
                Recipes = mealPlan.Recipes.Select(x => Recipe.FromInterface(x)).ToList(),
                ShoppingList = mealPlan.ShoppingList.Select(x => ShoppingListItem.FromInterface(x)).ToList()

            };
            return output;
        }

        public List<ShoppingListItem> CreateShoppingListItems()
        {
            var shoppingListItems = new List<ShoppingListItem>();

            foreach (var recipe in Recipes)
            {
                foreach(var ingredient in recipe.Ingredients)
                {
                    var item = new ShoppingListItem
                    {
                        ItemGuid = System.Guid.NewGuid().ToString(),
                        ItemName = ingredient.Name,
                        ItemUnit = ingredient.Unit,
                        ItemCount = ingredient.Amount,
                        Purchased = false,
                        MealPlanGuid = Guid,
                    };
                    shoppingListItems.Add(item);
                }
            }

            return shoppingListItems;
        }

        public List<MealPlanRecipe> CreateMealPlanRecipes()
        {
            var mealPlanRecipes = new List<MealPlanRecipe>();

            foreach (var recipe in Recipes)
            {
                var item = new MealPlanRecipe
                {
                    Guid = System.Guid.NewGuid().ToString(),
                    MealPlanGuid = Guid,
                    RecipeGuid = recipe.Guid
                };
                mealPlanRecipes.Add(item);
            }

            return mealPlanRecipes;
        }
    }
}
