using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Utility.Configuration
{
    public interface IDatabaseConfig
    {
        public string DbType { get; set; }
        public string DbPath { get; set; }
    }

    public class DatabaseConfig : IDatabaseConfig
    {
        public string DbType { get; set; }
        public string DbPath { get; set; }
    }
}
