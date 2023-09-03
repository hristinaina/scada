namespace scada.Models
{
    public class Alarm
    {
        // should be loaded from CONFIG file, not DB
        public int Id { get; set; }
        public AlarmType Type { get; set; }
        public int Priority { get; set; }
        public double Limit { get; set; }

    }


    public enum AlarmType
    {
        HIGH,
        LOW,
    }
}
