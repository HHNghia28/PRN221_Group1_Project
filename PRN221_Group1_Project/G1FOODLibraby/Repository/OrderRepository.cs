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
    }
}
