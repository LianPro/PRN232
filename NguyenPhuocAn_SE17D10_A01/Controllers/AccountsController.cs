using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Services;

namespace NguyenPhuocAn_SE17D10_A01.Controllers
{
    [Route("odata/Accounts")]
    public class AccountsController : ODataController
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);
                return Ok(account);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _accountService.AddAsync(account);
                return Created(account);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != account.AccountID)
                return BadRequest("Account ID mismatch.");

            try
            {
                await _accountService.UpdateAsync(account);
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
                await _accountService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid account ID.");
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
