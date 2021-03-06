using RecipeApp.Base.Interfaces.Models;

namespace RecipeApp.Base.Managers.Models
{
    public class ShoppingListItem : IShoppingListItem
    {
        public string ItemGuid { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemCount { get; set; }
        public bool Purchased { get; set; }
        public string MealPlanGuid { get; set; }
    }
}
