using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Update;
using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiredScope("categories.read")]
    public class CategoryController : ControllerBase
    {
        public CategoryController(ILogger<CategoryController> logger,
              ICategoryService categoryService)
        {
            Logger = logger;
            CategoryService = categoryService;
        }

        public ILogger<CategoryController> Logger { get; }
        public ICategoryService CategoryService { get; }

        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCategoryAsync([FromRoute] short id)
        {
            Logger.LogInformation($"Executing {nameof(GetCategoryAsync)}");

            var category = await CategoryService.GetCategoryAsync(id);

            if (category == null)
                return NotFound();

            var categoryViewModel = new CategoryViewModel() { Id = category.Id, IsActive = category.IsActive, Name = category.Name };


            return Ok(categoryViewModel);
        }

        [HttpGet("All", Name = "GetAllCategory")]
        [ProducesResponseType(typeof(List<CategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            Logger.LogInformation($"Executing {nameof(GetCategoryAsync)}");

            var categories = await CategoryService.GetCategoriesAsync();

            var categoriesModel = categories.Select(s => new CategoryViewModel() { Id = s.Id, IsActive = s.IsActive, Name = s.Name }).ToList();

            return Ok(categoriesModel);

        }


        [HttpPost("", Name = "PostCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostCategoryAsync([FromBody] CreateCategory createCategory)
        {
            Logger.LogInformation($"Executing {nameof(PostCategoryAsync)}");

            var entity = new Category() { IsActive = createCategory.IsActive, Name = createCategory.Name };

            var isSuccess = await CategoryService.CreateCategoryAsync(entity);

            return new CreatedAtRouteResult("GetCategory",
                   new { id = entity.Id });
        }


        [HttpPut("", Name = "PutCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutCategoryAsync([FromBody] UpdateCategory updateCategory)
        {
            Logger.LogInformation($"Executing {nameof(PutCategoryAsync)}");

            var entity = new Category() { Id = updateCategory.Id, IsActive = updateCategory.IsActive, Name = updateCategory.Name };

            await CategoryService.UpdateCategoryAsync(entity);

            return Ok();
        }


        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] short id)
        {
            Logger.LogInformation($"Executing {nameof(DeleteCategoryAsync)}");

            var category = await CategoryService.GetCategoryAsync(id);

            if (category == null)
                return NotFound();

            await CategoryService.DeleteCategoryAsync(id);

            return Ok();
        }

    }
}
