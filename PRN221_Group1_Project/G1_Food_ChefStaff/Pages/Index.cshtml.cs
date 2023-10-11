using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using G1FOODLibrary.DTO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace G1_Food_ChefStaff.Pages
{
    public class ChefDashboardModel : PageModel
    {
        private readonly ILogger<ChefDashboardModel> _logger;
        private readonly HttpClient _client;
        private readonly string _orderApiUrl;

        public IEnumerable<OrderResponse> Orders { get; set; }

        public ChefDashboardModel(ILogger<ChefDashboardModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _orderApiUrl = configuration.GetValue<string>("APIEndpoint:Order");
        }

        public async Task OnGetAsync()
        {

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}getOrderCooking");
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