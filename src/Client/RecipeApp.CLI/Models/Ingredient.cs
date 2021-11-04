using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.CLI.Models
{
    public class Ingredient : IIngredient
    {
        public string Guid { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
