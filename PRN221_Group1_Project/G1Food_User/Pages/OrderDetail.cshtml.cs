using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class OrderDetailModel : PageModel
    {
        private readonly ILogger<OrderDetailModel> _logger;
        private readonly HttpClient _client;
        private readonly string _orderApiUrl;

        public string orderID { get; set; }
        public string userIDClaim;


        public OrderResponse Orders { get; set; }

        public OrderDetailModel(ILogger<OrderDetailModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _orderApiUrl = configuration.GetValue<string>("APIEndpoint:Order");
        }

        public async Task OnGetAsync()
        {
            orderID = Request.Query["orderID"];
            
            // Get the ClaimsPrincipal from the current user
            var user = HttpContext.User as ClaimsPrincipal;

            // Find the "ID" claim
            userIDClaim = user.FindFirst("ID")?.Value;

            try
            {
                if (userIDClaim == null)
                {
                    //return RedirectToPage("/Login");
                }
                else
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_orderApiUrl}getOrderDetail?id={orderID}");
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        Orders = JsonSerializer.Deserialize<OrderResponse>(apiResponse.Data.ToString(), options);
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
        }
    }
}
