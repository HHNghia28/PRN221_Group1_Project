using DataAccess.Repository;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderRepository _orderRepository;

        public OrderController() => _orderRepository = new OrderRepository();

        [HttpPost("addOrder")]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreateDTO order)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            OrderDTO orderDTO = new OrderDTO();

            try
            {

                orderDTO = await _orderRepository.AddOrderAsync(order);

            }
            catch (Exception ex)
            {
                return Ok(new APIResponseDTO
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ex.Message
                });
            }

            return Ok(new APIResponseDTO
            {
                StatusCode = 200,
                Success = true,
                Message = "Add order successful!",
                Data = orderDTO
            });
        }
    }
}
