using DataAccess.Context;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<OrderDTO>> GetOrderByStatusAsync(Guid statusId)
        {
            if (statusId == Guid.Empty)
            {
                throw new ArgumentException("Status id can not null!");
            }

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            List<Order> orders;

            try
            {
               orders  = _context.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.StatusId == statusId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            foreach(Order order in orders)
            {
                IEnumerable<OrderDetailDTO> details = await GetOrderDetailByOrderIDAsync(order.Id);

                if (details.Count() != 0)
                {

                    orderDTOs.Add(new OrderDTO
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Note = order.Note,
                        ScheduleId = order.ScheduleId,
                        StatusId = order.StatusId,
                        Status = order.Status.Name,
                        Username = order.User.Name,
                        VoucherId = order.VoucherId,
                        Details = details
                    });
                }
            }

            return orderDTOs;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailByOrderIDAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order id can not null!");
            }

            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();
            List<OrderDetail> orderDetails;

            try
            {
                orderDetails = _context.OrderDetails
                                .Include(o => o.Product)
                                .Where(o => o.OrderId == orderId)
                                .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            foreach (OrderDetail detail in orderDetails)
            {
                orderDetailDTOs.Add(new OrderDetailDTO
                {
                    OrderId = detail.OrderId,
                    Id = detail.Id,
                    Note = detail.Note,
                    Price = detail.Price,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    SalePercent = detail.SalePercent,
                    ProductName = detail.Product.Name,
                    Image = detail.Product.Image
                });
            }

            return orderDetailDTOs;
        }

        public async Task UpdateStatusOrderAsync(Guid guid, Guid stutusId)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentNullException("Order ID can not null!");
            }

            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == guid);

                if (order == null)
                {
                    throw new Exception("Order not found!");
                }

                order.StatusId = stutusId;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
        {

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            List<Order> orders;

            try
            {
                orders = _context.Orders
                     .Include(o => o.OrderDetails)
                     .Include(o => o.User)
                     .Include(o => o.Status)
                     .Include(o => o.Voucher)
                     .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while querying the database!", ex);
            }

            foreach (Order order in orders)
            {
                IEnumerable<OrderDetailDTO> details = await GetOrderDetailByOrderIDAsync(order.Id);

                if (details.Count() != 0)
                {

                    orderDTOs.Add(new OrderDTO
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Note = order.Note,
                        ScheduleId = order.ScheduleId,
                        StatusId = order.StatusId,
                        Status = order.Status.Name,
                        Username = order.User.Name,
                        VoucherId = order.VoucherId,
                        Details = details
                    });
                }
            }

            return orderDTOs;
        }
    }
}
