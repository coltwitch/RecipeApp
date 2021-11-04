using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Managers
{
    public interface IMealPlanManager
    {
        public IEnumerable<IMealPlan> GetMealPlans();
        public IMealPlan GetMealPlanById(string id);
        public IMealPlan AddMealPlan(IMealPlan mealPlan);
        public IMealPlan GenerateShoppingList(IMealPlan mealPlan);
        public IMealPlan UpdateMealPlan(IMealPlan mealPlan, string id);
        public bool DeleteMealPlan(string id);

    }
}
