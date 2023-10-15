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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getAccount")]
        public IActionResult GetAccount(string id)
        {
            try
            {
                var accounts = _accountRepository.GetAccount(new Guid(id));
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get account successful!",
                    Data = accounts
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getUserByAccountId")]
        public async Task<IActionResult> GetUserByAccountId(string id)
        {
            try
            {
                var accounts = await _accountRepository.GetUsersByAccountId(new Guid(id));
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get user successful!",
                    Data = accounts
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(UserRequest userRequest)
        {
            try
            {
                await _accountRepository.AddUser(userRequest);
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add user successful!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("addAccount")]
        public async Task<IActionResult> AddAccount(AccountRequest userRequest)
        {
            try
            {
                await _accountRepository.AddAccountAsync(userRequest);
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add account successful!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            try
            {
                await _accountRepository.DeleteAccount(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Delete account successful!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] string password)
        {
            try
            {
                await _accountRepository.UpdatePassword(new Guid(id), password);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update password successful!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var accounts = await _accountRepository.GetRolesAsync();
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get all account role successful!",
                    Data = accounts
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getAccountStatus")]
        public async Task<IActionResult> GetAccountStatus()
        {
            try
            {
                var accounts = await _accountRepository.GetAccountStatusAsync();
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get all account status successful!",
                    Data = accounts
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("updateAccount")]
        public async Task<IActionResult> UpdateAccount(string id, AccountUpdateRequest account)
        {
            try
            {
                await _accountRepository.UpdateAccountAsync(account, new Guid(id));
                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update account successful!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
