using Azure.Core;
using DataAccess.Repository;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAccountRepository _accountRepository;

        public AuthController() => _accountRepository = new AccountRepository();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            AccountDTO account = new AccountDTO();

            try
            {

                account = await _accountRepository.RegisterAsync(register);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponseDTO
            {
                StatusCode = 200,
                Success = true,
                Message = "Register successful!",
                Data = account
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            AccountDTO account = new AccountDTO();
            try
            {

                account = await _accountRepository.LoginAsync(login);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            account.Token = CreateToken(account);

            SendEmail(account.Email);

            return Ok(new APIResponseDTO
            {
                StatusCode = 200,
                Success = true,
                Message = "Login successful!",
                Data = account
            });
        }

        private string CreateToken(AccountDTO account)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Email", account.Email),
                new Claim("RoleId", account.RoleId.ToString()),
                new Claim("TokenId", Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: GetIssuer(),
                audience: GetAudience(),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        private string GetSecretKey()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var str = config["AppSettings:SecretKey"];
            return str;
        }

        private string GetIssuer()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var str = config["AppSettings:Issuer"];
            return str;
        }

        private string GetAudience()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var str = config["AppSettings:Audience"];
            return str;
        }

        private void SendEmail(string s)
        {
            string fromMail = "g1food.fpt@gmail.com";
            string fromPassword = "gdfgxbbgtvehrcgg";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Test Subject";
            message.To.Add(new MailAddress("nghiahh.work@gmail.com"));
            message.Body = "<html><body> " + s + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}
