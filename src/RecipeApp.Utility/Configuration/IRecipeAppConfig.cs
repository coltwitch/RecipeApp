using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Utility.Configuration
{
    public interface IRecipeAppConfig
    {
        public IDatabaseConfig DatabaseConfig { get; set; }
        public ILoggingConfig LoggingConfig { get; set; }
    }
    public class RecipeAppConfig : IRecipeAppConfig
    {
        public IDatabaseConfig DatabaseConfig { get; set; }
        public ILoggingConfig LoggingConfig { get; set; }

        public static RecipeAppConfig FromConfig(IConfiguration config)
        {
            var recipeAppConfig = new RecipeAppConfig();
            
            var dbConfigSection = config.GetSection("DatabaseConfig");
            var databaseConfig = new DatabaseConfig
            {
                DbPath = dbConfigSection["dbPath"],
                DbType = dbConfigSection["dbType"]
            };
            var logConfigSection = config.GetSection("LoggingConfig");
            var loggingConfig = new LoggingConfig
            {
                LogFilePath = logConfigSection["logFilePath"]
            };
            recipeAppConfig.DatabaseConfig = databaseConfig;
            recipeAppConfig.LoggingConfig = loggingConfig;

            return recipeAppConfig;
        }
    }
}
