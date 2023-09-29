using DataAccess.Repository;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        IScheduleRepository _scheduleRepository;

        public ScheduleController() => _scheduleRepository = new ScheduleRepository();

        [HttpGet("getSchedules")]
        public async Task<IActionResult> GetSchedules()
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

                var schedules = await _scheduleRepository.GetSchedulesAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get schedules successful!",
                    Data = schedules
                });
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
        }

        [HttpPost("addSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] ScheduleRequest scheduleRequest)
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

                await _scheduleRepository.AddScheduleAsync(scheduleRequest);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add schedules successful!"
                });
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
        }

        [HttpPut("updateSchedule")]
        public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleRequest scheduleRequest, string id)
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

                await _scheduleRepository.UpdateScheduleAsync(scheduleRequest, new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update schedules successful!"
                });
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
        }

        [HttpPost("addMenu")]
        public async Task<IActionResult> AddMenu([FromBody] List<MenuRequest> menus)
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

                await _scheduleRepository.AddMenuAsync(menus);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add menu successful!"
                });
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
        }

        [HttpGet("getMenu")]
        public async Task<IActionResult> GetMenu(string id)
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

                var menu = await _scheduleRepository.GetMenusAsync(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get menu successful!",
                    Data = menu
                });
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
        }

        [HttpGet("getMenuNow")]
        public async Task<IActionResult> GetMenuNow()
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

                var menu = await _scheduleRepository.GetMenusNowAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get menu successful!",
                    Data = menu
                });
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
        }
    }
}
