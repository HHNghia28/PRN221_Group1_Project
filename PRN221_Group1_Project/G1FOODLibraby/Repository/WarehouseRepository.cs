using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        public Task<IEnumerable<WarehouseResponse>> GetWarehousesAsync() => WarehouseDAO.Instance.GetWarehousesAsync();

        public Task WarehouseExportAsync(List<WarehouseExportRequest> warehouseExportRequests) => WarehouseDAO.Instance.WarehouseExportAsync(warehouseExportRequests);

        public Task<IEnumerable<WarehouseExportResponse>> WarehouseExportStatisticAsync() => WarehouseDAO.Instance.WarehouseExportStatisticAsync();

        public Task WarehouseImportAsync(List<WarehouseImportRequest> warehouseImportRequests) => WarehouseDAO.Instance.WarehouseImportAsync(warehouseImportRequests);

        public Task<IEnumerable<WarehouseImportResponse>> WarehouseImportStatisticAsync() => WarehouseDAO.Instance.WarehouseImportStatisticAsync();
    }
}
