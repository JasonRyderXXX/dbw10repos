using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rydersoft.POS.API.Controllers
{
    [ApiController]
    public class Inventory : ControllerBase
    {

        [Route("api/[controller]/Item/{Sku}")]
        public async Task<IActionResult> Item(string sku)
        {

            return NotFound();

        }
    }
}
