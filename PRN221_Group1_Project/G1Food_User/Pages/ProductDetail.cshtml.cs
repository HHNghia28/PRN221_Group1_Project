using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class ProductDetailModel : PageModel
    {

        private readonly ILogger<ProductDetailModel> _logger;
        private readonly HttpClient _client;
        private readonly string _productApiUrl;

        [BindProperty]
        public ProductRequest productRequest { get; set; }

        public ProductResponse Product { get; private set; }


        public ProductDetailModel(ILogger<ProductDetailModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = configuration.GetValue<string>("APIEndpoint:Product");
        }

        public async Task OnGet()
        {
            try
            {
                var productId = Request.Query["productID"];
                
                HttpResponseMessage response = await _client.GetAsync($"{_productApiUrl}getProduct?id={productId}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Product = JsonSerializer.Deserialize<ProductResponse>(apiResponse.Data.ToString(), options);
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
