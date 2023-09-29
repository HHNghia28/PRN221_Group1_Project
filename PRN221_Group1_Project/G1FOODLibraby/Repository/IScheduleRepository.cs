using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IScheduleRepository
    {
        public Task<IEnumerable<ScheduleResponse>> GetSchedulesAsync();
        public Task AddScheduleAsync(ScheduleRequest scheduleRequest);
        public Task UpdateScheduleAsync(ScheduleRequest scheduleRequest, Guid id);
        public Task<IEnumerable<MenuResponse>> GetMenusNowAsync();
        public Task<IEnumerable<MenuResponse>> GetMenusAsync(Guid id);
        public Task AddMenuAsync(List<MenuRequest> menuRequests);
    }
}
