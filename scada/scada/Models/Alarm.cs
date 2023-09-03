using scada.DTO;

namespace scada.Models
{
    public class Alarm
    {
        // should be loaded from CONFIG file, not DB
        public int Id { get; set; }
        public AlarmType Type { get; set; }
        public int Priority { get; set; }
        public double Limit { get; set; }

        public Alarm() { }

        public Alarm(AlarmDTO alarmDTO)
        {
            if (alarmDTO.Type == "LOW") Type = AlarmType.LOW;
            else Type = AlarmType.HIGH;
            Priority = alarmDTO.Priority;
            Limit = alarmDTO.Limit;
        }

    }


    public enum AlarmType
    {
        HIGH,
        LOW,
    }
}
