using Microsoft.Extensions.Logging;
using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Base.Interfaces.ResourceAccess;
using RecipeApp.Base.Managers.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.Base.Managers
{
    public class MealPlanManager : IMealPlanManager
    {
        private IMealPlanResourceAccess _mealPlanResourceAccess;
        private IRecipeResourceAccess _recipeResourceAccess;
        private ILogger<MealPlanManager> _logger;

        public MealPlanManager(IMealPlanResourceAccess mealPlanResourceAccess, IRecipeResourceAccess recipeResourceAccess, ILogger<MealPlanManager> logger)
        {
            _mealPlanResourceAccess = mealPlanResourceAccess;
            _recipeResourceAccess = recipeResourceAccess;
            _logger = logger;
        }

        public IMealPlan GetMealPlanById(string id)
        {
            _logger.LogInformation($"Getting MealPlan {id}");
            var result = _mealPlanResourceAccess.GetMealPlan(id);
            return result;
        }

        public IEnumerable<IMealPlan> GetMealPlans()
        {
            _logger.LogInformation($"Getting MealPlans");
            var result = _mealPlanResourceAccess.GetMealPlans();
            return result;
        }

        public IMealPlan AddMealPlan(IMealPlan mealPlan)
        {
            var mealPlanResult = new MealPlan(mealPlan);
            _logger.LogInformation($"Adding mealPlan: {mealPlan.Guid}");
            var recipeList = new List<IRecipe>();
            foreach(var recipeGuid in mealPlan.Recipes.Select(x => x.Guid))
            {
                recipeList.Add(_recipeResourceAccess.GetRecipe(recipeGuid));
            }
            mealPlanResult.Recipes = recipeList;

            if (mealPlan.ShoppingList == null)
            {
                mealPlanResult.ShoppingList = GenerateShoppingList(mealPlanResult).ShoppingList;
            }
            var result = _mealPlanResourceAccess.CreateOrUpdateMealPlan(mealPlanResult);
            return result;
        }

        public IMealPlan UpdateMealPlan(IMealPlan mealPlan, string id)
        {
            _logger.LogInformation($"Updating mealPlan {id}");
            var result = _mealPlanResourceAccess.CreateOrUpdateMealPlan(mealPlan, id);
            return result;
        }

        public bool DeleteMealPlan(string id)
        {
            _logger.LogInformation($"Deleting mealPlan {id}");
            var result = _mealPlanResourceAccess.DeleteMealPlan(id);
            return result;
        }

        public IMealPlan GenerateShoppingList(IMealPlan mealPlan)
        {
            var shoppingList = new List<IShoppingListItem>();
            foreach (var recipe in mealPlan.Recipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    var shoppingListItem = new ShoppingListItem
                    {
                        ItemCount = ingredient.Amount,
                        ItemName = ingredient.Name,
                        ItemUnit = ingredient.Unit,
                        ItemGuid = System.Guid.NewGuid().ToString(),
                        Purchased = false
                    };
                    shoppingList.Add(shoppingListItem);
                }
            }

            mealPlan.ShoppingList = shoppingList;
            return mealPlan;
        }
    }
}
