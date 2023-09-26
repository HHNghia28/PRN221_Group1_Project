using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IAccountRepository
    {
        public IEnumerable<AccountResponse> GetAllAccounts();
        public Task<AccountResponse> RegisterAsync(RegisterRequest register);
        public Task<AccountResponse> LoginAsync(LoginRequest login);
        public Task<bool> ActiveAccountAsync(string email);
    }
}
