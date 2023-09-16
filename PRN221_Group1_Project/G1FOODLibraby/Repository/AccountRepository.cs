using G1FOODLibrary.Entities;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public IEnumerable<Account> GetAllAccounts() => AccountDAO.Instance.GetAllAccounts();
    }
}
