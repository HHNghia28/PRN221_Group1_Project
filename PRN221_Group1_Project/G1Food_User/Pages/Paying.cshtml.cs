using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class PayingModel : PageModel
    {
        private readonly ILogger<PayingModel> _logger;
        private readonly HttpClient _client;
        private readonly string _voucherApiUrl;
        private readonly string _cartApiUrl;

        public string userIDClaim;
        public string userNameClaim;
        public string userEmailClaim;
        public string userPhoneClaim;
        public string userAddressClaim;

        public List<CartResponse> ListCarts { get; set; }
        public IEnumerable<VoucherResponse> Vouchers { get; private set; }

        public PayingModel(ILogger<PayingModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _voucherApiUrl = configuration.GetValue<string>("APIEndpoint:Voucher");
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
        }

        public async Task OnGet()
        {
            try
            {
                // Get the ClaimsPrincipal from the current user
                var user = HttpContext.User as ClaimsPrincipal;

                // Find the "ID" claim
                userIDClaim = user.FindFirst("ID")?.Value;
                userNameClaim = user.FindFirst("Name")?.Value;
                userEmailClaim = user.FindFirst("Email")?.Value;
                userPhoneClaim = user.FindFirst("Phone")?.Value;
                userAddressClaim = user.FindFirst("Address")?.Value;
                if (userIDClaim == null)
                {
                    //return RedirectToPage("/Login");
                }
                else
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_voucherApiUrl}getVouchers");
                    response.EnsureSuccessStatusCode();


                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        Vouchers = JsonSerializer.Deserialize<List<VoucherResponse>>(apiResponse.Data.ToString(), options);
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    }

                    string serializedCarts = Request.Cookies["Carts"];

                    if (!string.IsNullOrEmpty(serializedCarts))
                    {
                        // Deserialize the data back to a list
                        ListCarts = JsonSerializer.Deserialize<List<CartResponse>>(serializedCarts);

                        // Use the carts list as needed
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


            try
            {
                    HttpResponseMessage response = await _client.GetAsync($"{_cartApiUrl}getCarts?id={userIDClaim}");
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        ListCarts = JsonSerializer.Deserialize<List<CartResponse>>(apiResponse.Data.ToString(), options);
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
        }
    }
}
