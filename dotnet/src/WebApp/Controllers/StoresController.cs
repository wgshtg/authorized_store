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
        private readonly IStoreService _stores;

        public StoresController(IStoreService stores)
        {
            _stores = stores;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Store entity)
        {
            return Ok(await _stores.CreateAsync(entity));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _stores.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var store = await _stores.GetAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] SearchCriteria criteria)
        {
            var stores = await _stores.FindAllAsync(criteria);

            return Ok(stores);
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Store entity)
        {
            return Ok(await _stores.UpdateAsync(id, entity));
        }
    }
}
