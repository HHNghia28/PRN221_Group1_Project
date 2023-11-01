using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class UpdateCartModel : PageModel
    {
        private readonly ILogger<UpdateCartModel> _logger;
        private readonly HttpClient _client;
        private readonly string _cartApiUrl;

        [BindProperty]
        public CartUpdateRequest UpdateRequest { get; set; }
        public IEnumerable<CartResponse> Carts { get; private set; }

        public UpdateCartModel(ILogger<UpdateCartModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
        }

        public async Task<IActionResult> OnGet() {
            double quantity = double.Parse(Request.Query["quantity"]);
            Guid cartID = Guid.Parse(Request.Query["cartID"]);
            UpdateRequest = new CartUpdateRequest {
                Id = cartID,
                Quantity = quantity 
            };
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_cartApiUrl}updateCarts", UpdateRequest);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    return RedirectToPage("/Cart");
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
            return RedirectToPage("/Index");
        }


    }
}
