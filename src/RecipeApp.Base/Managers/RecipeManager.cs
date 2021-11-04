using Microsoft.Extensions.Logging;
using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Base.Interfaces.ResourceAccess;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.Base.Managers
{
    public class RecipeManager : IRecipeManager
    {
        private IRecipeResourceAccess _recipeResourceAccess;
        private ILogger<RecipeManager> _logger;

        public RecipeManager(IRecipeResourceAccess recipeResourceAccess, ILogger<RecipeManager> logger)
        {
            _recipeResourceAccess = recipeResourceAccess;
            _logger = logger;
        }

        public IEnumerable<IRecipe> GetRecipes()
        {
            _logger.LogInformation("Getting recipes");
            return _recipeResourceAccess.GetRecipes();
        }

        public IRecipe GetRecipeByName(string name)
        {
            _logger.LogInformation($"Getting recipe by name {name}");
            return _recipeResourceAccess.GetRecipes(name).FirstOrDefault();
        }

        public IRecipe GetRecipeById(string guid)
        {
            _logger.LogInformation($"Getting recipe by id {guid}");
            return _recipeResourceAccess.GetRecipe(guid);
        }

        public IRecipe AddRecipe(IRecipe recipe)
        {
            _logger.LogInformation($"Adding recipe {recipe.Name}");
            var result = _recipeResourceAccess.CreateOrUpdateRecipe(recipe);
            return result;
        }

        public IRecipe UpdateRecipe(string guid, IRecipe recipe)
        {
            _logger.LogInformation($"Updating recipe {guid}");
            var result = _recipeResourceAccess.CreateOrUpdateRecipe(recipe, guid);
            return result;
        }

        public bool DeleteRecipe(string guid)
        {
            _logger.LogInformation($"Deleting recipe {guid}");
            var result = _recipeResourceAccess.DeleteRecipe(guid);
            return result;
        }
    }
}
