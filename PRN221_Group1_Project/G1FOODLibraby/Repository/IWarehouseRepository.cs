using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IWarehouseRepository
    {
        public Task<IEnumerable<WarehouseResponse>> GetWarehousesAsync();
        public Task WarehouseImportAsync(List<WarehouseImportRequest> warehouseImportRequests);
        public Task WarehouseExportAsync(List<WarehouseExportRequest> warehouseExportRequests);
        public Task<IEnumerable<WarehouseImportResponse>> WarehouseImportStatisticAsync();
        public Task<IEnumerable<WarehouseExportResponse>> WarehouseExportStatisticAsync();
    }
}
