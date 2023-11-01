using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ILogger<CategoryModel> _logger;
        private readonly HttpClient _client;
        private readonly string _productApiUrl;

        public IEnumerable<ProductResponse> Products { get; private set; }
        public IEnumerable<CategoryResponse> Categories { get; private set; }

        public CategoryModel(ILogger<CategoryModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = configuration.GetValue<string>("APIEndpoint:Product");
        }
        public async Task OnGet()
        {
            string categroyID = Request.Query["cID"];
            string txtSearch = Request.Query["txtSearch"];
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_productApiUrl}getProducts");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Products = JsonSerializer.Deserialize<List<ProductResponse>>(apiResponse.Data.ToString(), options);
                    if (categroyID != null)
                    {
                        foreach (var product in Products)
                        {
                            Products = Products.Where(product => product.CategogyId.ToString() == categroyID);
                        }
                    }
                    if(txtSearch != null)
                    {
                        foreach (var product in Products)
                        {
                            Products = Products.Where(product => product.Name.ToUpper().Contains(txtSearch.ToUpper().Trim()));
                        }
                    }
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
