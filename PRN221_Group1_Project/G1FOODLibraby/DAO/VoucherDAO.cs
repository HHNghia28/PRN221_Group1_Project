using DataAccess.Context;
using G1FOODLibrary.DTO;
using G1FOODLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    internal class VoucherDAO
    {
        private DBContext _context;
        private static VoucherDAO instance = null;
        private static readonly object instanceLock = new object();

        public static VoucherDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VoucherDAO();
                    }
                    return instance;
                }
            }
        }

        public VoucherDAO() => _context = new DBContext();

        public async Task<double> GetPercentVoucher(string voucherCode)
        {
            try
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code.ToLower().Equals(voucherCode.ToLower()));

                if (voucher == null)
                {
                    throw new Exception("Voucher not exist!");
                }

                if (voucher.StatusId != new Guid("DF9DC864-4ABD-4277-9ECD-751814C8763A"))
                {
                    throw new Exception("Voucher expired!");
                }

                if (voucher.Quantity < 1)
                {
                    throw new Exception("Vouchers are out of stock");
                }

                return voucher.SalePercent;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<VoucherResponse>> GetVouchers()
        {
            try
            {
                var vouchers = await _context.Vouchers.Include(v => v.Status).ToListAsync();
                List<VoucherResponse> vouchersResponse = new List<VoucherResponse>();

                foreach (var item in vouchers)
                {
                    vouchersResponse.Add(new VoucherResponse
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        SalePercent = item.SalePercent,
                        Status = item.Status.Name
                    });
                }

                return vouchersResponse;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VoucherResponse> GetVoucher(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            try
            {
                var voucher = await _context.Vouchers.Include(v => v.Status).FirstOrDefaultAsync(v => v.Id == id);

                VoucherResponse voucherResponse = new VoucherResponse
                {
                    Id = voucher.Id,
                    Code = voucher.Code,
                    Description = voucher.Description,
                    Quantity = voucher.Quantity,
                    SalePercent = voucher.SalePercent,
                    Status = voucher.Status.Name,
                    StatusId = voucher.StatusId
                };

                return voucherResponse;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddVoucher(VoucherRequest voucher)
        {
            try
            {
                var existVoucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code.ToLower().Equals(voucher.Code.ToLower()));

                if (existVoucher != null)
                {
                    throw new Exception("Voucher already exist!");
                }

                Voucher newVoucher = new Voucher
                {
                    Id = Guid.NewGuid(),
                    Code = voucher.Code,
                    Description = voucher.Description,
                    Quantity = voucher.Quantity,
                    SalePercent = voucher.SalePercent,
                    StatusId = voucher.StatusId
                };

                _context.Vouchers.Add(newVoucher);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateVoucher(VoucherRequest voucher, Guid id)
        {
            try
            {
                var existVoucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Id == id);

                if (existVoucher == null)
                {
                    throw new Exception("Voucher not exist!");
                }

                existVoucher.Quantity = voucher.Quantity;
                existVoucher.StatusId = voucher.StatusId;
                existVoucher.SalePercent = voucher.SalePercent;
                existVoucher.Description = voucher.Description;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<VoucherStatusResponse>> GetVoucherStatus()
        {
            try
            {
                var status = await _context.VoucherStatuses.ToListAsync();
                List<VoucherStatusResponse> voucherStatusResponses = new List<VoucherStatusResponse>();

                foreach (var item in status)
                {
                    voucherStatusResponses.Add(new VoucherStatusResponse
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name
                    });
                }

                return voucherStatusResponses;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
