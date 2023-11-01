


using Azure;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class PayingModel : PageModel
    {
        private readonly ILogger<PayingModel> _logger;
        private readonly HttpClient _client;
        private readonly string _voucherApiUrl;
        private readonly string _cartApiUrl;
        private readonly string _orderApiUrl;
        private readonly string _accountApiUrl;

        public string userIDClaim;
        public string userNameClaim;
        public string userEmailClaim;
        public string userPhoneClaim;
        public string userAddressClaim;

        [BindProperty]
        public string note { get; set; }
        [BindProperty]
        public string voucherCode { get; set; }

        public List<CartResponse> ListCarts { get; set; }
        public IEnumerable<VoucherResponse> Vouchers { get; private set; }

        public UserResponse User { get; private set; }

        public List<OrderDetailRequest> Orders { get; set; } = new List<OrderDetailRequest>();

        public PayingModel(ILogger<PayingModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _voucherApiUrl = configuration.GetValue<string>("APIEndpoint:Voucher");
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
            _orderApiUrl = configuration.GetValue<string>("APIEndpoint:Order");
            _accountApiUrl = configuration.GetValue<string>("APIEndpoint:Account");
        }

        public async Task OnGet()
        {
            try
            {
                // Get the ClaimsPrincipal from the current user
                var user = HttpContext.User as ClaimsPrincipal;

                // Find the "ID" claim
                userIDClaim = user.FindFirst("ID")?.Value;
                userNameClaim = user.FindFirst("Name")?.Value;
                userEmailClaim = user.FindFirst("Email")?.Value;
                userPhoneClaim = user.FindFirst("Phone")?.Value;
                userAddressClaim = user.FindFirst("Address")?.Value;
                if (userIDClaim == null)
                {
                    //return RedirectToPage("/Login");
                }
                else
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_voucherApiUrl}getVouchers");
                    response.EnsureSuccessStatusCode();


                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        Vouchers = JsonSerializer.Deserialize<List<VoucherResponse>>(apiResponse.Data.ToString(), options);
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


            try
            {
                    HttpResponseMessage response = await _client.GetAsync($"{_cartApiUrl}getCarts?id={userIDClaim}");
                    response.EnsureSuccessStatusCode();

                    string stringData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                    if (apiResponse.Success)
                    {
                        ListCarts = JsonSerializer.Deserialize<List<CartResponse>>(apiResponse.Data.ToString(), options);
                        if (ListCarts != null && ListCarts.Count() > 0)
                        {
                            foreach (var item in ListCarts)
                            {
                                Orders.Add(new OrderDetailRequest
                                {
                                    Quantity = item.Quantity,
                                    Note = null,
                                    ProductId = item.ProductId,
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

        public async Task<IActionResult> OnPostAsync() {
            string userID = "";
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_accountApiUrl}getUserByAccountId?id=d5cac6e2-1456-47e1-8ead-e082a5b047fa");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);
              
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request failed with error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }

            try
                {
                    //List<OrderDetailRequest> newOrderDetail = Orders.Where(item => item.Quantity != 0).ToList();

                    OrderRequest orderRequest = new OrderRequest
                    {
                        Note = note,
                        UserId = Guid.Parse(userID),
                        VoucherCode = null,
                        Details = Orders
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
                        return RedirectToPage("./UserProfile");
                    }
                    else
                    {
                        _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    }
                    foreach (var cart in ListCarts)
                    {
                        response = await _client.DeleteAsync($"{_cartApiUrl}deleteCarts?id={cart.Id.ToString()}");
                        response.EnsureSuccessStatusCode();
                    }
                    if (ListCarts.Count() <= 0)
                    {
                        if (HttpContext.Request.Cookies.TryGetValue("cartQuantity", out string cartQuantity))
                        {
                            int newCartQuantity = 0;
                            Response.Cookies.Append("cartQuantity", newCartQuantity.ToString());
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
            //}
            return RedirectToPage("Index");
        }
    }
}
