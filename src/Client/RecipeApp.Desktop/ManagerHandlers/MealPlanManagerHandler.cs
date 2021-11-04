using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Desktop.ManagerHandlers
{
    public class MealPlanManagerHandler : IMealPlanManager
    {
        private IMealPlanManager _mealPlanManager;
        public MealPlanManagerHandler(IMealPlanManager mealPlanManager, MainWindow mainWindow)
        {
            _mealPlanManager = mealPlanManager;
        }

        public IMealPlan AddMealPlan(IMealPlan mealPlan)
        {
            return _mealPlanManager.AddMealPlan(mealPlan);
        }

        public bool DeleteMealPlan(string id)
        {
            return _mealPlanManager.DeleteMealPlan(id);
        }

        public IMealPlan GenerateShoppingList(IMealPlan mealPlan)
        {
            return _mealPlanManager.GenerateShoppingList(mealPlan);
        }

        public IMealPlan GetMealPlanById(string id)
        {
            return _mealPlanManager.GetMealPlanById(id);
        }

        public IEnumerable<IMealPlan> GetMealPlans()
        {
            return _mealPlanManager.GetMealPlans();
        }

        public IMealPlan UpdateMealPlan(IMealPlan mealPlan, string id)
        {
            return _mealPlanManager.UpdateMealPlan(mealPlan, id);
        }
    }
}
