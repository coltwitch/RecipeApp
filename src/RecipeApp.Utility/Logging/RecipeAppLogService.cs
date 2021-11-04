using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Utility.Configuration;
using Serilog;

namespace RecipeApp.Utility.Logging
{
    public class RecipeAppLogService
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var config = services.GetService<IRecipeAppConfig>();

            var filepath = config.LoggingConfig.LogFilePath;
            if (!string.IsNullOrEmpty(filepath))
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(filepath)
                    .CreateLogger();
            }
            services.AddLogging(configure => configure.AddSerilog());
        }
    }
}
