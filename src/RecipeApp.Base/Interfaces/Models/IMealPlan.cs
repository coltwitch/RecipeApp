using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Models
{
    public interface IMealPlan
    {
        public string Guid { get; set; }
        public IEnumerable<IRecipe> Recipes { get; set; }
        public IEnumerable<IShoppingListItem> ShoppingList { get; set; }
        public IUser User { get; set; }
    }
}
