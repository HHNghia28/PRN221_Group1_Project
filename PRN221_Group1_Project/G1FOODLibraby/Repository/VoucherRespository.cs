using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class VoucherRespository : IVoucherRespository
    {
        public Task AddVoucher(VoucherRequest voucher) => VoucherDAO.Instance.AddVoucher(voucher);

        public Task<double> GetPercentVoucher(string voucherCode) => VoucherDAO.Instance.GetPercentVoucher(voucherCode);

        public Task<VoucherResponse> GetVoucher(Guid id) => VoucherDAO.Instance.GetVoucher(id);

        public Task<IEnumerable<VoucherResponse>> GetVouchers() => VoucherDAO.Instance.GetVouchers();

        public Task<IEnumerable<VoucherStatusResponse>> GetVoucherStatus() => VoucherDAO.Instance.GetVoucherStatus();

        public Task UpdateVoucher(VoucherRequest voucher, Guid id) => VoucherDAO.Instance.UpdateVoucher(voucher, id);
    }
}
