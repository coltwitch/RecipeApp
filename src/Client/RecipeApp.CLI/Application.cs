using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.CLI.Console;
using RecipeApp.CLI.ManagerHandlers;
using System.Collections.Generic;

namespace RecipeApp.CLI
{
    public class Application
    {
        private IMealPlanManager _mealPlanManager;
        private IRecipeManager _recipeManager;
        private RecipeManagerHandler _recipeManagerHandler;
        private MealPlanManagerHandler _mealPlanManagerHandler;
        private IConsoleUi _consoleUi;
        public Application(IMealPlanManager mealPlanManager, IRecipeManager recipeManager, IConsoleUi consoleUi)
        {
            _mealPlanManager = mealPlanManager;
            _recipeManager = recipeManager;
            _recipeManagerHandler = new RecipeManagerHandler(_recipeManager, consoleUi);
            _mealPlanManagerHandler = new MealPlanManagerHandler(_mealPlanManager, _recipeManager, consoleUi);
            _consoleUi = consoleUi;
        }
        public void Run()
        {
            var quit = false;
            while (!quit)
            {
                _consoleUi.Clear();
                var mealPlanOption = "MealPlan";
                var recipeOption = "Recipe";

                var selection = _consoleUi.GetOptionFromUser("Please type which Manager you'd like to interact with:", 
                    new List<string> { mealPlanOption, recipeOption }
                );

                if (selection == mealPlanOption)
                {
                    _mealPlanManagerHandler.RunMealPlan();
                }
                else if (selection == recipeOption)
                {
                    _recipeManagerHandler.RunRecipe();
                }
                else if (string.IsNullOrEmpty(selection))
                {
                    quit = true;
                }
                _consoleUi.Spacer();
                _consoleUi.WaitForEnter();
            }
        }
    }
}
