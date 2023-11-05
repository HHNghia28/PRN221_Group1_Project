using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class UserProfileModel : PageModel
    {
        private readonly ILogger<UserProfileModel> _logger;
        private readonly HttpClient _client;
        private readonly string _orderApiUrl;
        private readonly string _accountApiUrl;

        public string userIDClaim;
        public string userNameClaim;
        public string userEmailClaim;
        public string userPhoneClaim;
        public string userAddressClaim;
        public string userTokenClaim;

        public IEnumerable<OrderResponse> Orders { get; set; }
        public AccountResponse Account;
        //public Guid userId;

        public UserProfileModel(ILogger<UserProfileModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _orderApiUrl = configuration.GetValue<string>("APIEndpoint:Order");
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the ClaimsPrincipal from the current user
            var user = HttpContext.User as ClaimsPrincipal;

            // Find the "ID" claim
            userIDClaim = user.FindFirst("ID")?.Value;
            userNameClaim = user.FindFirst("Name")?.Value;
            userEmailClaim = user.FindFirst("Email")?.Value;
            userPhoneClaim = user.FindFirst("Phone")?.Value;
            userAddressClaim = user.FindFirst("Address")?.Value;
            userTokenClaim = user.FindFirst("Token")?.Value;

            Guid? userID = null;
            List<UserResponse> userResponses = new List<UserResponse>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_accountApiUrl}getUserByAccountId?id={userIDClaim}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
                if (apiResponse.Success)
                {
                    userResponses = JsonSerializer.Deserialize<List<UserResponse>>(apiResponse.Data.ToString(), options);
                }
                foreach (var item in userResponses)
                {
                    userID = item.Id;
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

            try
            { 

                if (userIDClaim == null) {
                    return RedirectToPage("/Login");
                } else
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}getOrders");
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        Orders = JsonSerializer.Deserialize<List<OrderResponse>>(apiResponse.Data.ToString(), options);
                        Orders = Orders.Where(o => o.UserID == userID);
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string orderID = Request.Form["orderID"];

            try
            {
                HttpResponseMessage response;

                response = await _client.PutAsync($"{_orderApiUrl}orderUpdateDelivering?id={orderID}", null);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    return RedirectToPage("/index");
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
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

            return RedirectToPage("/index");
        }
    }
}
