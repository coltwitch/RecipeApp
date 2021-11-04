using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Desktop.Models
{
    public class Ingredient : IIngredient
    {
        public string Guid { get; set; }
        public string RecipeGuid { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public static Ingredient FromInterface(IIngredient ingredient)
        {
            if (string.IsNullOrEmpty(ingredient.Guid))
            {
                ingredient.Guid = System.Guid.NewGuid().ToString();
            }
            var output = new Ingredient()
            {
                Guid = ingredient.Guid,
                Name = ingredient.Name,
                Amount = ingredient.Amount,
                Unit = ingredient.Unit
            };
            return output;
        }
    }
}
