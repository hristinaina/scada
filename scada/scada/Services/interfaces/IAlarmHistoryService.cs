using scada.DTO;
using scada.Models;

namespace scada.Services
{
    public interface IAlarmHistoryService
    {
        public List<AlarmHistory> Get();
        List<AlarmHistoryDTO> GetAlarmsByTime(FilterDTO filterDTO);
        List<AlarmHistoryDTO> GetByPriority(int priority);
    }
}
