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
    [Authorize]
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
            int maxRetries = 3; 
            int retryCount = 0;
            bool apiCallSuccess = false;

            while (retryCount < maxRetries && !apiCallSuccess)
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
                        apiCallSuccess = true; 
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            await Task.Delay(1000); 
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"HTTP request failed with error: {ex.Message}");
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        await Task.Delay(1000); 
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        await Task.Delay(1000); 
                    }
                }
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