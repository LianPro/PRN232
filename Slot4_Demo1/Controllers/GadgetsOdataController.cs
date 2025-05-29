using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Slot4_Demo1;
using System.Linq;

public class GadgetsOdataController : ControllerBase
{
    private readonly MyWorldDbContext _context;

    public GadgetsOdataController(MyWorldDbContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        var gadgets = _context.Gadgets.AsQueryable();
        return Ok(gadgets);
    }
}
