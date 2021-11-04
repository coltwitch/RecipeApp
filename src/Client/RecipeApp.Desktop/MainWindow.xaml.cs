using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Utility;
using RecipeApp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeApp.Resource;
using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Interfaces.Models;
using RecipeApp.Desktop.ManagerHandlers;
using RecipeApp.Desktop.Models;

namespace RecipeApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MealPlanManagerHandler _mealPlanHandler;
        private RecipesTab _recipesTab;
        private AddRecipesTab _addRecipesTab;

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _recipesTab = new RecipesTab(
                serviceProvider.GetService<IRecipeManager>(),
                RecipeLbl,
                RecipeDescriptionLbl,
                RecipeSearchTxt,
                RecipeSearchBtn,
                RecipeSearchCmb,
                RecipeIngredientsListBox,
                RecipeDirectionsListBox
            );

            _addRecipesTab = new AddRecipesTab(
                serviceProvider.GetService<IRecipeManager>(),
                AddRecipeGuidLabel,
                AddRecipeNameTextBox,
                AddRecipeDescriptionTextBox,
                AddRecipeIngredientName,
                AddRecipeIngredientAmount,
                AddRecipeIngredientUnit,
                AddRecipeDirectionTextTextBox,
                AddRecipeDirectionNotesTextBox,
                AddRecipeSearchCmb,
                AddRecipeSaveButton,
                AddRecipeDeleteRecipeButton,
                AddRecipeAddIngredientButton,
                AddRecipeUpdateIngredientButton,
                AddRecipeDeleteIngredientButton,
                AddRecipeAddDirectionButton,
                AddRecipeUpdateDirectionButton,
                AddRecipeDeleteDirectionButton,
                AddRecipeTreeView
            );

            _mealPlanHandler = new MealPlanManagerHandler(serviceProvider.GetService<IMealPlanManager>(), this);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            RecipeAppUtilityService.ConfigureServices(services);
            RecipeAppResourceAccessService.ConfigureServices(services);
            RecipeAppBaseService.ConfigureServices(services);
        }

        private void RecipeTab_Clicked(object sender, EventArgs e)
        {
            _recipesTab.ReloadRecipes();
        }

        private void AddRecipeTab_Clicked(object sender, EventArgs e)
        {
            _addRecipesTab.ReloadRecipes();
        }
    }
}
