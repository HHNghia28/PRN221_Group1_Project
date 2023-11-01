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
using Microsoft.Identity.Client;
using Microsoft.Win32;
using System.Security.Principal;

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

        public IEnumerable<AccountResponse> GetAllAccounts()
        {
            List<AccountResponse> accountDTOs = new List<AccountResponse>();
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
                    accountDTOs.Add(new AccountResponse
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return accountDTOs;
        }

        public AccountResponse GetAccount(Guid id)
        {
            try
            {
                var account = _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Status)
                .Include(ac => ac.Users)
                .FirstOrDefault(a => a.Id == id);

                return new AccountResponse
                {
                    Id = account.Id,
                    Email = account.Email,
                    Name = account.Users.First().Name,
                    Phone = account.Users.First().Phone,
                    AddressDetail = account.Users.First().AddressDetail,
                    Role = account.Role.Name,
                    Status = account.Status.Name,
                    StatusId = account.StatusId,
                    RoleId = account.RoleId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AccountResponse> ResgiterAsync(RegisterRequest register)
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
                StatusId = new Guid("750301CE-21B9-444E-A0D3-53824614CA40")
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
                throw new Exception(ex.Message);
            }

            AccountResponse accountDTO = new AccountResponse
            {
                Id = account.Id,
                Email = account.Email,
                Name = user.Name,
                Phone = user.Phone,
                AddressDetail = user.AddressDetail,
                RoleId = account.RoleId
            };

            return accountDTO;
        }

        public async Task<AccountResponse> LoginAsync(LoginRequest login)
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
                    .Include(ac => ac.Users)
                    .FirstOrDefaultAsync(a => a.Email.ToLower() == login.Email.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

            AccountResponse accountDTO = new AccountResponse
            {
                Id = account.Id,
                Email = account.Email,
                Name = account.Users.First().Name,
                Phone = account.Users.First().Phone,
                AddressDetail = account.Users.First().AddressDetail,
                Role = account.Role.Name,
                Status = account.Status.Name,
                RoleId = account.RoleId
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
                    .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());

                account.StatusId = new Guid("750301CE-21B9-444E-A0D3-53824614CA40");

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (account == null)
            {
                throw new ArgumentException("Account not found!");
            }

            return active;
        }

        public async Task<IEnumerable<UserResponse>> GetUsersByAccountId(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentException("Account ID can not be empty!");
            }

            try
            {
                var users = _context.Users.Where(u => u.AccountId == accountId).Include(a => a.Status).ToList();

                List<UserResponse> userResponses = new List<UserResponse>();

                foreach (var item in users)
                {
                    userResponses.Add(new UserResponse
                    {
                        Id = item.Id,
                        AddressDetail = item.AddressDetail,
                        Name = item.Name,
                        Phone = item.Phone
                    });
                }

                return userResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddUser(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentException("User can not be empty!");
            }

            try
            {
                User user = new User
                {
                    Id = new Guid(),
                    AccountId = userRequest.AccountId,
                    Name = userRequest.Name,
                    AddressDetail = userRequest.AddressDetail,
                    Phone = userRequest.Phone
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAccount(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Account ID can not be empty!");
            }

            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Id == id);

                if (account == null)
                {
                    throw new Exception("Account not exist!");
                }

                account.StatusId = new Guid("FF07CAA9-3D82-41CA-8DAD-643A97455590");

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdatePassword(Guid id, string password)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Account ID can not be empty!");
            }

            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.Id == id);

                if (account == null)
                {
                    throw new Exception("Account not exist!");
                }

                account.EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAccountAsync(AccountRequest register)
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
                RoleId = register.RoleId,
                StatusId = register.StatusId
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StatusResponse>> GetAccountStatusAsync()
        {
            try
            {
                var status = await _context.AccountStatuses.ToListAsync();

                List<StatusResponse> result = new List<StatusResponse>();

                foreach (var item in status)
                {
                    result.Add(new StatusResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<StatusResponse>> GetRolesAsync()
        {
            try
            {
                var status = await _context.Roles.ToListAsync();

                List<StatusResponse> result = new List<StatusResponse>();

                foreach (var item in status)
                {
                    result.Add(new StatusResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAccountAsync(AccountUpdateRequest account, Guid id)
        {
            if(account == null) throw new ArgumentNullException(nameof(account));
            if(id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            try
            {
                var existAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
                if (existAccount == null)
                {
                    throw new Exception("Account not found!");
                }

                existAccount.StatusId = account.StatusId;
                existAccount.RoleId = account.RoleId;
                
                var existUser = await _context.Users.FirstOrDefaultAsync(y => y.AccountId == id);

                if (existUser == null)
                {
                    throw new Exception("Account not found!");
                }

                existUser.Name = account.Name;
                existUser.AddressDetail = account.AddressDetail;
                existUser.Phone = account.Phone;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
