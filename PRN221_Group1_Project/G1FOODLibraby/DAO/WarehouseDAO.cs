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
    internal class WarehouseDAO
    {
        private DBContext _context;
        private static WarehouseDAO instance = null;
        private static readonly object instanceLock = new object();

        public static WarehouseDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new WarehouseDAO();
                    }
                    return instance;
                }
            }
        }

        public WarehouseDAO() => _context = new DBContext();

        public async Task<IEnumerable<WarehouseResponse>> GetWarehousesAsync()
        {
            try
            {
                List<WarehouseResponse> warehouseResponses = new List<WarehouseResponse>();
                var warehouses = await _context.Warehouses.Include(w => w.WarehouseItem).ToListAsync();

                foreach (var item in warehouses)
                {
                    warehouseResponses.Add(new WarehouseResponse
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        ItemName = item.WarehouseItem.Name,
                        WarehouseItemId = item.WarehouseItemId
                    });
                }

                return warehouseResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task WarehouseImportAsync(List<WarehouseImportRequest> warehouseImportRequests)
        {
            if (warehouseImportRequests.Count == 0)
            {
                throw new ArgumentNullException("Warehouse import cannot be null.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in warehouseImportRequests)
                {

                    var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseItemId == item.WarehouseItemId);

                    if (warehouse == null)
                    {
                        throw new Exception("Warehouse not found.");
                    }

                    Guid newId = Guid.NewGuid();
                    WarehouseImport warehouseImport = new WarehouseImport
                    {
                        Id = newId,
                        Date = DateTime.Now,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        WarehouseId = warehouse.Id
                    };

                    await _context.WarehouseImports.AddAsync(warehouseImport);

                    warehouse.Quantity += item.Quantity;

                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public async Task WarehouseExportAsync(List<WarehouseExportRequest> warehouseExportRequests)
        {
            if (warehouseExportRequests.Count == 0)
            {
                throw new ArgumentNullException("Warehouse export can not null!");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in warehouseExportRequests)
                {
                    var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseItemId == item.WarehouseItemId);

                    if (warehouse == null)
                    {
                        throw new ArgumentNullException("Warehouse not found!");
                    }

                    WarehouseExport warehouseExport = new WarehouseExport
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        Quantity = item.Quantity,
                        WarehouseId = warehouse.Id
                    };

                    await _context.WarehouseExports.AddAsync(warehouseExport);

                    if (warehouse.Quantity < item.Quantity)
                    {
                        throw new Exception("Insufficient inventory!");
                    }

                    warehouse.Quantity -= item.Quantity;

                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<WarehouseImportResponse>> WarehouseImportStatisticAsync()
        {
            try
            {
                var warehouseImport = await _context.WarehouseImports.Include(w => w.Warehouse).ThenInclude(w => w.WarehouseItem).OrderByDescending(w => w.Date).ToListAsync();

                List<WarehouseImportResponse> warehouseImportResponses = new List<WarehouseImportResponse>();

                foreach (var item in warehouseImport)
                {
                    warehouseImportResponses.Add(new WarehouseImportResponse
                    {
                        Id = item.Id,
                        Date = item.Date,
                        Quantity = item.Quantity,
                        WarehouseItem = item.Warehouse.WarehouseItem.Name
                    });
                }

                return warehouseImportResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<WarehouseExportResponse>> WarehouseExportStatisticAsync()
        {
            try
            {
                var warehouseExport = await _context.WarehouseExports.Include(w => w.Warehouse).ThenInclude(w => w.WarehouseItem).OrderByDescending(w => w.Date).ToListAsync();

                List<WarehouseExportResponse> warehouseExportResponses = new List<WarehouseExportResponse>();

                foreach (var item in warehouseExport)
                {
                    warehouseExportResponses.Add(new WarehouseExportResponse
                    {
                        Id = item.Id,
                        Date = item.Date,
                        Quantity = item.Quantity,
                        WarehouseItem = item.Warehouse.WarehouseItem.Name
                    });
                }

                return warehouseExportResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
