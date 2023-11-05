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

        public async Task AddOrderAsync(OrderRequest order)
        {
            if (order == null)
            {
                throw new ArgumentException("Order can not null!");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var voucher = new Voucher();

                if (order.VoucherCode != null)
                {
                    voucher = _context.Vouchers.FirstOrDefault(v => v.Code.ToLower().Equals(order.VoucherCode.ToLower()));

                    if (voucher == null)
                    {
                        throw new Exception("Voucher not exist!");
                    }

                    if (voucher.StatusId != new Guid("DF9DC864-4ABD-4277-9ECD-751814C8763A"))
                    {
                        throw new Exception("Voucher expired!");
                    }

                    if (voucher.Quantity < 1)
                    {
                        throw new Exception("Vouchers are out of stock");
                    }

                    voucher.Quantity -= 1;

                }

                Guid guid = Guid.NewGuid();

                Order newOrder = new Order
                {
                    Id = guid,
                    Date = DateTime.Now,
                    Note = order.Note,
                    StatusId = order.UserId == Guid.Empty ? new Guid("4F716054-4241-4A5C-A484-9CCF9705D3B0") : new Guid("DE3E4850-B990-4D62-BA90-4BBB49506722"),
                    UserId = order.UserId == Guid.Empty ? null : order.UserId,
                    ScheduleId = null,
                    VoucherId = order.VoucherCode != null ? voucher.Id : null
                };

                List<OrderDetail> orderDetails = new List<OrderDetail>();

                foreach (OrderDetailRequest detail in order.Details)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == detail.ProductId);

                    if (product == null)
                    {
                        throw new Exception("Product not exist!");
                    }

                    orderDetails.Add(new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        Note = detail.Note,
                        Price = product.Price,
                        Quantity = detail.Quantity,
                        SalePercent = product.SalePercent,
                        OrderId = guid,
                        ProductId = detail.ProductId
                    });
                }

                _context.Orders.Add(newOrder);
                foreach (OrderDetail detail in orderDetails)
                {
                    _context.OrderDetails.Add(detail);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderResponse>> GetOrderByStatusAsync(Guid statusId)
        {
            if (statusId == Guid.Empty)
            {
                throw new ArgumentException("Status id can not null!");
            }

            List<OrderResponse> orderDTOs = new List<OrderResponse>();
            List<Order> orders;

            try
            {
               orders  = _context.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.User)
                    .Include(o => o.Status)
                    .Include(o => o.Voucher)
                    .Where(o => o.StatusId == statusId)
                    .OrderBy(o => o.Date)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            foreach(Order order in orders)
            {
                IEnumerable<OrderDetailResponse> details = await GetOrderDetailByOrderIDAsync(order.Id);

                if (details.Count() != 0)
                {

                    orderDTOs.Add(new OrderResponse
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Note = order.Note,
                        Status = order.Status.Name,
                        Username = order.User == null ? "Khách tại cửa hàng" : order.User.Name,
                        SalePercent = order.Voucher == null ? 0 : order.Voucher.SalePercent,
                        Details = details
                    });
                }
            }

            return orderDTOs;
        }

        public async Task<IEnumerable<OrderDetailResponse>> GetOrderDetailByOrderIDAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order id can not null!");
            }

            List<OrderDetailResponse> orderDetailDTOs = new List<OrderDetailResponse>();
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
                throw new Exception(ex.Message);
            }

            foreach (OrderDetail detail in orderDetails)
            {
                orderDetailDTOs.Add(new OrderDetailResponse
                {
                    Id = detail.Id,
                    Note = detail.Note,
                    Price = detail.Price,
                    Quantity = detail.Quantity,
                    SalePercent = detail.SalePercent,
                    ProductName = detail.Product.Name,
                    Image = detail.Product.Image
                });
            }

            return orderDetailDTOs;
        }

        public async Task UpdateStatusOrderAsync(Guid orderId, Guid statusId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orderId), "Order ID cannot be empty!");
            }

            if (statusId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(statusId), "Status ID cannot be empty!");
            }

            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new Exception("Order not found!");
                }

                order.StatusId = statusId;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> GetOrderAsync(Guid id)
        {


            try
            {
                var order = _context.Orders
                     .Include(o => o.OrderDetails)
                     .Include(o => o.User)
                     .Include(o => o.Status)
                     .Include(o => o.Voucher)
                     .FirstOrDefault(o => o.Id == id);

                IEnumerable<OrderDetailResponse> details = await GetOrderDetailByOrderIDAsync(id);

                OrderResponse orderResponse = new OrderResponse
                {
                    Id = id,
                    Date = order.Date,
                    Details = details,
                    Note = order.Note,
                    SalePercent = order.Voucher == null ? 0 : order.Voucher.SalePercent,
                    Status = order.Status.Name,
                    Username = order.User == null ? "Khách tại cửa hàng" : order.User.Name
                };

                return orderResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync()
        {

            List<OrderResponse> orderDTOs = new List<OrderResponse>();
            List<Order> orders;

            try
            {
                orders = _context.Orders
                     .Include(o => o.OrderDetails)
                     .Include(o => o.User)
                     .Include(o => o.Status)
                     .Include(o => o.Voucher)
                     .OrderByDescending(o => o.Date)
                     .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            foreach (Order order in orders)
            {
                IEnumerable<OrderDetailResponse> details = await GetOrderDetailByOrderIDAsync(order.Id);

                if (details.Count() != 0)
                {

                    orderDTOs.Add(new OrderResponse
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Note = order.Note,
                        UserID = order.User.Id,
                        Status = order.Status.Name,
                        Username = order.User == null ? "Khách tại cửa hàng" : order.User.Name,
                        Details = details
                    }); ;
                }
            }

            return orderDTOs;
        }

        public async Task<IEnumerable<OrderResponse>> GetOrderHistoryAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new ArgumentNullException("Account ID can not null!");
            }

            try
            {
                List<OrderResponse> orderResponses = new List<OrderResponse>();

                var users = _context.Users.Where(u => u.AccountId == accountId).ToList();

                foreach (var item in users)
                {
                    var orders = _context.Orders
                        .Include(o => o.User)
                        .Where(o => o.UserId == item.Id)
                        .ToList();

                    foreach (var item1 in orders)
                    {
                        orderResponses.Add(new OrderResponse
                        {
                            Id = item1.Id,
                            Date = item1.Date,
                            Note = item1.Note,
                            Username = item1.User.Name
                        });
                    }
                }

                return orderResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderResponse> GetOrderDetailAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException("Order ID can not null!");
            }

            try
            {
                List<OrderDetailResponse> orderDetailResponses = new List<OrderDetailResponse>();

                var order = _context.Orders.Include(o => o.Status).Include(o => o.User).FirstOrDefault(o => o.Id == orderId);
                var orderDetails = _context.OrderDetails.Include(o => o.Product).Where(o => o.OrderId == order.Id).ToList();

                foreach (var item in orderDetails)
                {
                    orderDetailResponses.Add(new OrderDetailResponse
                    {
                        Id = item.Id,
                        Image = item.Product.Image,
                        ProductName = item.Product.Name,
                        Price = item.Price,
                        Note = item.Note,
                        Quantity = item.Quantity,
                        SalePercent = item.SalePercent
                    });
                }

                OrderResponse orderResponse = new OrderResponse
                {
                    Id = order.Id,
                    Note = order.Note,
                    Date = order.Date,
                    Status = order.Status.Name,
                    Username = order.User.Name,
                    VoucherCode = order.Voucher.Code,
                    VoucherPercent = order.Voucher.SalePercent,
                    Details = orderDetailResponses
                };

                return orderResponse;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
