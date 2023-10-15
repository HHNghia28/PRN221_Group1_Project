using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using G1FOODLibrary.Entities;
using G1FOODLibrary.DTO;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_Admin.Pages.Voucher
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly HttpClient _client;
        private readonly string _voucherApiUrl;

        [BindProperty]
        public VoucherRequest Voucher { get; set; }
        
        public VoucherResponse VoucherResponse { get; set; }

        public List<StatusResponse> Status { get; set; }

        public EditModel(ILogger<EditModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _voucherApiUrl = configuration.GetValue<string>("APIEndpoint:Voucher");
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_voucherApiUrl}getVoucher?id={id}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    VoucherResponse = JsonSerializer.Deserialize<VoucherResponse>(apiResponse.Data.ToString(), options);

                    Voucher = new VoucherRequest
                    {
                        Code = VoucherResponse.Code,
                        Description = VoucherResponse.Description,
                        Quantity = VoucherResponse.Quantity,
                        SalePercent = VoucherResponse.SalePercent,
                        StatusId = VoucherResponse.StatusId
                    };
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                }

                response = await _client.GetAsync($"{_voucherApiUrl}getVouchersStatus");
                response.EnsureSuccessStatusCode();

                stringData = await response.Content.ReadAsStringAsync();
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Status = JsonSerializer.Deserialize<List<StatusResponse>>(apiResponse.Data.ToString(), options);
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_voucherApiUrl}updateVoucher?id={id}", Voucher);
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                    return NotFound();
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

            return RedirectToPage("./Index");
        }
    }
}
