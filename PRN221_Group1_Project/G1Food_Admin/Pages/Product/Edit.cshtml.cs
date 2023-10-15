using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G1FOODLibrary.DTO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace G1Food_Admin.Pages.Product
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly HttpClient _client;
        private readonly string _productApiUrl;

        public List<CategoryResponse> Categories { get; set; }
        public List<StatusResponse> Status { get; set; }

        [BindProperty]
        public ProductRequest Product { get; set; }
        public ProductResponse ProductResponse { get; set; } = default!;
        public EditModel(ILogger<EditModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _productApiUrl = configuration.GetValue<string>("APIEndpoint:Product");
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_productApiUrl}getProduct?id={id}");
                response.EnsureSuccessStatusCode();

                string stringData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    ProductResponse = JsonSerializer.Deserialize<ProductResponse>(apiResponse.Data.ToString(), options);

                    Product = new ProductRequest
                    {
                        Name = ProductResponse.Name,
                        CategogyId = ProductResponse.CategogyId,
                        Description = ProductResponse.Description,
                        Image = ProductResponse.Image,
                        Price = ProductResponse.Price,
                        SalePercent = ProductResponse.SalePercent,
                        StatusId = ProductResponse.StatusId
                    };
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                }

                response = await _client.GetAsync($"{_productApiUrl}getProductCategories");
                response.EnsureSuccessStatusCode();

                stringData = await response.Content.ReadAsStringAsync();
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

                if (apiResponse.Success)
                {
                    Categories = JsonSerializer.Deserialize<List<CategoryResponse>>(apiResponse.Data.ToString(), options);
                }
                else
                {
                    _logger.LogError($"API call failed with message: {apiResponse.Message}");
                }

                response = await _client.GetAsync($"{_productApiUrl}getStatus");
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

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync($"{_productApiUrl}updateProduct?id={id}", Product);
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
