using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using RecipeApp.CLI.Console;
using RecipeApp.CLI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.CLI.ManagerHandlers
{
    public class MealPlanManagerHandler
    {
        private IMealPlanManager _mealPlanManager;
        private IRecipeManager _recipeManager;
        private IConsoleUi _consoleUi;
        private string latestId = string.Empty;

        public MealPlanManagerHandler(IMealPlanManager mealPlanManager, IRecipeManager recipeManager, IConsoleUi consoleUi)
        {
            _mealPlanManager = mealPlanManager;
            _recipeManager = recipeManager;
            _consoleUi = consoleUi;
        }

        public void RunMealPlan()
        {

            _consoleUi.Clear();

            var addMealPlanOption = "AddMealPlan";
            var getMealPlansOption = "GetMealPlans";
            var getMealPlanByIdOption = "GetMealPlanById";
            var deleteMealPlanOption = "DeleteMealPlan";
            var updateMealPlanOption = "UpdateMealPlan";
            var options = new List<string>
            {
                addMealPlanOption,
                getMealPlansOption,
                getMealPlanByIdOption,
                deleteMealPlanOption,
                updateMealPlanOption
            };

            var selection = _consoleUi.GetOptionFromUser("Please type which entry point you'd like to interact with:", options);

            _consoleUi.Clear();

            if (selection == addMealPlanOption)
            {
                AddMealPlan();
            }
            if (selection == getMealPlansOption)
            {
                GetMealPlans();
            }
            if (selection == getMealPlanByIdOption)
            {
                GetMealPlanById();
            }
            if (selection == deleteMealPlanOption)
            {
                DeleteMealPlan();
            }
            if (selection == updateMealPlanOption)
            {
                UpdateMealPlan();
            }
        }

        public void GetMealPlanById()
        {
            GetId();
            var mealPlan = _mealPlanManager.GetMealPlanById(latestId);
            _consoleUi.PrintObject(mealPlan);
        }

        public void GetMealPlans()
        {
            var results = _mealPlanManager.GetMealPlans().ToList();
            for (int i = 0; i < results.Count; i++)
            {
                _consoleUi.Spacer(3);
                var mealPlan = results[i];
                _consoleUi.WriteLine($"{i + 1 }: {mealPlan.Guid}\n\t{string.Join(", ", mealPlan.Recipes.Select(x => x.Name))}");
            }
            _consoleUi.Spacer(3);
            var numSelected = _consoleUi.GetPositiveIntFromUser("Enter number to store id in lookup");
            if (numSelected > 0 && numSelected <= results.Count)
            {
                var id = results[numSelected - 1]?.Guid;
                if (!string.IsNullOrEmpty(id))
                {
                    _consoleUi.WriteLine($"('{id}' Saved to latest)");
                    latestId = id;
                }
            }
        }

        public void AddMealPlan()
        {
            var mealPlan = GetMealPlanFromCmd();
            var result = _mealPlanManager.AddMealPlan(mealPlan);
            _consoleUi.Spacer();
            _consoleUi.PrintObject(result);
        }

        public void UpdateMealPlan()
        {
            GetId();
            var mealPlan = GetMealPlanFromCmd();
            var result = _mealPlanManager.UpdateMealPlan(mealPlan, latestId);
            _consoleUi.PrintObject(result);
        }

        public void DeleteMealPlan()
        {
            GetId();
            var success = _mealPlanManager.DeleteMealPlan(latestId);
            _consoleUi.PrintObject(success);
        }

        private void GetId()
        {
            if (string.IsNullOrEmpty(latestId))
            {
                latestId = _consoleUi.GetStringFromUser("Enter ID of mealPlan");
            }
            else
            {
                var useLatest = _consoleUi.GetBoolFromUser("Use latestId (y/n)?: " + latestId);
                if (!useLatest)
                {
                    latestId = _consoleUi.GetStringFromUser("Enter ID of mealPlan");
                }
            }
        }

        private MealPlan GetMealPlanFromCmd()
        {
            _consoleUi.Clear();
            var userId = _consoleUi.GetStringFromUser("Enter user id for MealPlan");
            var remainingRecipes = _recipeManager.GetRecipes().ToList();
            var selectedRecipes = new List<IRecipe>();
            var doneSelecting = false;
            while (!doneSelecting)
            {
                var options = new List<string>();
                foreach(var remainingRecipe in remainingRecipes)
                {
                    options.Add(remainingRecipe.Name);
                }
                _consoleUi.Clear();
                _consoleUi.WriteLine("Selected recipes:");
                _consoleUi.WriteLine(string.Join(", ", selectedRecipes.Select(x => x.Name)));
                _consoleUi.Spacer(3);
                _consoleUi.WriteLine();
                var option = _consoleUi.GetOptionFromUser("Select recipe to add to meal plan:", options);

                if (string.IsNullOrEmpty(option))
                {
                    doneSelecting = true;
                }
                else
                {
                    var recipe = remainingRecipes.FirstOrDefault(x => x.Name == option);
                    if (recipe != null)
                    {
                        selectedRecipes.Add(recipe);
                        remainingRecipes.Remove(recipe);
                    }
                }
            }

            var mealPlan = new MealPlan()
            {
                User = new User { UserId = userId },
                Recipes = selectedRecipes
            };

            return mealPlan;
        }
    }
}
