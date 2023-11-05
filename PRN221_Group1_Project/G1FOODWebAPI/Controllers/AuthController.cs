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
using System.Runtime;
using Newtonsoft.Json.Linq;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAccountRepository _accountRepository;

        public AuthController() => _accountRepository = new AccountRepository();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            AccountResponse account = new AccountResponse();

            try
            {

                account = await _accountRepository.RegisterAsync(register);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Register successful!",
                Data = account
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            AccountResponse account = new AccountResponse();
            try
            {

                account = await _accountRepository.LoginAsync(login);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            account.Token = CreateToken(account, 960);

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Login successful!",
                Data = account
            });
        }

        [HttpPost("sendMailAuthentication")]
        public async Task<IActionResult> SendMailAuthentication([FromBody] string email)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            string token = "";
            try
            {
                AccountResponse account = new AccountResponse
                {
                    Email = email,
                    RoleId = new Guid("C73813A0-CE6E-4F59-B281-507690B51406")
                };

                Random rand = new Random();
                StringBuilder numericToken = new StringBuilder();

                for (int i = 0; i < 6; i++)
                {
                    numericToken.Append(rand.Next(0, 10)); // Generates a random digit between 0 and 9.
                }

                token = numericToken.ToString(); // 8 is the token length.
                SendEmail(token, email);
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = token.ToString(),
                    Message = ex.Message
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Data = token.ToString(),
                Message = "Send mail successful!"
            });
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email)
        {

            if (IsTokenValid(token))
            {
                bool active;
                try
                {
                    active = await _accountRepository.ActiveAccountAsync(email);
                }
                catch (Exception ex)
                {
                    return Ok(new APIResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = ex.Message
                    });
                }

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Authentication successful!"
                });
            }
            else
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = true,
                    Message = "Authentication fail!"
                });
            }
        }

        private string CreateToken(AccountResponse account, int time)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Email", account.Email),
                new Claim(ClaimTypes.Role, account.RoleId.ToString()),
                new Claim("TokenId", Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: GetIssuer(),
                audience: GetAudience(),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(time),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        private bool IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey()));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true, 
                ValidIssuer = GetIssuer(),
                ValidateAudience = true, 
                ValidAudience = GetAudience(),
                ValidateLifetime = true, 
                ClockSkew = TimeSpan.Zero 
            };

            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false; 
            }

            return true;
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

        private void SendEmail(string s, string email)
        {
            string fromMail = "g1food.fpt@gmail.com";
            string fromPassword = "gdfgxbbgtvehrcgg";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "[G1Food xác thực email]";
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " +
                        "<p>Mã xác thực email của bạn là:  <span style={color:red;font-weight:bold}>" + s + "</span></p>" +
                        "<p>Trân trọng</p>" +
                        "<p>G1Food</p>" +
                " </body></html>";
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
