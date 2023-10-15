using G1FOODLibrary.Entities;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1FOODLibrary.DTO;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public Task<bool> ActiveAccountAsync(string email) => AccountDAO.Instance.ActiveAccountAsync(email);

        public Task AddAccountAsync(AccountRequest register) => AccountDAO.Instance.AddAccountAsync(register);

        public Task AddUser(UserRequest userRequest) => AccountDAO.Instance.AddUser(userRequest);

        public Task DeleteAccount(Guid id) => AccountDAO.Instance.DeleteAccount(id);

        public AccountResponse GetAccount(Guid id) => AccountDAO.Instance.GetAccount(id);

        public Task<IEnumerable<StatusResponse>> GetAccountStatusAsync() => AccountDAO.Instance.GetAccountStatusAsync();

        public IEnumerable<AccountResponse> GetAllAccounts() => AccountDAO.Instance.GetAllAccounts();

        public Task<IEnumerable<StatusResponse>> GetRolesAsync() => AccountDAO.Instance.GetRolesAsync();

        public Task<IEnumerable<UserResponse>> GetUsersByAccountId(Guid accountId) => AccountDAO.Instance.GetUsersByAccountId(accountId);

        public Task<AccountResponse> LoginAsync(LoginRequest login) => AccountDAO.Instance.LoginAsync(login);

        public Task<AccountResponse> RegisterAsync(RegisterRequest register) => AccountDAO.Instance.ResgiterAsync(register);

        public Task UpdateAccountAsync(AccountUpdateRequest account, Guid id) => AccountDAO.Instance.UpdateAccountAsync(account, id);

        public Task UpdatePassword(Guid id, string password) => AccountDAO.Instance.UpdatePassword(id, password);
    }
}
