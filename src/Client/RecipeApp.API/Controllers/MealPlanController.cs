using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeApp.API.Models;
using RecipeApp.Base.Interfaces.Managers;

namespace RecipeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanController : Controller
    {
        private readonly ILogger<MealPlanController> _logger;
        private IMealPlanManager _mealPlanManager;
        public MealPlanController(ILogger<MealPlanController> logger, IMealPlanManager mealPlanManager)
        {
            _logger = logger;
            _mealPlanManager = mealPlanManager;
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetMealPlanById([FromRoute] string id)
        {
            _logger.LogInformation($"GetMealPlanById : {id}");
            var result = _mealPlanManager.GetMealPlanById(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetMealPlans()
        {
            _logger.LogInformation($"Getting Meal Plans");
            var result = _mealPlanManager.GetMealPlans();
            return Ok(result);
        }

        [HttpPut]
        [Route("add")]
        public IActionResult AddMealPlan([FromBody] MealPlanRequest mealPlan)
        {
            _logger.LogInformation($"Adding meal plan");
            var result = _mealPlanManager.AddMealPlan(mealPlan);
            return Ok(result);
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateMealPlan([FromBody] MealPlanRequest mealPlan, [FromQuery] string id)
        {
            _logger.LogInformation($"Updating meal plan: {id}");
            var result = _mealPlanManager.UpdateMealPlan(mealPlan, id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteMealPlan([FromRoute] string id)
        {
            _logger.LogInformation($"Deleting meal plan: {id}");
            var result = _mealPlanManager.DeleteMealPlan(id);
            return Ok(result);
        }
    }
}