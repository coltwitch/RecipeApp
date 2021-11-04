using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.API.Models
{
    public class ShoppingListItemRequest : IShoppingListItem
    {
        public string ItemGuid { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemCount { get; set; }
        public bool Purchased { get; set; }
        public string MealPlanGuid { get; set; }
    }
}
