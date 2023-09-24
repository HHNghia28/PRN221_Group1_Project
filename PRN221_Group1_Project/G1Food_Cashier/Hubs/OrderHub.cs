using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1Food_Cashier.Hubs
{
    public class OrderHub : Hub
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl;

        public OrderHub()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = GetOrderAPIEndpoint();
        }

        public override async Task OnConnectedAsync()
        {
            SendOrderPending();
        }

        public async Task SendOrderPending()
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "getOrderPending");
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                APIResponseDTO aPIResponseDTO = JsonSerializer.Deserialize<APIResponseDTO>(stringData, options);

                await Clients.All.SendAsync("ReceiveMessage", aPIResponseDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetOrderAPIEndpoint()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["APIEndpoints:Order"];
            return strConn;
        }
    }
}
