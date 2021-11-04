using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.CLI.Models
{
    public class Recipe : IRecipe
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<IIngredient> Ingredients { get; set; }
        public IEnumerable<IInstruction> Instructions { get; set; }

    }
}
