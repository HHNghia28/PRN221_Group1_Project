using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibraby.Repository
{
    public interface IAccountRepository
    {
        public IEnumerable<Account> GetAllAccounts();
    }
}
