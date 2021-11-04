using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Base.Interfaces.ResourceAccess;
using RecipeApp.Resource.ResourceAccess;
using RecipeApp.Resource.ResourceAccess.Repositories;
using RecipeApp.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Resource
{
    public class RecipeAppResourceAccessService
    {
        private bool sqlite;
        private RecipeAppResourceAccessService(IRecipeAppConfig config)
        {
            if (config.DatabaseConfig.DbType.ToLower() == "sqlite")
            {
                sqlite = true;
            }
            else
            {
                //default
                sqlite = true;
            }
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            if (sqlite)
            {
                services.AddDbContext<RecipeAppSqliteContext>();
            }
        }

        private void ConfigureMealPlanResourceAccess(IServiceCollection services)
        {
            if (sqlite)
            {
                services.AddScoped<IMealPlanResourceAccess, MealPlanResourceAccessSQLite>();
            }
        }

        private void ConfigureRecipeResourceAccess(IServiceCollection services)
        {
            if (sqlite)
            {
                services.AddScoped<IRecipeResourceAccess, RecipeResourceAccessSQLite>();
            }
        }

        private void ConfigureUserResourceAccess(IServiceCollection services)
        {
            if (sqlite)
            {
                services.AddScoped<IUserResourceAccess, UserResourceAccessSQLite>();
            }
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var config = services.GetService<IRecipeAppConfig>();
            var factory = new RecipeAppResourceAccessService(config);
            factory.ConfigureDbContext(services);
            factory.ConfigureRecipeResourceAccess(services);
            factory.ConfigureMealPlanResourceAccess(services);
            factory.ConfigureUserResourceAccess(services);
        }
    }
}
