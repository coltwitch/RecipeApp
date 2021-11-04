using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Models
{
    public interface IIngredient
    {
        public string Guid { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
