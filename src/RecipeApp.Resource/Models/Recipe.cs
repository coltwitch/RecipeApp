using MoreLinq;
using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Resource.ResourceAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipeApp.Resource.Models
{
    public class Recipe : IRecipe
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        
        [JsonIgnore]
        [NotMapped]
        IEnumerable<IIngredient> IRecipe.Ingredients 
        { 
            get { return Ingredients; } 
            set { Ingredients = (ICollection<Ingredient>)value; }
        }

        [JsonIgnore]
        [NotMapped]
        IEnumerable<IInstruction> IRecipe.Instructions 
        { 
            get { return Instructions;  } 
            set { Instructions = (ICollection<Instruction>)value;  } 
        }

        public static Recipe FromInterface(IRecipe recipe)
        {
            if (string.IsNullOrEmpty(recipe.Guid))
            {
                recipe.Guid = System.Guid.NewGuid().ToString();
            }
            var output = new Recipe()
            {
                Guid = recipe.Guid,
                Name = recipe.Name,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients.Select(x => Ingredient.FromInterface(x)).ToList(),
                Instructions = recipe.Instructions.Select(x => Instruction.FromInterface(x)).ToList()
            };
            output.Ingredients.ForEach(x => x.RecipeGuid = output.Guid);
            output.Instructions.ForEach(x => x.RecipeGuid = output.Guid);
            return output;
        }
        
        public List<Ingredient> GetIngredients(RecipeAppSqliteContext context)
        {

            var ingredients = context.Ingredients.Where(x => x.RecipeGuid == Guid);
            return ingredients.ToList();
        }

        public List<Instruction> GetInstructions(RecipeAppSqliteContext context)
        {

            var instructions = context.Instructions.Where(x => x.RecipeGuid == Guid);
            return instructions.ToList();
        }
    }
}
