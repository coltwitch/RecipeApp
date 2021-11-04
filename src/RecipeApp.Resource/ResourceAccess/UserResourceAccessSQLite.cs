using RecipeApp.Base.Interfaces.ResourceAccess;
using RecipeApp.Resource.ResourceAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Resource.ResourceAccess
{
    public class UserResourceAccessSQLite : IUserResourceAccess
    {
        private readonly RecipeAppSqliteContext context;

        public UserResourceAccessSQLite(RecipeAppSqliteContext con)
        {
            context = con;
        }
    }
}
