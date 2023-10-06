using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1_Food_Inventory.Pages
{
    [Authorize]
    public class InventoryImportModel : PageModel
    {
        private readonly ILogger<InventoryImportModel> _logger;
        private readonly HttpClient _client;
        private readonly string _inventoryApiUrl;

        public IEnumerable<WarehouseResponse> Warehouses { get; set; }
        [BindProperty]
        public string SelectedWarehouse { get; set; }
        [BindProperty]
        public string Quantity { get; set; }
        [BindProperty]
        public string Price { get; set; }
        public List<WarehouseImportRequest> SelectedItems { get; set; } = new List<WarehouseImportRequest>();

        public InventoryImportModel(ILogger<InventoryImportModel> logger, IConfiguration configuration)
        {

            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _inventoryApiUrl = configuration.GetValue<string>("APIEndpoint:Warehouse");

            OnGetAsync().Wait();
        }
        public async Task OnGetAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_inventoryApiUrl}getWarehouses");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Warehouses = JsonSerializer.Deserialize<List<WarehouseResponse>>(apiResponse.Data.ToString(), options);
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                SelectedItems.Add(new WarehouseImportRequest
                {
                    WarehouseItemId = new Guid(SelectedWarehouse),
                    Price = decimal.Parse(Price),
                    Quantity = int.Parse(Quantity)
                });

                try
                {
                    HttpResponseMessage response = await _client.PostAsJsonAsync($"{_inventoryApiUrl}ImportWarehouse", SelectedItems);
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        return Page();
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
            return Page();
        }
    }
}
