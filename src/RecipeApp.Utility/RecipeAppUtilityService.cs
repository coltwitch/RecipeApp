using Microsoft.Extensions.DependencyInjection;
using RecipeApp.Utility.Configuration;
using RecipeApp.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Utility
{
    public class RecipeAppUtilityService
    {
        public static void ConfigureServices(IServiceCollection services)        
        {
            RecipeAppConfigurationService.ConfigureServices(services);
            RecipeAppLogService.ConfigureServices(services);
        }
    }
}
