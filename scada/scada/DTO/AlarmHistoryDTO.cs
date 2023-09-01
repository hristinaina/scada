using scada.Models;

namespace scada.DTO
{
    public class AlarmHistoryDTO
    {
        public int AlarmId { get; set; }
        public string Type { get; set; }
        public double Limit { get; set; }
        public int Priority { get; set; }
        public DateTime Date { get; set; }
        public string TagName { get; set; }

        public AlarmHistoryDTO(Alarm alarm, AlarmHistory ah, AITag aITag)
        {
            this.AlarmId = alarm.Id;
            this.Type = alarm.Type.ToString();
            this.Limit = alarm.Limit;
            this.Priority = alarm.Priority;
            this.Date = ah.Timestamp;
            this.TagName = aITag.TagName;
        }

        public override string ToString()
        {
            return $"AlarmId: {AlarmId}, Type: {Type}, TagName: {TagName}";
        }
    }
}
