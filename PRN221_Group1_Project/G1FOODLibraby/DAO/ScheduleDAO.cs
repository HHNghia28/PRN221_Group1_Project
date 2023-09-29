﻿using DataAccess.Context;
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
    }
}