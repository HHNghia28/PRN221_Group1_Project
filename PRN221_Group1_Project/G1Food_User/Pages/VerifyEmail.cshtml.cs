using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;

namespace G1Food_User.Pages
{
    public class VerifyEmailModel : PageModel
    {
        private readonly ILogger<VerifyEmailModel> _logger;
        private readonly HttpClient _client;
        private readonly string _authApiUrl;
        
        public AccountRequest accountRequest { get; set; }

        [BindProperty]
        public string enteredTooken { get; set; }

        [BindProperty]
        public bool isWrongOTP { get; set; } = false;

        public string token { get; set; }
        public AccountResponse Account { get; private set; }

        [BindProperty]
        public string ResponseMessage { get; set; }

        public VerifyEmailModel(ILogger<VerifyEmailModel> logger, IConfiguration configuration)
        {
            token = "";
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _authApiUrl = configuration.GetValue<string>("APIEndpoint:Auth");
        }

        public async Task OnGet()
        {
            string email = Request.Query["email"];
            string phone = Request.Query["phone"];
            string name = Request.Query["name"];
            string address = Request.Query["address"];
            string password = Request.Query["password"];


            Response.Cookies.Append("isRegistered", "isRegistered");
            Response.Cookies.Append("email", email);
            Response.Cookies.Append("phone", phone);
            Response.Cookies.Append("name", name);
            Response.Cookies.Append("address", address);
            Response.Cookies.Append("password", password);

            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync($"{_authApiUrl}sendMailAuthentication", email);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Extract the substring from the end to the specified index.
                int startIndex = stringData.LastIndexOf(':') + 2;
                token = stringData.Substring(startIndex, 6);
                Response.Cookies.Append("Token", token.ToString());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request failed with error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            string email = Request.Cookies["email"];
            string phone = Request.Cookies["phone"];
            string name = Request.Cookies["name"];
            string address = Request.Cookies["address"];
            string password = Request.Cookies["password"];

            token = Request.Cookies["Token"];
           
            if (token == enteredTooken)
            {
                try
                {
                    accountRequest = new AccountRequest
                    {
                        Email = email,
                        Password = password,
                        Name = name,
                        AddressDetail = address,
                        Phone = phone,
                    };
                    HttpResponseMessage response = await _client.PostAsJsonAsync($"{_authApiUrl}register", accountRequest);
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
                    if (apiResponse.Success)
                    {
                        ResponseMessage = "Register successfully!";
                        Account = JsonSerializer.Deserialize<AccountResponse>(apiResponse.Data.ToString(), options);
                        Account.Token = token;
                        var claims = new List<Claim>
                                {
                                    new Claim("ID", Account.Id.ToString()),
                                    new Claim("Name", Account.Name),
                                    new Claim("Email",Account.Email),
                                    new Claim("Phone", Account.Phone),
                                    new Claim("Address", Account.AddressDetail),
                                    new Claim("Role", Account.RoleId.ToString()),
                                    new Claim("Token", Account.Token)
                                };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        return RedirectToPage("/Login");
                    }
                    else
                    {
                        ResponseMessage = "Register failed!";
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"HTTP request failed with error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                }
            } else
            {
                isWrongOTP = true;
            } 
            return Page();
        }
    }
}
