using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp.Desktop.Models
{
    public class AddRecipesTab
    {
        private IRecipeManager _recipeManager;
        private const string addNewSelection = "( add new recipe)";
        private int _currentIngredientIndex;
        private int _currentDirectionIndex;

        private Label _guidLabel;
        
        private TextBox _nameTextBox;
        private TextBox _descriptionTextBox;
        private TextBox _ingredientNameTextBox;
        private TextBox _ingredientAmountTextBox;
        private TextBox _ingredientUnitTextBox;
        private TextBox _directionTextTextBox;
        private TextBox _directionNotesTextBox;

        private ComboBox _searchComboBox;

        private Button _saveRecipe;
        private Button _deleteRecipe;
        private Button _addIngredient;
        private Button _updateIngredient;
        private Button _deleteIngredient;
        private Button _addDirection;
        private Button _updateDirection;
        private Button _deleteDirection;
        
        private TreeView _recipeTreeView;
        private Recipe _recipe;


        public AddRecipesTab(
            IRecipeManager recipeManager,
            Label guidLabel,
            TextBox nameTextBox,
            TextBox descriptionTextBox,
            TextBox ingredientNameTextBox,
            TextBox ingredientAmountTextBox,
            TextBox ingredientUnitTextBox,
            TextBox directionTextTextBox,
            TextBox directionNotesTextBox,
            ComboBox searchComboBox,
            Button saveRecipe,
            Button deleteRecipe,
            Button addIngredient,
            Button updateIngredient,
            Button deleteIngredient,
            Button addDirection,
            Button updateDirection,
            Button deleteDirection,
            TreeView recipeTreeView
        )
        {
            _recipeManager = recipeManager;
            _guidLabel = guidLabel;
            _nameTextBox = nameTextBox;
            _descriptionTextBox = descriptionTextBox;
            _ingredientNameTextBox = ingredientNameTextBox;
            _ingredientAmountTextBox = ingredientAmountTextBox;
            _ingredientUnitTextBox = ingredientUnitTextBox;
            _directionTextTextBox = directionTextTextBox;
            _directionNotesTextBox = directionNotesTextBox;
            _searchComboBox = searchComboBox;
            _saveRecipe = saveRecipe;
            _deleteRecipe = deleteRecipe;
            _addIngredient = addIngredient;
            _updateIngredient = updateIngredient;
            _deleteIngredient = deleteIngredient;
            _addDirection = addDirection;
            _updateDirection = updateDirection;
            _deleteDirection = deleteDirection;
            _recipeTreeView = recipeTreeView;

            _searchComboBox.SelectionChanged -= SearchSelectionChanged;
            _searchComboBox.SelectionChanged += SearchSelectionChanged;

            _saveRecipe.Click -= SaveClicked;
            _saveRecipe.Click += SaveClicked;
            _deleteRecipe.Click -= DeleteClicked;
            _deleteRecipe.Click += DeleteClicked;

            _addIngredient.Click -= AddIngredientClicked;
            _addIngredient.Click += AddIngredientClicked;
            _updateIngredient.Click -= UpdateIngredientClicked;
            _updateIngredient.Click += UpdateIngredientClicked;
            _deleteIngredient.Click -= DeleteIngredientClicked;
            _deleteIngredient.Click += DeleteIngredientClicked;

            _addDirection.Click -= AddDirectionClicked;
            _addDirection.Click += AddDirectionClicked;
            _updateDirection.Click -= UpdateDirectionClicked;
            _updateDirection.Click += UpdateDirectionClicked;
            _deleteDirection.Click -= DeleteDirectionClicked;
            _deleteDirection.Click += DeleteDirectionClicked;

            ReloadRecipes();
        }

        public void ResetTab(bool clearSearchBox = true)
        {
            if (clearSearchBox)
            {
                _searchComboBox.SelectedItem = null;
            }
            _currentDirectionIndex = -1;
            _currentIngredientIndex = -1;
            _guidLabel.Content = string.Empty;
            _nameTextBox.Text = string.Empty;
            _descriptionTextBox.Text = string.Empty;
            _ingredientNameTextBox.Text = string.Empty;
            _ingredientAmountTextBox.Text = string.Empty;
            _ingredientUnitTextBox.Text = string.Empty;
            _directionTextTextBox.Text = string.Empty;
            _directionNotesTextBox.Text = string.Empty;
            _recipeTreeView.ItemsSource = new List<TreeViewItem>();
        }

        public void ReloadRecipes(string startWithRecipeSelected = null)
        {
            if (string.IsNullOrEmpty(startWithRecipeSelected))
            {
                ResetTab();
                var recipes = new List<string> { addNewSelection };
                recipes.AddRange(_recipeManager.GetRecipes()?.Select(x => x.Name)?.ToList());
                _searchComboBox.ItemsSource = recipes;
            }
            else
            {
                _searchComboBox.SelectionChanged -= SearchSelectionChanged;

                ResetTab();
                var recipes = new List<string> { addNewSelection };
                recipes.AddRange(_recipeManager.GetRecipes()?.Select(x => x.Name)?.ToList());
                _searchComboBox.ItemsSource = recipes;

                _searchComboBox.SelectedItem = startWithRecipeSelected;
                _searchComboBox.SelectionChanged += SearchSelectionChanged;
            }
        }

        private void SaveRecipe(IRecipe recipe)
        {
            _recipe = Recipe.FromInterface(recipe);
            if (string.IsNullOrEmpty(_recipe.Guid))
            {
                _recipeManager.AddRecipe(_recipe);
            }
            else
            {
                _recipeManager.UpdateRecipe(_recipe.Guid, _recipe);
            }
            ReloadRecipe();
        }
        private void DeleteRecipe()
        {
            if (!string.IsNullOrEmpty(_recipe.Guid))
            {
                _recipeManager.DeleteRecipe(_recipe.Guid);
            }
            ReloadRecipes();
        }

        private void ReloadRecipe()
        {            
            ReloadRecipes(_recipe.Name);

            _nameTextBox.Text = _recipe.Name;
            _guidLabel.Content = _recipe.Guid;
            _descriptionTextBox.Text = _recipe.Description;
            TreeViewItem recipeRoot = new();
            recipeRoot.IsExpanded = true;
            _ = recipeRoot.Items.Add(new TreeViewItem() { Header = _recipe.Name });
            _ = recipeRoot.Items.Add(new TreeViewItem() { Header = _recipe.Description });

            TreeViewItem ingredients = new() { Header = "Ingredients"};
            ingredients.IsExpanded = true;
            for(int i = 0; i < _recipe.Ingredients.Count; i++)
            {
                var ingredient = _recipe.Ingredients.ToArray()[i];
                var ingredientTreeViewItem = new TreeViewItem() { Header = ingredient.Name };
                ingredientTreeViewItem.Selected += new RoutedEventHandler((sender, e) => IngredientSelected(ingredient));
                ingredients.Items.Add(ingredientTreeViewItem);
            }
            _ = recipeRoot.Items.Add(ingredients);

            TreeViewItem directions = new() { Header = "Directions"};
            directions.IsExpanded = true;
            for (int i = 0; i < _recipe.Instructions.Count; i++)
            {
                var direction = _recipe.Instructions.ToArray()[i];
                var directionTreeViewItem = new TreeViewItem() { Header = "Step " + direction.OrderNumber };
                directionTreeViewItem.Selected += new RoutedEventHandler((sender, e) => DirectionSelected(direction)); ;
                directions.Items.Add(directionTreeViewItem);
            }
            _ = recipeRoot.Items.Add(directions);

            _recipeTreeView.ItemsSource = new List<TreeViewItem> { recipeRoot };
        }
        
        private void LoadIngredient(Ingredient ingredient, int index)
        {
            _currentIngredientIndex = index;
            _currentDirectionIndex = -1;
            _ingredientNameTextBox.Text = ingredient.Name;
            _ingredientAmountTextBox.Text = ingredient.Amount.ToString();
            _ingredientUnitTextBox.Text = ingredient.Unit;

            _directionTextTextBox.Text = string.Empty;
            _directionNotesTextBox.Text = string.Empty;
        }

        private void LoadDirection(Instruction instruction, int index)
        {
            _currentIngredientIndex = -1;
            _currentDirectionIndex = index;
            _directionTextTextBox.Text = instruction.Text;
            _directionNotesTextBox.Text = instruction.Notes;

            _ingredientNameTextBox.Text = string.Empty;
            _ingredientAmountTextBox.Text = string.Empty;
            _ingredientUnitTextBox.Text = string.Empty;
        }

        private void SaveIngredient(bool update = false)
        {
            Ingredient ingredient = new Ingredient
            {
                Name = _ingredientNameTextBox.Text,
                Amount = Decimal.Parse(_ingredientAmountTextBox.Text),
                Unit = _ingredientUnitTextBox.Text,
                RecipeGuid = _recipe.Guid
            };
            if (update)
            {
                var ingredients = _recipe.Ingredients.ToList();
                ingredients[_currentIngredientIndex] = ingredient;
                _recipe.Ingredients = ingredients;
            }
            else
            {
                _recipe.Ingredients.Add(ingredient);
            }
            _ingredientNameTextBox.Text = string.Empty;
            _ingredientAmountTextBox.Text = string.Empty;
            _ingredientUnitTextBox.Text = string.Empty;
            SaveRecipe(_recipe);
        }
        private void DeleteIngredient()
        {
            if (_currentIngredientIndex < 0)
            {
                return;
            }
            var ingredients = _recipe.Ingredients.ToList();
            var ingredientToRemove = ingredients[_currentIngredientIndex];
            ingredients.Remove(ingredientToRemove);
            _recipe.Ingredients = ingredients;
            _currentIngredientIndex = -1;
            foreach (var item in _recipeTreeView.Items)
            {
                ((TreeViewItem)item).IsSelected = false;
            }
            SaveRecipe(_recipe);
        }

        private void SaveDirection(bool update = false)
        {
            Instruction instruction = new Instruction
            {
                Text = _directionTextTextBox.Text,
                Notes = _directionNotesTextBox.Text,
                RecipeGuid = _recipe.Guid
            };
            _recipe.Instructions = _recipe.Instructions.OrderBy(x => x.OrderNumber).ToList();
            for (int i = 0; i < _recipe.Instructions.Count(); i++)
            {
                _recipe.Instructions.ToArray()[i].OrderNumber = i + 1;
            }
            if (update)
            {
                var instructions = _recipe.Instructions.ToList();
                instruction.OrderNumber = _currentDirectionIndex + 1;
                instructions[_currentDirectionIndex] = instruction;
                _recipe.Instructions = instructions;
            }
            else
            {
                _currentDirectionIndex = _recipe.Instructions.Count;
                instruction.OrderNumber = _currentDirectionIndex + 1;
                _recipe.Instructions.Add(instruction);
            }
            _directionTextTextBox.Text = string.Empty;
            _directionNotesTextBox.Text = string.Empty;
            SaveRecipe(_recipe);
        }
        private void DeleteDirection()
        {
            if (_currentDirectionIndex < 0)
            {
                return;
            }
            var directions = _recipe.Instructions.ToList();
            var directionToRemove = directions[_currentDirectionIndex];
            directions.Remove(directionToRemove);
            _recipe.Instructions = directions;
            for (int i = 0; i < _recipe.Instructions.Count(); i++)
            {
                _recipe.Instructions.ToArray()[i].OrderNumber = i + 1;
            }
            _currentDirectionIndex = -1;
            foreach (var item in _recipeTreeView.Items)
            {
                ((TreeViewItem)item).IsSelected = false;
            }
            SaveRecipe(_recipe);
        }

        #region Events
        private void IngredientSelected(Ingredient ingredient)
        {
            var index = _recipe.Ingredients.ToList().IndexOf(ingredient);
            LoadIngredient(ingredient, index);
        }

        private void DirectionSelected(Instruction direction)
        {
            var index = _recipe.Instructions.ToList().IndexOf(direction);
            LoadDirection(direction, index);
        }

        private void SearchSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetTab(false);
            var recipeName = ((ComboBox)sender).SelectedItem?.ToString() ?? string.Empty;
            if (recipeName == addNewSelection)
            {
                _nameTextBox.Text = "Enter Recipe Name";
                _nameTextBox.Focus();
                _nameTextBox.SelectAll();
            }
            else if (!string.IsNullOrEmpty(recipeName))
            {
                var recipe = _recipeManager.GetRecipeByName(recipeName);
                _recipe = Recipe.FromInterface(recipe);
                ReloadRecipe();
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (_recipe == null)
            {
                _recipe = new Recipe();
            }
            _recipe.Name = _nameTextBox.Text;
            _recipe.Description = _descriptionTextBox.Text;
            _recipe.Guid = _guidLabel.ContentStringFormat;

            var shouldSave = true;
            if (string.IsNullOrEmpty(_recipe.Guid))
            {
                _recipe.Guid = _recipeManager.GetRecipeByName(_recipe.Name)?.Guid ?? string.Empty;
                if (!string.IsNullOrEmpty(_recipe.Guid))
                {
                    shouldSave = false;
                    MessageBox.Show("Recipe already exists, loading from database");
                    _searchComboBox.SelectedItem = _recipe.Name;
                }
            }
            if (shouldSave)
            {
                SaveRecipe(_recipe);
            }
        }
        private void DeleteClicked(object sender, RoutedEventArgs e)
        {
            DeleteRecipe();
        }

        private void AddIngredientClicked(object sender, RoutedEventArgs e)
        {
            SaveIngredient();
        }
        private void UpdateIngredientClicked(object sender, RoutedEventArgs e)
        {
            SaveIngredient(true);
        }
        private void DeleteIngredientClicked(object sender, RoutedEventArgs e)
        {
            DeleteIngredient();
        }

        private void AddDirectionClicked(object sender, RoutedEventArgs e)
        {
            SaveDirection();
        }
        private void UpdateDirectionClicked(object sender, RoutedEventArgs e)
        {
            SaveDirection(true);
        }
        private void DeleteDirectionClicked(object sender, RoutedEventArgs e)
        {
            DeleteDirection();
        }

        #endregion
    }
}
