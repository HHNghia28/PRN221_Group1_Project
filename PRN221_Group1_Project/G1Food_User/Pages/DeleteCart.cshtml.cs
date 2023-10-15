using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class DeleteCartModel : PageModel
    {
        private readonly ILogger<DeleteCartModel> _logger;
        private readonly HttpClient _client;
        private readonly string _cartApiUrl;

        public DeleteCartModel(ILogger<DeleteCartModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
        }

        public async Task<IActionResult> OnGetAsync(Guid cartID)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"{_cartApiUrl}deleteCarts?id={cartID.ToString()}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
                if (apiResponse.Success)
                {
                    if (HttpContext.Request.Cookies.TryGetValue("cartQuantity", out string cartQuantity))
                    {
                        int newQuantity = Convert.ToInt32(cartQuantity) - 1;
                        Response.Cookies.Append("cartQuantity", newQuantity.ToString());
                    }
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
