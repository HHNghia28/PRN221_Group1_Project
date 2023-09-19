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
        public Task<IEnumerable<OrderDTO>> GetOrderPending();
        public Task<IEnumerable<OrderDTO>> GetOrderCooking();
        public Task<IEnumerable<OrderDTO>> GetOrderShipping();
        public Task<IEnumerable<OrderDTO>> GetOrderFinish();
    }
}
