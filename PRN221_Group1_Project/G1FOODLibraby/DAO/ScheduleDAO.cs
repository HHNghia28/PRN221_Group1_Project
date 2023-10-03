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
    internal class ScheduleDAO
    {
        private DBContext _context;
        private static ScheduleDAO instance = null;
        private static readonly object instanceLock = new object();

        public static ScheduleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ScheduleDAO();
                    }
                    return instance;
                }
            }
        }

        public ScheduleDAO() => _context = new DBContext();

        public async Task<IEnumerable<ScheduleResponse>> GetSchedulesAsync()
        {
            try
            {
                var schedules = await _context.Schedules.OrderByDescending(s => s.Date).ToListAsync();
                List<ScheduleResponse> results = new List<ScheduleResponse>();

                foreach (var item in schedules)
                {
                    results.Add(new ScheduleResponse
                    {
                        Id = item.Id,
                        Date = item.Date,
                        Note = item.Note
                    });
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddScheduleAsync(ScheduleRequest scheduleRequest)
        {
            try
            {
                if(scheduleRequest == null)
                {
                    throw new Exception("Schedule can not null!");
                }

                var schedules = await _context.Schedules.FirstOrDefaultAsync(s => s.Date.Day == scheduleRequest.Date.Day
                                                                            && s.Date.Month == scheduleRequest.Date.Month
                                                                            && s.Date.Year == scheduleRequest.Date.Year);

                if (schedules != null)
                {
                    throw new Exception("Schedule already exist!");
                }

                Schedule schedule = new Schedule
                {
                    Id = Guid.NewGuid(),
                    Date = scheduleRequest.Date,
                    Note = scheduleRequest.Note
                };

                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateScheduleAsync(ScheduleRequest scheduleRequest, Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (scheduleRequest == null)
                {
                    throw new Exception("Schedule can not null!");
                }

                var existSchedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);

                if (existSchedule == null)
                {
                    throw new Exception("ID not exist!");
                }

                Guid existId = existSchedule.Id;

                _context.Schedules.Remove(existSchedule);
                _context.SaveChanges();

                var schedules = await _context.Schedules.FirstOrDefaultAsync(s => s.Date.Day == scheduleRequest.Date.Day
                                                                            && s.Date.Month == scheduleRequest.Date.Month
                                                                            && s.Date.Year == scheduleRequest.Date.Year);

                if (schedules != null)
                {
                    throw new Exception("Schedule already exist!");
                }

                Schedule schedule = new Schedule
                {
                    Id = existId,
                    Date = scheduleRequest.Date,
                    Note = scheduleRequest.Note
                };

                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task AddMenuAsync(List<MenuRequest> menuRequests)
        {
            try
            {
                if (!menuRequests.Any())
                {
                    throw new ArgumentException("Menu cannot be empty!");
                }

                var menus = menuRequests.Select(item => new Menu
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ScheduleId = item.ScheduleId
                }).ToList();

                await _context.Menus.AddRangeAsync(menus);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MenuResponse>> GetMenusAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Menu ID cannot be empty!");
                }

                var menus = await _context.Menus.Where(m => m.ScheduleId == id).ToListAsync();
                List<MenuResponse> menuResponses = new List<MenuResponse>();

                foreach (var item in menus)
                {
                    menuResponses.Add(new MenuResponse
                    {
                        Id = item.Id,
                        ScheduleId = item.ScheduleId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                return menuResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<MenuResponse>> GetMenusNowAsync()
        {
            try
            {
                DateTime date = DateTime.Now;

                var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Date.Day == date.Date.Day
                                                                            && s.Date.Month == date.Date.Month
                                                                            && s.Date.Year == date.Date.Year);

                if(schedule == null)
                {
                    throw new Exception("No products available today!");
                }

                var menus = await _context.Menus.Include(m => m.Product).Where(m => m.ScheduleId == schedule.Id).ToListAsync();
                List<MenuResponse> menuResponses = new List<MenuResponse>();

                foreach (var item in menus)
                {
                    menuResponses.Add(new MenuResponse
                    {
                        Id = item.Id,
                        ScheduleId = item.ScheduleId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Name = item.Product.Name,
                        Description = item.Product.Description,
                        Image = item.Product.Image,
                        Price = item.Product.Price,
                        SalePercent = item.Product.SalePercent
                    });
                }

                return menuResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
