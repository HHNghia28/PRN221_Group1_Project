using DataAccess.Entities;
using G1FOODLibraby.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountRepository _accountRepository;

        public AccountController() => _accountRepository = new AccountRepository();

        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _accountRepository.GetAllAccounts();
        }
    }
}
