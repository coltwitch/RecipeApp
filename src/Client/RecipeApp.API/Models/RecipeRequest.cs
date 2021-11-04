using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipeApp.API.Models
{
    public class RecipeRequest : IRecipe
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<IngredientRequest> Ingredients { get; set; }
        public ICollection<InstructionRequest> Instructions { get; set; }

        [JsonIgnore]
        IEnumerable<IIngredient> IRecipe.Ingredients
        {
            get { return Ingredients; }
            set { Ingredients = (ICollection<IngredientRequest>)value; }
        }

        [JsonIgnore]
        IEnumerable<IInstruction> IRecipe.Instructions
        {
            get { return Instructions; }
            set { Instructions = (ICollection<InstructionRequest>)value; }
        }
    }
}
