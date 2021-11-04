using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.CLI.Console;
using RecipeApp.CLI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp.CLI.ManagerHandlers
{
    public class RecipeManagerHandler
    {
        private IRecipeManager _recipeManager;
        private string latestId = string.Empty;
        private IConsoleUi _consoleUi;
        public RecipeManagerHandler(IRecipeManager recipeManager, IConsoleUi consoleUi)
        {
            _recipeManager = recipeManager;
            _consoleUi = consoleUi;
        }

        public void RunRecipe()
        {
            _consoleUi.Clear();
            
            var addRecipeOption = "AddRecipe";
            var getRecipesOption = "GetRecipes";
            var getRecipeByNameOption = "GetRecipeByName";
            var getRecipeByIdOption = "GetRecipeById";
            var deleteRecipeOption = "DeleteRecipe";
            var updateRecipeOption = "UpdateRecipe";
            var options = new List<string>
            {
                addRecipeOption,
                getRecipesOption,
                getRecipeByNameOption,
                getRecipeByIdOption,
                deleteRecipeOption,
                updateRecipeOption
            };

            var selection = _consoleUi.GetOptionFromUser("Please type which entry point you'd like to interact with:", options);

            _consoleUi.Clear();

            if (selection == addRecipeOption)
            {
                AddRecipe();
            }
            if (selection == getRecipesOption)
            {
                GetRecipes();
            }
            if (selection == getRecipeByNameOption)
            {
                GetRecipeByName();
            }
            if (selection == getRecipeByIdOption)
            {
                GetRecipeById();
            }
            if (selection == deleteRecipeOption)
            {
                DeleteRecipe();
            }
            if (selection == updateRecipeOption)
            {
                UpdateRecipe();
            }
        }

        private void GetId()
        {
            if (string.IsNullOrEmpty(latestId))
            {
                latestId = _consoleUi.GetStringFromUser("Enter ID of recipe");
            }
            else
            {
                var useLatest = _consoleUi.GetBoolFromUser("Use latestId (y/n)?: " + latestId);
                if (!useLatest)
                {
                    latestId = _consoleUi.GetStringFromUser("Enter ID of recipe");                    
                }
            }
        }

        private void AddRecipe()
        {
            var newRecipe = "(Add New Recipe)";
            var apples = "Apples";
            var bananas = "Bananas";
            var options = new List<string>
            {
                newRecipe,
                apples,
                bananas
            };
            var input = _consoleUi.GetOptionFromUser("Enter recipe name or select from the following templates:", options);
            
            Recipe recipe = null;

            if(input == newRecipe)
            {
                recipe = GetRecipeFromCmd();
            }
            else if (input == apples)
            {
                recipe = new Recipe()
                {
                    Name = "Apples",
                    Description = "It's just apples bro",
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient { Amount = 3, Name = "Apples", Unit = "Whole" }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction { OrderNumber = 1, Text = "Cut up your apples", Notes = "Just do it"}
                    }
                };
            }
            else if (input == bananas)
            {
                recipe = new Recipe()
                {
                    Name = "Bananas",
                    Description = "We're eating bananas tonight!",
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient { Amount = 3, Name = "Banana", Unit = "Whole" },
                        new Ingredient { Amount = 3, Name = "More Banana", Unit = "Halves" }
                    },
                    Instructions = new List<Instruction>()
                    {
                        new Instruction { OrderNumber = 0, Text = "Cut up your bananas", Notes = "However you'd like to cut them"},
                        new Instruction { OrderNumber = 1, Text = "Serve them on plate"}
                    }
                };
            }

            var result = _recipeManager.AddRecipe(recipe);
            _consoleUi.WriteLine();
            _consoleUi.WriteLine("Recipe Added!");
            _consoleUi.PrintObject(result);
        }

        private void GetRecipes()
        {
            var results = _recipeManager.GetRecipes().ToList();
            for (int i = 0; i < results.Count; i++)
            {
                _consoleUi.Spacer(3);
                var recipe = results[i];
                _consoleUi.WriteLine($"{i + 1 }: {recipe.Name} - {recipe.Description} ({recipe.Guid})");
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

        private void GetRecipeByName()
        {
            var input = _consoleUi.GetStringFromUser("Enter name of recipe");
            var result = _recipeManager.GetRecipeByName(input);
            if (result != null)
            {
                _consoleUi.WriteLine($"{result.Name}, {result.Description}");
                _consoleUi.PrintObject(result);
                latestId = result?.Guid;
                _consoleUi.WriteLine($"('{latestId}' Saved to latest)");
            }
            else
            {
                _consoleUi.WriteLine("Couldn't find corresponding recipe");
            }
        }

        private void GetRecipeById()
        {
            GetId();
            var result = _recipeManager.GetRecipeById(latestId);
            _consoleUi.Spacer(3);
            _consoleUi.PrintObject(result);
            _consoleUi.Spacer(3);
        }

        private void DeleteRecipe()
        {
            GetId();
            var result = _recipeManager.DeleteRecipe(latestId);
            if (result)
            {
                _consoleUi.WriteLine("Recipe Deleted!");
            }
            else
            {
                _consoleUi.WriteLine("Recipe not found");
            }
        }

        private void UpdateRecipe()
        {
            GetId();
            var recipe = GetRecipeFromCmd();
            recipe.Guid = latestId;
            var result = _recipeManager.UpdateRecipe(latestId, recipe);
            _consoleUi.WriteLine();
            _consoleUi.WriteLine("Recipe Updated!");
            _consoleUi.PrintObject(result);
        }

        private Recipe GetRecipeFromCmd(string name = null)
        {
            _consoleUi.Spacer();
            if (string.IsNullOrEmpty(name))
            {
                name = _consoleUi.GetStringFromUser("Enter recipe name");
            }
            else
            {
                _consoleUi.WriteLine("Recipe name:");
                _consoleUi.WriteLine(name);
            }
            _consoleUi.Spacer(10);
            var description = _consoleUi.GetStringFromUser("Enter recipe description");
            var ingredients = new List<Ingredient>();
            _consoleUi.Spacer(10);
            var ingredientNum = _consoleUi.GetPositiveIntFromUser("Enter number of ingredients");
            for (int i = 0; i < ingredientNum; i++)
            {
                _consoleUi.Spacer(3);
                _consoleUi.WriteLine($"Ingredient #{i + 1}:");
                var ingredientName = _consoleUi.GetStringFromUser("Enter ingredient name");
                _consoleUi.Spacer(3);
                var ingredientAmount = _consoleUi.GetDecimalFromUser("Enter ingredient amount");
                _consoleUi.Spacer(3);
                var ingredientUnit = _consoleUi.GetStringFromUser("Enter ingredient amount unit");
                ingredients.Add(new Ingredient { Name = ingredientName, Amount = ingredientAmount, Unit = ingredientUnit });
            }

            _consoleUi.Spacer(10);
            var instructions = new List<Instruction>();
            var instructionNum = _consoleUi.GetPositiveIntFromUser("Enter number of instructions");
            for (int i = 0; i < instructionNum; i++)
            {
                _consoleUi.Spacer(3);
                _consoleUi.WriteLine($"Instruction #{i + 1}:");
                _consoleUi.Spacer(3);
                var instructionText = _consoleUi.GetStringFromUser("Enter instruction text");
                _consoleUi.Spacer(3);
                var instructionNotes = _consoleUi.GetStringFromUser("Enter instruction notes");
                instructions.Add(new Instruction { Text = instructionText, Notes = instructionNotes, OrderNumber = i + 1 });
            }
            _consoleUi.Spacer();

            var recipe = new Recipe()
            {
                Name = name,
                Description = description,
                Ingredients = ingredients,
                Instructions = instructions
            };

            return recipe;
        }
    }
}
