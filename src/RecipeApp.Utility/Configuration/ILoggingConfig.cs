using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Utility.Configuration
{
    public interface ILoggingConfig
    {
        public string LogFilePath { get; set; }
    }
    public class LoggingConfig : ILoggingConfig
    {
        public string LogFilePath { get; set; }
    }
}
