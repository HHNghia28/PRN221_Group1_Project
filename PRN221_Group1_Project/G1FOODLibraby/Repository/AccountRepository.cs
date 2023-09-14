using DataAccess.Entities;
using G1FOODLibraby.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibraby.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public IEnumerable<Account> GetAllAccounts() => AccountDAO.Instance.GetAllAccounts();
    }
}
