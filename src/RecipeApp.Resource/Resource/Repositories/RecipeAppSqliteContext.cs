using Microsoft.EntityFrameworkCore;
using RecipeApp.Resource.Models;
using RecipeApp.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Resource.ResourceAccess.Repositories
{
    public class RecipeAppSqliteContext : DbContext
    {
        public string DatabasePath { get; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<MealPlanRecipe> MealPlanRecipes { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<User> Users { get; set; }
        
        private static bool _created = false;
        public RecipeAppSqliteContext(IRecipeAppConfig recipeAppConfig)
        {
            DatabasePath = recipeAppConfig.DatabaseConfig.DbPath;
            if (!_created)
            {
                _created = true;
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public Recipe GetRecipe(string id)
        {
            var recipe = Recipes.FirstOrDefault(x => x.Guid == id);
            if (recipe != null)
            {
                recipe.Ingredients = recipe.GetIngredients(this);
                recipe.Instructions = recipe.GetInstructions(this);
            }
            return recipe;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePath};");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().ToTable("Recipes").HasKey(r => r.Guid);
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients").HasKey(i => i.Guid);
            modelBuilder.Entity<Instruction>().ToTable("Instructions").HasKey(i => i.Guid);
            modelBuilder.Entity<MealPlan>().ToTable("MealPlans").HasKey(mp => mp.Guid);
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Guid);
            modelBuilder.Entity<ShoppingListItem>().ToTable("ShoppingListItems").HasKey(sli => sli.ItemGuid);
            modelBuilder.Entity<MealPlanRecipe>().ToTable("MealPlanRecipes").HasKey(mpr => mpr.Guid);
        }
    }
}