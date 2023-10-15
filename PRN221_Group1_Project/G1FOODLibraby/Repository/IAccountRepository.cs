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
        public Task AddUser(UserRequest userRequest);
        public Task<IEnumerable<UserResponse>> GetUsersByAccountId(Guid accountId);
        public Task UpdatePassword(Guid id, string password);
        public Task DeleteAccount(Guid id);
        public AccountResponse GetAccount(Guid id);
        public Task AddAccountAsync(AccountRequest register);
        public Task<IEnumerable<StatusResponse>> GetAccountStatusAsync();
        public Task<IEnumerable<StatusResponse>> GetRolesAsync();
        public Task UpdateAccountAsync(AccountUpdateRequest account, Guid id);
    }
}
