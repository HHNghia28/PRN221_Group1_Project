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
        public IEnumerable<AccountDTO> GetAllAccounts();
        public Task<AccountDTO> RegisterAsync(RegisterDTO register);
        public Task<AccountDTO> LoginAsync(LoginDTO login);
        public Task<bool> ActiveAccountAsync(string email);
    }
}
