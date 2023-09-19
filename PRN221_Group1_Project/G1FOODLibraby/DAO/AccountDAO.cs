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

        public IEnumerable<AccountDTO> GetAllAccounts()
        {
            List<AccountDTO> accountDTOs = new List<AccountDTO>();
            try
            {
                var accounts = _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Status)
                .Include(ac => ac.Users)
                .Where(ac => ac.Users.Any(user => user.DefaultUser == true))
                .ToList();

                foreach (var account in accounts)
                {
                    accountDTOs.Add(new AccountDTO
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Name = account.Users.First().Name,
                        Phone = account.Users.First().Phone,
                        AddressDetail = account.Users.First().AddressDetail,
                        Role = account.Role.Name,
                        Status = account.Status.Name
                    });
                }
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            return accountDTOs;
        }

        public async Task<AccountDTO> ResgiterAsync(RegisterDTO register)
        {
            if (string.IsNullOrEmpty(register.Email) || string.IsNullOrEmpty(register.Password))
            {
                throw new ArgumentException("Email or Password can not be empty!");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);

            Guid newId = Guid.NewGuid();

            Account account = new Account
            {
                Id = newId,
                Email = register.Email,
                EncryptedPassword = passwordHash,
                RoleId = new Guid("C73813A0-CE6E-4F59-B281-507690B51406"),
                StatusId = new Guid("2BB38E30-BCAC-45C4-A05E-09BF7B1BCC9E")
            };

            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = register.Name,
                Phone = register.Phone,
                DefaultUser = true,
                AccountId = newId,
                AddressDetail = register.AddressDetail,
                StatusId = new Guid("750301CE-21B9-444E-A0D3-53824614CA40")
            };

            try
            {
                _context.Accounts.Add(account);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            AccountDTO accountDTO = new AccountDTO
            {
                Id = account.Id,
                Email = account.Email,
                Name = user.Name,
                Phone = user.Phone,
                AddressDetail = user.AddressDetail
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

        public async Task<bool> ActiveAccountAsync(string email)
        {
            bool active = true;

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email can not be empty!");
            }

            Account account = null;
            try
            {
                account = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());

                account.StatusId = new Guid("750301CE-21B9-444E-A0D3-53824614CA40");

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            if (account == null)
            {
                throw new ArgumentException("Account not found!");
            }

            return active;
        }
    }
}
