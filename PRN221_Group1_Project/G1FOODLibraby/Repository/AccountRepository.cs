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

        public IEnumerable<Account> GetAllAccounts() => AccountDAO.Instance.GetAllAccounts();

        public Task<AccountDTO> LoginAsync(LoginDTO login) => AccountDAO.Instance.LoginAsync(login);

        public Task<AccountDTO> RegisterAsync(RegisterDTO register) => AccountDAO.Instance.ResgiterAsync(register);
    }
}
