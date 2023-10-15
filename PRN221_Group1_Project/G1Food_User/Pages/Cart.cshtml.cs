using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace G1Food_User.Pages
{
    public class CartModel : PageModel
    {
        private readonly ILogger<CartModel> _logger;
        private readonly HttpClient _client;
        private readonly string _cartApiUrl;

        public IEnumerable<CartResponse> Carts { get; private set; }
        public string userIDClaim;

        public CartModel(ILogger<CartModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                // Get the ClaimsPrincipal from the current user
                var user = HttpContext.User as ClaimsPrincipal;

                // Find the "ID" claim
                userIDClaim = user.FindFirst("ID")?.Value;
                if (userIDClaim == null)
                {
                    return RedirectToPage("/Login");
                } else
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
                        Carts = JsonSerializer.Deserialize<List<CartResponse>>(apiResponse.Data.ToString(), options);
                        
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

        public async Task<IActionResult> OnPostAsync(Guid productID, double quantity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<CartRequest> cartRequestList = new List<CartRequest>();
                    // Get the ClaimsPrincipal from the current user
                    var user = HttpContext.User as ClaimsPrincipal;

                    // Find the "ID" claim
                    Guid userIDClaim = Guid.Parse(user.FindFirst("ID")?.Value);
                    CartRequest newCartRequest = new CartRequest
                    {
                        Quantity = quantity, // Set the values as needed
                        ProductId = productID,
                        AccountId = userIDClaim
                    };
                    cartRequestList.Add(newCartRequest);
                    if (userIDClaim == null)
                    {
                        return RedirectToPage("/Login");
                    }
                    else
                    {
                        HttpResponseMessage response = await _client.PostAsJsonAsync($"{_cartApiUrl}addCarts", cartRequestList);
                        response.EnsureSuccessStatusCode();

                        string stringData = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
                        if (HttpContext.Request.Cookies.TryGetValue("cartQuantity", out string cartQuantity))
                        {
                            int newQuantity = Convert.ToInt32(cartQuantity) + 1;
                            Response.Cookies.Append("cartQuantity", newQuantity.ToString());
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
            }
            return RedirectToPage("/Cart");
        }
        
    }
}
