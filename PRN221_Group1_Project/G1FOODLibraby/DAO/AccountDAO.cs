using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using G1FOODLibrary.Entities;
using System.Security.Authentication;
using G1FOODLibrary.DTO;

namespace DataAccess.DAO
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
            List<Account> accounts = _context.Accounts.Include(ac => ac.Users).ToList();

            return accounts;
        }

        public async Task<AccountDTO> ResgiterAsync(RegisterDTO register)
        {
            if (string.IsNullOrEmpty(register.Email) || string.IsNullOrEmpty(register.Password))
            {
                throw new ArgumentException("Email or Password can not be empty!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);

            Account account = new Account
            {
                Id = Guid.NewGuid(),
                Email = register.Email,
                EncryptedPassword = passwordHash,
                RoleId = new Guid("C73813A0-CE6E-4F59-B281-507690B51406"),
                StatusId = new Guid("750301CE-21B9-444E-A0D3-53824614CA40")
            };

            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            AccountDTO accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email
            };

            return accountDTO;
        }

        public async Task<AccountDTO> LoginAsync(LoginDTO login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password)) 
            {
                throw new ArgumentException("Email or Password can not be empty!");
            }

            Account account = null;
            try
            {
                account = await _context.Accounts
                    .Include(a => a.Role)
                    .Include(a => a.Status)
                    .SingleOrDefaultAsync(a => a.Email.ToLower() == login.Email.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            if (account == null)
            {
                throw new ArgumentException("Account not found!");
            }

            if (!BCrypt.Net.BCrypt.Verify(login.Password, account.EncryptedPassword))
            {
                throw new ArgumentException("Email or Password invalid!");
            }

            if (!account.Status.Id.ToString().ToUpper().Equals("750301CE-21B9-444E-A0D3-53824614CA40"))
            {
                throw new ArgumentException("Account is inactive!");
            }

            AccountDTO accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                Status = account.Status.Name,
                Role = account.Role.Name,
                RoleId = account.Role.Id
            };

            return accountDTO;
        }
    }
}
