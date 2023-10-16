using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace G1Food_User.Pages
{
    public class UpdateCartModel : PageModel
    {
        private readonly ILogger<UpdateCartModel> _logger;
        private readonly HttpClient _client;
        private readonly string _cartApiUrl;

        public IEnumerable<CartResponse> Carts { get; private set; }

        public UpdateCartModel(ILogger<UpdateCartModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _cartApiUrl = configuration.GetValue<string>("APIEndpoint:Cart");
        }

        //public async Task<IActionResult> OnGet(string productID, int quantity)
        //{
        //    HttpResponseMessage response = await _client.PutAsJsonAsync($"{_cartApiUrl}updateProduct?id={productID", Product);
        //    response.EnsureSuccessStatusCode();
        //    return;
        //}
    }
}
