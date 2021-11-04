using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Base.Interfaces.ResourceAccess;
using RecipeApp.Resource.Models;
using RecipeApp.Resource.ResourceAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.Resource.ResourceAccess
{
    public class MealPlanResourceAccessSQLite : IMealPlanResourceAccess
    {
        private readonly RecipeAppSqliteContext context;

        public MealPlanResourceAccessSQLite(RecipeAppSqliteContext con)
        {
            context = con;
        }

        public IMealPlan CreateOrUpdateMealPlan(IMealPlan mealPlan, string id = null)
        {
            var result = MealPlan.FromInterface(mealPlan);
            var existingMealPlan = false;
            MealPlan mealPlanToUpdate = null;
            if (!string.IsNullOrEmpty(id))
            {
                mealPlanToUpdate = context.MealPlans.FirstOrDefault(x => x.Guid == id);
            }

            if (mealPlanToUpdate != null)
            {
                existingMealPlan = true;
                result.Guid = mealPlanToUpdate.Guid;
                mealPlanToUpdate = result;
            }
            else
            {
                context.MealPlans.Add(result);
            }

            if (existingMealPlan)
            {
                var mealPlanRecipes = GetExistingMealPlanRecipes(mealPlanToUpdate);
                var shoppingListItems = GetExistingShoppingListItems(mealPlanToUpdate);
                context.MealPlanRecipes.RemoveRange(mealPlanRecipes);
                context.ShoppingListItems.RemoveRange(shoppingListItems);
            }

            context.MealPlanRecipes.AddRange(result.CreateMealPlanRecipes());
            context.ShoppingListItems.AddRange(result.CreateShoppingListItems());

            context.SaveChanges();

            return GetMealPlan(result.Guid);
        }

        private List<ShoppingListItem> GetExistingShoppingListItems(MealPlan mealPlanToUpdate)
        {
            return context.ShoppingListItems.Where(x => x.MealPlanGuid == mealPlanToUpdate.Guid).ToList();
        }

        private List<MealPlanRecipe> GetExistingMealPlanRecipes(MealPlan mealPlanToUpdate)
        {
            return context.MealPlanRecipes.Where(x => x.MealPlanGuid == mealPlanToUpdate.Guid).ToList();
        }

        public bool DeleteMealPlan(string id)
        {
            var mealPlan = context.MealPlans.FirstOrDefault(x => x.Guid == id);
            if (mealPlan != null)
            {
                var mealPlanRecipesToDelete = context.MealPlanRecipes
                    .Where(x => x.MealPlanGuid == mealPlan.Guid);

                context.MealPlanRecipes.RemoveRange(mealPlanRecipesToDelete);

                var shoppingListItemsToDelete = context.ShoppingListItems.Where(x => x.MealPlanGuid == mealPlan.Guid);
                context.ShoppingListItems.RemoveRange(shoppingListItemsToDelete);

                context.MealPlans.Remove(mealPlan);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IMealPlan GetMealPlan(string id)
        {
            var mealPlan = context.MealPlans.FirstOrDefault(x => x.Guid == id);
            if (mealPlan != null)
            {
                PopulateMealPlan(mealPlan);
            }
            return mealPlan;
        }

        public IEnumerable<IMealPlan> GetMealPlans()
        {
            var mealPlans = context.MealPlans.ToList();

            foreach (var mealPlan in mealPlans)
            {
                PopulateMealPlan(mealPlan);
            }
            return mealPlans;
        }

        private void PopulateMealPlan(MealPlan mealPlan)
        {
            var recipeGuids = context.MealPlanRecipes
                .Where(x => x.MealPlanGuid == mealPlan.Guid)
                .Select(x => x.RecipeGuid).ToList();
            var recipes = new List<Recipe>();
            foreach (var guid in recipeGuids)
            {
                recipes.Add(context.GetRecipe(guid));
            }
            mealPlan.Recipes = recipes;
            mealPlan.ShoppingList = context.ShoppingListItems.Where(x => x.MealPlanGuid == mealPlan.Guid).ToList();
        }
    }
}
