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
    public class RecipesTab
    {
        private IRecipeManager _recipeManager;
        private Label _nameLabel;
        private Label _descriptionLabel;
        private TextBox _searchTextBox;
        private Button _searchButton;
        private ComboBox _searchComboBox;
        private ListBox _directionsListBox;
        private ListBox _ingredientsListBox;

        public RecipesTab(
            IRecipeManager recipeManager,
            Label nameLabel, 
            Label descLabel, 
            TextBox searchTextBox, 
            Button searchButton, 
            ComboBox searchComboBox, 
            ListBox ingredientListBox,
            ListBox directionsListBox
        )
        {
            _recipeManager = recipeManager;
            _nameLabel = nameLabel;
            _descriptionLabel = descLabel;
            _searchTextBox = searchTextBox;
            _searchButton = searchButton;
            _searchComboBox = searchComboBox;
            _directionsListBox = directionsListBox;
            _ingredientsListBox = ingredientListBox;

            _searchComboBox.SelectionChanged -= SearchSelectionChanged;
            _searchComboBox.SelectionChanged += SearchSelectionChanged;

            _searchButton.Click -= SearchClicked;
            _searchButton.Click += SearchClicked;

            ReloadRecipes();
        }

        public void ResetTab(bool clearSearchBox = true)
        {
            if (clearSearchBox)
            {
                _searchComboBox.SelectedItem = null;
            }

            _nameLabel.Content = string.Empty;
            _descriptionLabel.Content = string.Empty;
            _searchTextBox.Text = string.Empty;
            _directionsListBox.ItemsSource = new List<string>();
            _ingredientsListBox.ItemsSource = new List<string>();
        }

        public void ReloadRecipes()
        {
            ResetTab();
            var recipes = _recipeManager.GetRecipes().Select(x => x.Name).ToList();
            _searchComboBox.ItemsSource = recipes;
        }

        private void LoadRecipeToRecipesTab(IRecipe recipe)
        {
            _nameLabel.Content = recipe.Name;
            _descriptionLabel.Content = recipe.Description;
            var ingredients = new List<string>();
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredients.Add($"{ingredient.Name}: {ingredient.Amount} - {ingredient.Unit}");
            }
            _ingredientsListBox.ItemsSource = ingredients;
            var directions = new List<string>();
            foreach (var direction in recipe.Instructions.OrderBy(x => x.OrderNumber))
            {
                var directionText = $"{direction.OrderNumber}:\n{direction.Text}\n\t{direction.Notes}\n\n";
                directions.Add(directionText);
            }
            _directionsListBox.ItemsSource = directions;
        }

        #region Events
        private void SearchSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetTab(false);
            var recipeName = ((ComboBox)sender).SelectedItem?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(recipeName))
            {
                var recipe = _recipeManager.GetRecipeByName(recipeName);
                LoadRecipeToRecipesTab(recipe);
            }
        }

        private void SearchClicked(object sender, RoutedEventArgs e)
        {
            var recipeName = 
            _searchComboBox.SelectedItem = _searchTextBox.Text;
            if (_searchComboBox.SelectedItem.ToString() != _searchTextBox.Text)
            {
                MessageBox.Show($"Couldn't find recipe: {_searchTextBox.Text}");
            }
            _searchTextBox.Text = string.Empty;
        }
        #endregion
    }
}
