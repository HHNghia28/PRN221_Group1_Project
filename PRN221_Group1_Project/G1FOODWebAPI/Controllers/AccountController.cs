using G1FOODLibrary.Entities;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using G1FOODLibrary.DTO;
using Microsoft.Win32;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountRepository _accountRepository;

        public AccountController() => _accountRepository = new AccountRepository();

        // GET: api/<AccountController>
        [HttpGet("getAllAccounts")]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get all accounts successful!",
                    Data = accounts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
