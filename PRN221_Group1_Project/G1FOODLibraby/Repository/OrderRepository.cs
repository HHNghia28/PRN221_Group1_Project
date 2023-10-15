using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public Task AddOrderAsync(OrderRequest orderDTO) => OrderDAO.Instance.AddOrderAsync(orderDTO);

        public Task<IEnumerable<OrderResponse>> GetOrderCookingAsync() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("4F716054-4241-4A5C-A484-9CCF9705D3B0"));

        public Task<IEnumerable<OrderResponse>> GetOrderFinishAsync() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("5E1BDE6C-589F-413C-807C-A36E6C64A090"));

        public Task<IEnumerable<OrderResponse>> GetOrderPendingAsync() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("DE3E4850-B990-4D62-BA90-4BBB49506722"));

        public Task<IEnumerable<OrderResponse>> GetOrderDeliveringAsync() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("ECDB311C-0C0B-46AB-91D3-9F44F0C86FA5"));

        public Task UpdateOrderStatusToBlockAsync(Guid orderId) => OrderDAO.Instance.UpdateStatusOrderAsync(orderId, new Guid("E08E4AB1-2C5D-45CD-9AA6-A18009590B54"));

        public Task UpdateOrderStatusToCookingAsync(Guid orderId) => OrderDAO.Instance.UpdateStatusOrderAsync(orderId, new Guid("4F716054-4241-4A5C-A484-9CCF9705D3B0"));

        public Task UpdateOrderStatusToDeliveredAsync(Guid orderId) => OrderDAO.Instance.UpdateStatusOrderAsync(orderId, new Guid("5E1BDE6C-589F-413C-807C-A36E6C64A090"));

        public Task UpdateOrderStatusToDeliveringAsync(Guid orderId) => OrderDAO.Instance.UpdateStatusOrderAsync(orderId, new Guid("ECDB311C-0C0B-46AB-91D3-9F44F0C86FA5"));

        public Task<IEnumerable<OrderResponse>> GetOrdersAsync() => OrderDAO.Instance.GetOrdersAsync();

        public Task<OrderResponse> GetOrderDetailAsync(Guid orderId) => OrderDAO.Instance.GetOrderDetailAsync(orderId);

        public Task<IEnumerable<OrderResponse>> GetOrderHistoryAsync(Guid accountId) => OrderDAO.Instance.GetOrderHistoryAsync(accountId);

        public Task<OrderResponse> GetOrderAsync(Guid id) => OrderDAO.Instance.GetOrderAsync(id);
    }
}
