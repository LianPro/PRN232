using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Services;

namespace NguyenPhuocAn_SE17D10_A01.Controllers
{
    [Route("odata/Tags")]
    public class TagsController : ODataController
    {
        private readonly TagService _tagService;

        public TagsController(TagService tagService)
        {
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var tag = await _tagService.GetByIdAsync(id);
                return Ok(tag);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _tagService.AddAsync(tag);
                return Created(tag);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != tag.TagID)
                return BadRequest("Tag ID mismatch.");

            try
            {
                await _tagService.UpdateAsync(tag);
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
                await _tagService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid tag ID.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
