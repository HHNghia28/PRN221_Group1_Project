using DataAccess.Repository;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        IWarehouseRepository _warehouseRepository;

        public WarehouseController() => _warehouseRepository = new WarehouseRepository();

        //[Authorize(Roles = "d1ddb501-e7fa-4d50-9d1b-e2713c0a3b2d")]
        [HttpGet("getWarehouses")]
        public async Task<IActionResult> GetWarehouses()
        {
            try
            {
                var warehouses = await _warehouseRepository.GetWarehousesAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get warehouses successful!",
                    Data = warehouses
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

        [HttpGet("getWarehousesImportStatistics")]
        public async Task<IActionResult> GetWarehousesImportStatistics()
        {
            try
            {
                var warehouses = await _warehouseRepository.WarehouseImportStatisticAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get warehouses import statistic successful!",
                    Data = warehouses
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

        [HttpGet("getWarehousesExportStatistics")]
        public async Task<IActionResult> GetWarehousesExportStatistics()
        {
            try
            {
                var warehouses = await _warehouseRepository.WarehouseExportStatisticAsync();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get warehouses statistics export successful!",
                    Data = warehouses
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

        [HttpPost("ImportWarehouse")]
        public async Task<IActionResult> ImportWarehouse([FromBody] List<WarehouseImportRequest> warehouseImportRequests)
        {
            try
            {
                await _warehouseRepository.WarehouseImportAsync(warehouseImportRequests);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Import warehouse successful!"
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

        [HttpPost("ExportWarehouse")]
        public async Task<IActionResult> ExportWarehouse([FromBody] List<WarehouseExportRequest> warehouseExportRequests)
        {
            try
            {
                await _warehouseRepository.WarehouseExportAsync(warehouseExportRequests);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Export warehouse successful!"
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
