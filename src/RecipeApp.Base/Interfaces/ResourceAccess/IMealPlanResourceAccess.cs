using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.ResourceAccess
{
    public interface IMealPlanResourceAccess
    {
        public IEnumerable<IMealPlan> GetMealPlans();
        public IMealPlan GetMealPlan(string id);
        public IMealPlan CreateOrUpdateMealPlan(IMealPlan mealPlan, string id = null);
        public bool DeleteMealPlan(string id);
    }
}
