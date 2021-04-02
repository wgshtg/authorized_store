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
        public async Task<IActionResult> GetStoresAsync([FromQuery]SearchCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }

            var stores = await _storeDao.FindAllAsync(criteria);

            return Ok(stores);
        }
    }
}
