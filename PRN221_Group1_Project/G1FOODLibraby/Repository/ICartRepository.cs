using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICartRepository
    {
        public IEnumerable<CartResponse> GetCarts(Guid id);
        public void AddCart(List<CartRequest> carts);
        public void UpdateCart(CartUpdateRequest cart);
        public void DeleteCart(Guid id);
    }
}
