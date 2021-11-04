using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.ResourceAccess
{
    public interface IRecipeResourceAccess
    {
        public IRecipe CreateOrUpdateRecipe(IRecipe recipe, string id = null);
        public IEnumerable<IRecipe> GetRecipes(string name = null);
        public IRecipe GetRecipe(string id);
        public bool DeleteRecipe(string id);
    }
}
