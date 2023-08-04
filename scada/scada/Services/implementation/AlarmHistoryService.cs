﻿using Microsoft.EntityFrameworkCore;
using scada.Database;
using scada.Models;

namespace scada.Services
{
    public class AlarmHistoryService : IAlarmHistoryService
    {
        public List<AlarmHistory> get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var alarmHistory = dbContext.AlarmHistory.ToList();
                return alarmHistory;
            }
        }
    }
}
