using System.Threading.Tasks;
using AuthorizedStore;
using AuthorizedStore.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
            => _categoryService = categoryService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            var category = await _categoryService.GetAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] CategoryCriteria criteria)
        {
            var categories = await _categoryService.GetListAsync(criteria);

            return Ok(categories);
        }
    }
}
