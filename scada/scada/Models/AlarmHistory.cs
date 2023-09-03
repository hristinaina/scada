namespace scada.Models
{
    // should be saved to DB
    public class AlarmHistory
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int TagId { get; set; }  // maybe this attribute is not needed 
        public int AlarmId { get; set; }
    }
}
