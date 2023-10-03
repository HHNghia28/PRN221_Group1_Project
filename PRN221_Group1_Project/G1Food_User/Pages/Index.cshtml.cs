using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_User.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _client = null;
        private string _scheduleApiUrl;

        public IEnumerable<MenuResponse> Products { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _scheduleApiUrl = GetAPIEndpointSchedule();
        }

        private string GetAPIEndpointSchedule()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["APIEndpoint:Schedule"];
            return strConn;
        }

        public async Task OnGet()
        {
            HttpResponseMessage response = await _client.GetAsync(_scheduleApiUrl + "getMenuNow");
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            APIResponse apiResponse = JsonSerializer.Deserialize<APIResponse>(stringData, options);

            if (apiResponse.Success)
            {
                Products = JsonSerializer.Deserialize<List<MenuResponse>>(apiResponse.Data.ToString(), options);
            }
        }
    }
}