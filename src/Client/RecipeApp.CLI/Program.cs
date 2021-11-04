using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Base;
using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.CLI.Console;
using RecipeApp.Resource;
using RecipeApp.Utility;
using System.IO;

namespace RecipeApp.CLI
{
    class Program
    {
        private static readonly string AppName = "RecipeApp.CLI";
        private static readonly int width = 100;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = new ConsoleApplication(AppName);
            app.RegisterManager(serviceProvider.GetService<IRecipeManager>())
                .RegisterMethodAsIdReader("recipeId", "GetRecipeById")
                .RegisterMethodAsIdSelector("recipeId", "Guid", "GetRecipes");
            app.RegisterManager(serviceProvider.GetService<IMealPlanManager>())
                .RegisterMethodAsIdReader("mealPlanId", "GetMealPlanById")
                .RegisterMethodAsIdSelector("mealPlanId", "Guid", "GetMealPlans");
            //var app = new Application(
            //    serviceProvider.GetService<IMealPlanManager>(),
            //    serviceProvider.GetService<IRecipeManager>(),
            //    serviceProvider.GetService<IConsoleUi>()
            //);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            RecipeAppUtilityService.ConfigureServices(services);
            RecipeAppResourceAccessService.ConfigureServices(services);
            RecipeAppBaseService.ConfigureServices(services);

            services.AddSingleton<IConsoleUi>(new ConsoleUi(AppName, width));
        }
    }
}
