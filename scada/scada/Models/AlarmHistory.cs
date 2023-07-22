namespace scada.Models
{
    public class AlarmHistory
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int TagId { get; set; }  // maybe this attribute is not needed 
        public Alarm Alarm { get; set; }
    }
}
