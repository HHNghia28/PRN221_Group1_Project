using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IVoucherRespository
    {
        public Task<double> GetPercentVoucher(string voucherCode);
        public Task<IEnumerable<VoucherResponse>> GetVouchers();
        public Task AddVoucher(VoucherRequest voucher);
        public Task UpdateVoucher(VoucherRequest voucher, Guid id);
        public Task<VoucherResponse> GetVoucher(Guid id);
        public Task<IEnumerable<VoucherStatusResponse>> GetVoucherStatus();
    }
}
