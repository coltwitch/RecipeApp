using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Managers
{
    public interface IRecipeManager
    {
        public IEnumerable<IRecipe> GetRecipes();
        public IRecipe GetRecipeByName(string name);
        public IRecipe GetRecipeById(string guid);

        //public IRecipe BeginRecipe(string name, string description);
        //public IRecipe AddIngredient(string guid, string name, string unit, decimal amount);
        //public IRecipe AddInstruction(string guid, string text, string notes);
        public IRecipe AddRecipe(IRecipe recipe);
        public IRecipe UpdateRecipe(string guid, IRecipe recipe);
        public bool DeleteRecipe(string guid);
    }
}
