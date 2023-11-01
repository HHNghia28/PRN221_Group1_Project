using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class UpdateProfileModel : PageModel
    {
        private readonly ILogger<UpdateProfileModel> _logger;
        private readonly HttpClient _client;
        private readonly string _accountApiUrl;

        public string roleIDUser = "c73813a0-ce6e-4f59-b281-507690b51406";
        public string statusIDIsActive = "750301ce-21b9-444e-a0d3-53824614ca40";

        public string userIDClaim;
        public string userNameClaim;
        public string userEmailClaim;
        public string userPhoneClaim;
        public string userAddressClaim;

        [BindProperty]
        public AccountUpdateRequest UpdateRequest { get; set; }
        public AccountResponse Account;


        public UpdateProfileModel(ILogger<UpdateProfileModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }
        public async Task<IActionResult> OnGet()
        {
            Guid accountID = Guid.Parse(Request.Query["id"]); 
            string userName = Request.Query["userName"];
            string phone = Request.Query["phone"];
            string address = Request.Query["address"];
            string email = Request.Query["email"];
            string token = Request.Query["token"];

            UpdateRequest = new AccountUpdateRequest {
                    Name = userName,
                    AddressDetail = address,
                    Phone = phone,
                    RoleId = Guid.Parse(roleIDUser),
                    StatusId = Guid.Parse(statusIDIsActive)
            };

            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_accountApiUrl}updateAccount?id={accountID}", UpdateRequest);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
                if (apiResponse.Success)
                {
                    var claims = new List<Claim>
                            {
                                new Claim("ID", accountID.ToString()),
                                new Claim("Name", userName),
                                new Claim("Email",email),
                                new Claim("Phone", phone),
                                new Claim("Address", address),
                                new Claim("Role", roleIDUser),
                                new Claim("Token", token)
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

                    return RedirectToPage("/UserProfile");
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
            return RedirectToPage("/UserProfile");
        }
    }
}
