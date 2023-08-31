using Microsoft.EntityFrameworkCore;
using scada.Database;
using scada.DTO;
using scada.Models;
using scada.Services.implementation;

namespace scada.Services
{
    public class AlarmHistoryService : IAlarmHistoryService
    {
        public List<AlarmHistory> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var alarmHistory = dbContext.AlarmHistory.ToList();
                return alarmHistory;
            }
        }

        List<AlarmHistoryDTO> IAlarmHistoryService.GetAlarmsByTime(FilterDTO filter)
        {
            List<AlarmHistoryDTO> dto = new List<AlarmHistoryDTO>();

            using (var dbContext = new ApplicationDbContext())
            {
                List<AlarmHistory> filteredAlarmHistories = dbContext.AlarmHistory.ToList()
                .Where(ah => ah.Timestamp >= filter.StartDate && ah.Timestamp <= filter.EndDate)
                .ToList();

                foreach (AlarmHistory ah in filteredAlarmHistories)
                {
                    dto.Add(new AlarmHistoryDTO(new TagService().GetAlarmById(ah.AlarmId), ah, new TagService().GetTagByAlarmId(ah.AlarmId)));
                }
            }

            return dto;
        }

        List<AlarmHistoryDTO> IAlarmHistoryService.GetByPriority(int priority)
        {
            List<AlarmHistoryDTO> dto = new List<AlarmHistoryDTO>();

            using (var dbContext = new ApplicationDbContext())
            {
                var filteredAlarmHistories = from history in dbContext.AlarmHistory.ToList()
                             join alarm in new TagService().GetAllAlarms() on history.AlarmId equals alarm.Id
                             where alarm.Priority == priority
                             select history;

                foreach (AlarmHistory ah in filteredAlarmHistories)
                {
                    dto.Add(new AlarmHistoryDTO(new TagService().GetAlarmById(ah.AlarmId), ah, new TagService().GetTagByAlarmId(ah.AlarmId)));
                }
            }

            return dto;
        }
    }
}
