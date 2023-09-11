using Microsoft.EntityFrameworkCore;
using scada.Database;
using scada.DTO;
using scada.Models;
using scada.Repositories;
using scada.Services.implementation;

namespace scada.Services
{
    public class AlarmHistoryService : IAlarmHistoryService
    {
        private AlarmHistoryRepository _repository;

        public AlarmHistoryService() 
        { 
        }

        public AlarmHistoryService(AlarmHistoryRepository alarmHistoryRepository)
        {
            this._repository = alarmHistoryRepository;
        }

        public List<AlarmHistory> Get()
        {
            return _repository.Get();
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

            if (filter.SortingType == "priority")
                dto = dto.OrderBy(item => item.Priority).ToList();
            else if (filter.SortingType == "time")
                dto = dto.OrderByDescending(item => item.Date).ToList();

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

            dto = dto.OrderByDescending(item => item.Date).ToList();

            return dto;
        }

        public bool Delete(int id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List <AlarmHistory> alarmsToDelete = dbContext.AlarmHistory.Where(u => u.AlarmId == id).ToList();

                if (alarmsToDelete != null)
                {
                    dbContext.AlarmHistory.RemoveRange(alarmsToDelete);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
