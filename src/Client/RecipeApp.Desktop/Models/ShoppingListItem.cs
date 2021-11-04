using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Desktop.Models
{
    public class ShoppingListItem : IShoppingListItem
    {
        public string ItemGuid { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemCount { get; set; }
        public bool Purchased { get; set; }
        public string MealPlanGuid { get; set; }

        public static ShoppingListItem FromInterface(IShoppingListItem shoppingListItem)
        {

            if (string.IsNullOrEmpty(shoppingListItem.ItemGuid))
            {
                shoppingListItem.ItemGuid = Guid.NewGuid().ToString();
            }
            var output = new ShoppingListItem()
            {
                ItemGuid = shoppingListItem.ItemGuid,
                ItemName = shoppingListItem.ItemName,
                ItemUnit = shoppingListItem.ItemUnit,
                ItemCount = shoppingListItem.ItemCount,
                Purchased = shoppingListItem.Purchased,
                MealPlanGuid = shoppingListItem.MealPlanGuid,
            };
            return output;
        }
    }
}
