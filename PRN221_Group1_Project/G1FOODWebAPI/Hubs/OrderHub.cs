using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;

namespace G1FOODWebAPI.Hubs
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
        }

        public async Task SendOrderPendingAsync()
        {
            string orderMsg = "Pending";
            await Clients.All.SendAsync("ReceiveMessage", orderMsg);
        }

        public async Task SendOrderCookingAsync()
        {
            string orderMsg = "Cooking";
            await Clients.All.SendAsync("ReceiveMessage", orderMsg);
        }

        public async Task SendOrderDeliveringingAsync()
        {
            string orderMsg = "Delivering";
            await Clients.All.SendAsync("ReceiveMessage", orderMsg);
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
