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

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Category category)
        {
            var entity = await _categoryService.CreateAsync(category);

            return Ok(entity);
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Category category)
        {
            var entity = await _categoryService.UpdateAsync(id, category);

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok();
        }
    }
}
