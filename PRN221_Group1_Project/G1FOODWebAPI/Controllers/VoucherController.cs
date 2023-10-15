using DataAccess.Repository;
using G1FOODLibrary.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace G1FOODWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        IVoucherRespository _voucherRespository;

        public VoucherController() => _voucherRespository = new VoucherRespository();

        [HttpGet("getPercentVoucher")]
        public async Task<IActionResult> GetPercentVoucher(string voucherCode)
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
                double percent = await _voucherRespository.GetPercentVoucher(voucherCode);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get percent voucher successful!",
                    Data = percent
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

        [HttpGet("getVouchers")]
        public async Task<IActionResult> GetVouchers()
        {
            try
            {
                var voucher = await _voucherRespository.GetVouchers();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get vouchers successful!",
                    Data = voucher
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

        [HttpGet("getVoucher")]
        public async Task<IActionResult> GetVoucher(string id)
        {
            try
            {
                var voucher = await _voucherRespository.GetVoucher(new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get voucher successful!",
                    Data = voucher
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

        [HttpGet("getVouchersStatus")]
        public async Task<IActionResult> GetVouchersStatus()
        {
            try
            {
                var voucher = await _voucherRespository.GetVoucherStatus();

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Get vouchers status successful!",
                    Data = voucher
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

        [HttpPost("addVoucher")]
        public async Task<IActionResult> AddVoucher([FromBody] VoucherRequest voucherResquest)
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
                await _voucherRespository.AddVoucher(voucherResquest);

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Add voucher successful!"
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

        [HttpPut("updateVoucher")]
        public async Task<IActionResult> UpdateVoucher([FromBody] VoucherRequest voucherResquest, string id)
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
                await _voucherRespository.UpdateVoucher(voucherResquest, new Guid(id));

                return Ok(new APIResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message = "Update voucher successful!"
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
