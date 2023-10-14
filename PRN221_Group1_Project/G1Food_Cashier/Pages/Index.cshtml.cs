using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Text.Json;
using G1FOODLibrary.DTO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace G1Food_Cashier.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private readonly string _orderApiUrl;

        public IEnumerable<OrderResponse> Orders { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
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
                    HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}getOrderPending");
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
                        await Task.Delay(1000); // Chờ 1 giây trước khi thử lại
                    }
                }
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            string formType = Request.Form["formType"];
            string orderID = Request.Form["orderID"];

            try
            {
                HttpResponseMessage response;

                if (formType == "confirm")
                {
                    response = await _client.PutAsync($"{_orderApiUrl}orderUpdateCooking?id={orderID}", null);
                }
                else
                {
                    response = await _client.PutAsync($"{_orderApiUrl}orderUpdateBlock?id={orderID}", null);
                }
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