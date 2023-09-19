using DataAccess.Context;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class OrderDAO
    {
        private DBContext _context;
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public OrderDAO() => _context = new DBContext();

        public async Task<OrderDTO> AddOrderAsync(OrderCreateDTO order)
        {
            if (order == null)
            {
                throw new ArgumentException("Order can not null!");
            }

            Guid guid = Guid.NewGuid();

            Order newOrder = new Order
            {
                Id = guid,
                Date = DateTime.Now,
                Note = order.Note,
                StatusId = new Guid("DE3E4850-B990-4D62-BA90-4BBB49506722"),
                UserId = order.UserId,
                ScheduleId = order.ScheduleId == Guid.Empty ? null : order.ScheduleId,
                VoucherId = order.VoucherId == Guid.Empty ? null : order.VoucherId
            };

            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (OrderDetailDTO detail in order.Details) 
            {
                orderDetails.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    Note = detail.Note,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    SalePercent = detail.SalePercent,
                    OrderId = guid,
                    ProductId = detail.ProductId
                });
            }

            try
            {
                _context.Orders.Add(newOrder);
                foreach (OrderDetail detail in orderDetails)
                {
                    _context.OrderDetails.Add(detail);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            foreach (OrderDetailDTO detail in order.Details)
            {
                detail.OrderId = guid;
            }

            OrderDTO orderDTO = new OrderDTO
            {
                Id = guid,
                Date = DateTime.Now,
                Note = order.Note,
                StatusId = new Guid("DE3E4850-B990-4D62-BA90-4BBB49506722"),
                UserId = order.UserId,
                ScheduleId = order.ScheduleId == Guid.Empty ? null : order.ScheduleId,
                VoucherId = order.VoucherId == Guid.Empty ? null : order.VoucherId,
                Details = order.Details
            };

            return orderDTO;
        }
    }
}
