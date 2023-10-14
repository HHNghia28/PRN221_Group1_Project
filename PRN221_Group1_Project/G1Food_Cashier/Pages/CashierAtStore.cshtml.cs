using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_Cashier.Pages
{
    [Authorize]
    public class CashierAtSroreModel : PageModel
    {
        private readonly ILogger<CashierAtSroreModel> _logger;
        private readonly HttpClient _client;
        private readonly string _productApiUrl;
        private readonly string _orderApiUrl;

        public List<ProductResponse> Products { get; set; }
        [BindProperty]
        public string SelectedProduct { get; set; }
        [BindProperty]
        public List<OrderDetailRequest> Orders { get; set; } = new List<OrderDetailRequest>();
        public CashierAtSroreModel(ILogger<CashierAtSroreModel> logger, IConfiguration configuration)
        {

            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = configuration.GetValue<string>("APIEndpoint:Product");
            _orderApiUrl = configuration.GetValue<string>("APIEndpoint:Order");
        }
        public async Task OnGetAsync()
        {
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

                    if (Products != null && Products.Count > 0)
                    {
                        foreach (var item in Products)
                        {
                            Orders.Add(new OrderDetailRequest
                            {
                                ProductId = item.Id,
                                Note = null,
                                Quantity = 0
                            });
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

        public async Task<IActionResult> OnPostAsync()
        {

            if (Orders != null)
            {
                try
                {
                    List<OrderDetailRequest> newOrderDetail = Orders.Where(item => item.Quantity != 0).ToList();

                    OrderRequest orderRequest = new OrderRequest
                    {
                        Note = "Order at store",
                        UserId = Guid.Empty,
                        VoucherCode = null,
                        Details = newOrderDetail
                    };

                    HttpResponseMessage response = await _client.PostAsJsonAsync($"{_orderApiUrl}addOrder", orderRequest);
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        return RedirectToPage("CashierAtStore");
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
            return RedirectToPage("CashierAtStore");
        }
    }
}
