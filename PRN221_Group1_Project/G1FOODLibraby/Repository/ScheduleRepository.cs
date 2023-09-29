using DataAccess.DAO;
using G1FOODLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        public Task AddScheduleAsync(ScheduleRequest scheduleRequest) => ScheduleDAO.Instance.AddScheduleAsync(scheduleRequest);

        public Task<IEnumerable<ScheduleResponse>> GetSchedulesAsync() => ScheduleDAO.Instance.GetSchedulesAsync();

        public Task UpdateScheduleAsync(ScheduleRequest scheduleRequest, Guid id) => ScheduleDAO.Instance.UpdateScheduleAsync(scheduleRequest, id);
    }
}
