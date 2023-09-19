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
        public Task<OrderDTO> AddOrderAsync(OrderCreateDTO orderDTO) => OrderDAO.Instance.AddOrderAsync(orderDTO);

        public Task<IEnumerable<OrderDTO>> GetOrderCooking() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("4F716054-4241-4A5C-A484-9CCF9705D3B0"));

        public Task<IEnumerable<OrderDTO>> GetOrderFinish() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("5E1BDE6C-589F-413C-807C-A36E6C64A090"));

        public Task<IEnumerable<OrderDTO>> GetOrderPending() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("DE3E4850-B990-4D62-BA90-4BBB49506722"));

        public Task<IEnumerable<OrderDTO>> GetOrderShipping() => OrderDAO.Instance.GetOrderByStatusAsync(new Guid("ECDB311C-0C0B-46AB-91D3-9F44F0C86FA5"));
    }
}
