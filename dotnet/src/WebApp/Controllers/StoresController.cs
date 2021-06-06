using System.Threading.Tasks;
using AuthorizedStore;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly IStoreDao _storeDao;

        public StoresController(IStoreDao storeDao)
        {
            _storeDao = storeDao;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var store = await _storeDao.GetAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] SearchCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var stores = await _storeDao.FindAllAsync(criteria);

            return Ok(stores);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Store entity)
        {
            if (entity == null
                || string.IsNullOrWhiteSpace(entity.Name)
                || string.IsNullOrWhiteSpace(entity.ContractContent))
            {
                return BadRequest();
            }

            return Ok(await _storeDao.CreateAsync(entity));
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Store entity)
        {
            if (entity == null
                || string.IsNullOrWhiteSpace(entity.Name)
                || string.IsNullOrWhiteSpace(entity.ContractContent))
            {
                return BadRequest();
            }

            var store = await _storeDao.GetAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            return Ok(await _storeDao.UpdateAsync(id, entity));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var store = await _storeDao.GetAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            await _storeDao.DeleteAsync(id);
            return Ok();
        }
    }
}
