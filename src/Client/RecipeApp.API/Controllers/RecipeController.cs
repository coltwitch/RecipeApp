using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApp.API.Models;
using RecipeApp.Base.Interfaces.Managers;

namespace RecipeApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private IRecipeManager _recipeManager;
        public RecipeController(ILogger<RecipeController> logger, IRecipeManager recipeManager)
        {
            _logger = logger;
            _recipeManager = recipeManager;
        }

        [HttpGet]
        [Route("get/name/{name}")]
        public IActionResult GetRecipeByName([FromRoute] string name)
        {
            _logger.LogInformation($"Get recipe by name: {name}");
            var result = _recipeManager.GetRecipeByName(name);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetRecipeById([FromRoute] string id)
        {
            _logger.LogInformation($"Get recipe by id: {id}");
            var result = _recipeManager.GetRecipeById(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetRecipes()
        {
            _logger.LogInformation($"Getting Recipes");
            var result = _recipeManager.GetRecipes();
            return Ok(result);
        }


        [HttpPut]
        [Route("add")]
        public IActionResult AddRecipe([FromBody] RecipeRequest recipe)
        {
            _logger.LogInformation($"Adding recipe");
            var result = _recipeManager.AddRecipe(recipe);
            return Ok(result);
        }



        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateRecipe([FromBody] RecipeRequest recipe, [FromQuery] string id)
        {
            _logger.LogInformation($"Updating meal plan: {id}");
            var result = _recipeManager.UpdateRecipe(id, recipe);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteRecipe([FromRoute] string id)
        {
            _logger.LogInformation($"Deleting meal plan: {id}");
            var result = _recipeManager.DeleteRecipe(id);
            return Ok(result);
        }
    }
}
