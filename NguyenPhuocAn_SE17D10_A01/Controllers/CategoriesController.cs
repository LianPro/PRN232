using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Services;

namespace NguyenPhuocAn_SE17D10_A01.Controllers
{
    [Route("odata/Categories")]
    public class CategoriesController : ODataController
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _categoryService.AddAsync(category);
                return Created(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != category.CategoryID)
                return BadRequest("Category ID mismatch.");

            try
            {
                await _categoryService.UpdateAsync(category);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid category ID.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
