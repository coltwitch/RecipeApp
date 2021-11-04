using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Utility.Configuration
{
    public static class RecipeAppConfigurationService
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var config = services.GetService<IConfiguration>();
            var recipeAppConfig = RecipeAppConfig.FromConfig(config);
            services.AddSingleton<IRecipeAppConfig>(recipeAppConfig);
        }

        public static T GetService<T>(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<T>();
        }
    }
}
