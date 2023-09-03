using Google.Protobuf.WellKnownTypes;
using scada.Models;

namespace scada.DTO
{
    public class TrendingAlarmDTO
    {
        public string TagName { get; set; }

        public int Priority { get; set; }

        public string Description { get; set; }

        public TrendingAlarmDTO(string tagName, Alarm alarm) 
        {
            TagName = tagName;
            Priority = alarm.Priority;
            Description = alarm.Type + " then " + alarm.Limit;
        }
    }
}