using DataAccess.Repository;
using G1FOODLibrary.DTO;
using G1FOODWebAPI.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly OrderHub orderHub;
        IOrderRepository _orderRepository;

        public OrderController(OrderHub orderHub) {
            _orderRepository = new OrderRepository();
            this.orderHub = orderHub;
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest order)
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

                await _orderRepository.AddOrderAsync(order);
                
                if(order.UserId == Guid.Empty)
                {
                    await orderHub.SendOrderCookingAsync();
                } else
                {
                    await orderHub.SendOrderPendingAsync();
                }
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
                Message = "Add order successful!"
            });
        }

        [HttpGet("getOrderPending")]
        public async Task<IActionResult> GetOrderPending()
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

                var order = await _orderRepository.GetOrderPendingAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order pending successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                var order = await _orderRepository.GetOrderCookingAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order cooking successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                var order = await _orderRepository.GetOrderDeliveringAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order pending successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {

                var order = await _orderRepository.GetOrdersAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrder")]
        public async Task<IActionResult> GetOrder(string id)
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

                var order = await _orderRepository.GetOrderAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrderHistory")]
        public async Task<IActionResult> GetOrderHistory(string id)
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

                var order = await _orderRepository.GetOrderHistoryAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order history successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("getOrderDetail")]
        public async Task<IActionResult> GetOrderDetail(string id)
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

                var order = await _orderRepository.GetOrderDetailAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get order successful!",
                    Data = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {
                await _orderRepository.UpdateOrderStatusToCookingAsync(new Guid(id));
                await orderHub.SendOrderCookingAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update cooking successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {
                await _orderRepository.UpdateOrderStatusToBlockAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update block successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {
                await _orderRepository.UpdateOrderStatusToDeliveredAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update delivered successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
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
                return Ok(new APIResponse
                {
                    StatusCode = 400,
                    Success = false,
                    Data = ModelState
                });
            }

            try
            {
                await _orderRepository.UpdateOrderStatusToDeliveringAsync(new Guid(id));
                await orderHub.SendOrderDeliveringingAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Order update delivering successful!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    StatusCode = 500,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
