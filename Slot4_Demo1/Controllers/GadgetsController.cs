using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Slot4_Demo1.Controllers
{
    [Route("odata/[controller]")]
    public class GadgetsController : ODataController
    {
        private readonly MyWorldDbContext _context;

        public GadgetsController(MyWorldDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Gadgets);
        }
    }
}
