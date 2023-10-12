using DataAccess.Repository;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using G1FOODWebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        ICartRepository _cartRepository;

        public CartController() => _cartRepository = new CartRepository();

        [HttpGet("getCarts")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }
            try
            {

                var carts = _cartRepository.GetCarts(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get carts successful!",
                    Data = carts
                });

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message,
                });
            }
        }

        [HttpPost("addCarts")]
        public IActionResult Post([FromBody] List<CartRequest> carts)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                _cartRepository.AddCart(carts);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Add carts successful!"
            });
        }

        [HttpPut("updateCarts")]
        public IActionResult Put([FromBody] CartUpdateRequest cart)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                _cartRepository.UpdateCart(cart);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Update carts successful!"
            });
        }

        [HttpDelete("deleteCarts")]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                _cartRepository.DeleteCart(new Guid(id));

            }
            catch (Exception ex)
            {
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponse
            {
                StatusCode = 200,
                Success = true,
                Message = "Delete cart successful!"
            });
        }
    }
}
