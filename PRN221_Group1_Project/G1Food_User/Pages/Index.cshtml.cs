using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace G1Food_User.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private readonly string _voucherApiUrl;
        private readonly string _productApiUrl;

        //public AccountResponse Account { get; set; }
        public IEnumerable<ProductResponse> Products { get; private set; }
        public IEnumerable<VoucherResponse> Vouchers { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _voucherApiUrl = configuration.GetValue<string>("APIEndpoint:Voucher");
            _productApiUrl = configuration.GetValue<string>("APIEndpoint:Product");
        }

        public async Task OnGet()
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

                //if (Request.Cookies.ContainsKey("AccountCookie"))
                //{
                //    string accountJson = Request.Cookies["AccountCookie"];
                //    Account = JsonSerializer.Deserialize<AccountResponse>(accountJson, options);
                //}
                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Products = JsonSerializer.Deserialize<List<ProductResponse>>(apiResponse.Data.ToString(), options);
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

            try
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
