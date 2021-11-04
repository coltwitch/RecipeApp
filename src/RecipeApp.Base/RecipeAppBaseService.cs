using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Base.Interfaces.Managers;
using RecipeApp.Base.Managers;
using RecipeApp.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base
{
    public class RecipeAppBaseService
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MealPlanManager>();
            services.AddTransient<RecipeManager>();

            services.AddScoped<IMealPlanManager, MealPlanManager>();
            services.AddScoped<IRecipeManager, RecipeManager>();
        }
    }
}
