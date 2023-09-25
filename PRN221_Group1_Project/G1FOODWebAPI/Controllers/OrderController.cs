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

        [HttpGet("getOrderPending")]
        public async Task<IActionResult> GetOrderPending()
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

            try
            {

                var order = await _orderRepository.GetOrderPendingAsync();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order pending successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrderCooking")]
        public async Task<IActionResult> GetOrderCooking()
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

            try
            {

                var order = await _orderRepository.GetOrderCookingAsync();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order pending successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrderDelivering")]
        public async Task<IActionResult> GetOrderDelivering()
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

            try
            {

                var order = await _orderRepository.GetOrderDeliveringAsync();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order pending successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrders")]
        public async Task<IActionResult> GetOrders()
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

            try
            {

                var order = await _orderRepository.GetOrdersAsync();

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("orderUpdateCooking")]
        public async Task<IActionResult> OrderUpdateCooking(string id)
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

            try
            {
                await _orderRepository.UpdateOrderStatusToCookingAsync(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update cooking successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("orderUpdateBlock")]
        public async Task<IActionResult> OrderUpdateBlock(string id)
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

            try
            {
                await _orderRepository.UpdateOrderStatusToBlockAsync(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update block successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("orderUpdateDelivered")]
        public async Task<IActionResult> OrderUpdateDelivered(string id)
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

            try
            {
                await _orderRepository.UpdateOrderStatusToDeliveredAsync(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update delivered successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("orderUpdateDelivering")]
        public async Task<IActionResult> OrderUpdateDelivering(string id)
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

            try
            {
                await _orderRepository.UpdateOrderStatusToDeliveringAsync(new Guid(id));

                return Ok(new APIResponseDTO
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update delivering successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponseDTO
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
