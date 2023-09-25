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
        public Task<OrderDTO> AddOrderAsync(OrderCreateDTO orderDTO);
        public Task<IEnumerable<OrderDTO>> GetOrdersAsync();
        public Task<IEnumerable<OrderDTO>> GetOrderPendingAsync();
        public Task<IEnumerable<OrderDTO>> GetOrderCookingAsync();
        public Task<IEnumerable<OrderDTO>> GetOrderDeliveringAsync();
        public Task<IEnumerable<OrderDTO>> GetOrderFinishAsync();
        public Task UpdateOrderStatusToCookingAsync(Guid orderId);
        public Task UpdateOrderStatusToDeliveringAsync(Guid orderId);
        public Task UpdateOrderStatusToDeliveredAsync(Guid orderId);
        public Task UpdateOrderStatusToBlockAsync(Guid orderId);
    }
}
