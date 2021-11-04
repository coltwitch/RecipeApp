using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Models
{
    public interface IRecipe
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<IIngredient> Ingredients { get; set; }
        public IEnumerable<IInstruction> Instructions { get; set; }
    }
}
