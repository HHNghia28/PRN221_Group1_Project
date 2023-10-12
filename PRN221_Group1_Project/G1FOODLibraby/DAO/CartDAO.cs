using DataAccess.Context;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class CartDAO
    {
        private DBContext _context;
        private static CartDAO instance = null;
        private static readonly object instanceLock = new object();

        public static CartDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartDAO();
                    }
                    return instance;
                }
            }
        }

        public CartDAO() => _context = new DBContext();

        public IEnumerable<CartResponse> GetCarts(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            try
            {

                var carts = _context.Carts.Include(c => c.Product).Where(c => c.AccountId == id).ToList();
                List<CartResponse> result = new List<CartResponse>();
                foreach (var item in carts)
                {
                    result.Add(new CartResponse
                    {
                        Id = item.Id,
                        Description = item.Product.Description,
                        Image = item.Product.Image,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        SalePercent = item.Product.SalePercent
                    });
                }

                return result;

            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddCart(List<CartRequest> carts)
        {
            if(carts.Count == 0)
            {
                throw new Exception("List cart can not null!");
            }

            try
            {
                List<Cart> list = new List<Cart>();
                foreach (var item in carts)
                {
                    list.Add(new Cart
                    {
                        Id = Guid.NewGuid(),
                        AccountId = item.AccountId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                _context.Carts.AddRange(list);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateCart(CartUpdateRequest cart)
        {
            if (cart.Id == Guid.Empty)
            {
                throw new Exception("ID cart can not null!");
            }

            try
            {
                var existCart = _context.Carts.FirstOrDefault(c => c.Id == cart.Id);

                if (existCart == null)
                {
                    throw new Exception("Cart not found!");
                }

                existCart.Quantity = cart.Quantity;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCart(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            try
            {
                Cart cart = _context.Carts.FirstOrDefault(c => c.Id == id);

                if(cart == null)
                {
                    throw new Exception("Cart not found!");
                }

                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
