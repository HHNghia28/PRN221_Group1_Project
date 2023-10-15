using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        public Task AddOrderAsync(OrderRequest orderDTO);
        public Task<IEnumerable<OrderResponse>> GetOrdersAsync();
        public Task<IEnumerable<OrderResponse>> GetOrderPendingAsync();
        public Task<IEnumerable<OrderResponse>> GetOrderCookingAsync();
        public Task<IEnumerable<OrderResponse>> GetOrderDeliveringAsync();
        public Task<IEnumerable<OrderResponse>> GetOrderFinishAsync();
        public Task UpdateOrderStatusToCookingAsync(Guid orderId);
        public Task UpdateOrderStatusToDeliveringAsync(Guid orderId);
        public Task UpdateOrderStatusToDeliveredAsync(Guid orderId);
        public Task UpdateOrderStatusToBlockAsync(Guid orderId);
        public Task<OrderResponse> GetOrderDetailAsync(Guid orderId);
        public Task<IEnumerable<OrderResponse>> GetOrderHistoryAsync(Guid accountId);
        public Task<OrderResponse> GetOrderAsync(Guid id);
    }
}
