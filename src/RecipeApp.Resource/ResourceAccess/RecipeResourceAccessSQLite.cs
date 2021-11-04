using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Base.Interfaces.ResourceAccess;
using RecipeApp.Resource.Models;
using RecipeApp.Resource.ResourceAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Resource.ResourceAccess
{
    public class RecipeResourceAccessSQLite : IRecipeResourceAccess
    {
        private readonly RecipeAppSqliteContext context;

        public RecipeResourceAccessSQLite(RecipeAppSqliteContext con)
        {
            context = con;
        }

        public IEnumerable<IRecipe> GetRecipes(string name = null)
        {
            List<Recipe> recipes = null;
            if (string.IsNullOrEmpty(name))
            {
                recipes = context.Recipes.ToList();
            }
            else
            {
                recipes = context.Recipes.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            }
            foreach(var recipe in recipes)
            {
                recipe.Ingredients = recipe.GetIngredients(context);
                recipe.Instructions = recipe.GetInstructions(context);
            }
            return recipes;
        }

        public IRecipe GetRecipe(string id)
        {
            var recipe = context.Recipes.FirstOrDefault(x => x.Guid == id);
            if (recipe != null)
            {
                recipe.Ingredients = recipe.GetIngredients(context);
                recipe.Instructions = recipe.GetInstructions(context);
            }
            return recipe;
        }

        public bool DeleteRecipe(string id)
        {
            try
            {
                var recipeToRemove = context.Recipes.FirstOrDefault(x => x.Guid == id);
                
                if (recipeToRemove != null)
                {
                    var ingredientsToRemove = recipeToRemove.GetIngredients(context);
                    var instructionsToRemove = recipeToRemove.GetInstructions(context);
                    context.Recipes.Remove(recipeToRemove);
                    context.Ingredients.RemoveRange(ingredientsToRemove);
                    context.Instructions.RemoveRange(instructionsToRemove);
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                //do nothing
            }
            
            return false;
        }

        public IRecipe CreateOrUpdateRecipe(IRecipe recipe, string id = null)
        {
            var result = Recipe.FromInterface(recipe);
            var existingRecipe = false;
            Recipe recipeToUpdate = null;
            if (!string.IsNullOrEmpty(id))
            {
                recipeToUpdate = context.GetRecipe(id);
            }

            if (recipeToUpdate != null)
            {
                existingRecipe = true;
                result.Guid = recipeToUpdate.Guid;
                recipeToUpdate = result;
            }
            else
            {
                context.Recipes.Add(result);
            }

            if (existingRecipe)
            {
                var ingredients = recipeToUpdate.GetIngredients(context);
                var instructions = recipeToUpdate.GetInstructions(context);
                context.Ingredients.RemoveRange(ingredients);
                context.Instructions.RemoveRange(instructions);
            }
            context.Ingredients.AddRange(result.Ingredients);
            context.Instructions.AddRange(result.Instructions);

            context.SaveChanges();
            return result;
        }
    }
}
