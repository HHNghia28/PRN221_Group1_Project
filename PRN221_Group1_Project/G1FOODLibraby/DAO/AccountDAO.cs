using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1FOODLibraby.Context;
using DataAccess.Entities;

namespace G1FOODLibraby.DAO
{
    internal class AccountDAO
    {
        private DBContext _context;
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();

        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }

        public AccountDAO() => _context = new DBContext();

        public IEnumerable<Account> GetAllAccounts()
        {
            List<Account> accounts = _context.Accounts.ToList();

            return accounts;
        }
    }
}
