using LearnSmartCoding.EssentialProducts.API.ViewModel.Create;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Get;
using LearnSmartCoding.EssentialProducts.API.ViewModel.Update;
using LearnSmartCoding.EssentialProducts.Core;
using LearnSmartCoding.EssentialProducts.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnSmartCoding.EssentialProducts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public CategoryController(ILogger<CategoryController> logger, IMemoryCache memoryCache,
              ICategoryService categoryService)
        {
            Logger = logger;
            MemoryCache = memoryCache;
            CategoryService = categoryService;
        }

        public ILogger<CategoryController> Logger { get; }
        public IMemoryCache MemoryCache { get; }
        public ICategoryService CategoryService { get; }

        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
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
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var keyToSave = "categories";

            Logger.LogInformation($"Executing {nameof(GetCategoryAsync)}");

            var categories = await CategoryService.GetCategorysAsync();

            var categoriesModel = categories.Select(s => new CategoryViewModel() { Id = s.Id, IsActive = s.IsActive, Name = s.Name }).ToList();

            return Ok(categoriesModel);

        }


        [HttpPost("", Name = "PostCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCategoryAsync([FromBody] CreateCategory createCategory)
        {
            Logger.LogInformation($"Executing {nameof(PostCategoryAsync)}");

            var entity = new Category() { IsActive = createCategory.IsActive, Name = createCategory.Name };

            var isSuccess = await CategoryService.CreateCategoryAsync(entity);

            return new CreatedAtRouteResult("GetCategory",
                   new { id = entity.Id });
        }


        [HttpPut("{id}", Name = "PutCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCategoryAsync([FromRoute] short id, [FromBody] UpdateCategory updateCategory)
        {
            //TODO: add valdation to check id in route and id from model are same. if not return 400 bad request

            Logger.LogInformation($"Executing {nameof(PutCategoryAsync)}");
            var category = await CategoryService.GetCategoryAsync(id);

            if (category == null)
                return NotFound();

            category.IsActive = updateCategory.IsActive;
            category.Name = updateCategory.Name;

            await CategoryService.UpdateCategoryAsync(category);

            return Ok();
        }


        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
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
