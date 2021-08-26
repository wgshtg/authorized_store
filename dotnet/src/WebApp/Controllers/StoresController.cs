using System.Threading.Tasks;
using AuthorizedStore;
using AuthorizedStore.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Store entity)
        {
            return Ok(await _storeService.CreateAsync(entity));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _storeService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var store = await _storeService.GetAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] StoreCriteria criteria)
        {
            var stores = await _storeService.GetListAsync(criteria);

            return Ok(stores);
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Store entity)
        {
            return Ok(await _storeService.UpdateAsync(id, entity));
        }
    }
}
