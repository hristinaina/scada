using scada.Models;

namespace scada.Services
{
    public interface IAlarmHistoryService
    {
        public List<AlarmHistory> Get();
    }
}
