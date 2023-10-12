using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CartRepository : ICartRepository
    {
        public void AddCart(List<CartRequest> carts) => CartDAO.Instance.AddCart(carts);

        public void DeleteCart(Guid id) => CartDAO.Instance.DeleteCart(id);

        public IEnumerable<CartResponse> GetCarts(Guid id) => CartDAO.Instance.GetCarts(id);

        public void UpdateCart(CartUpdateRequest cart) => CartDAO.Instance.UpdateCart(cart);
    }
}
