using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using G1FOODLibrary.DTO;
using G1Food_Admin.Data;
using System.Net.Http.Headers;
using G1FOODLibrary.Entities;
using System.Text.Json;

namespace G1Food_Admin.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client;
        private readonly string _productApiUrl;
        public ProductResponse ProductResponse { get; set; } = default!;
        public DetailsModel(ILogger<IndexModel> logger, IConfiguration configuration)
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
    }
}
