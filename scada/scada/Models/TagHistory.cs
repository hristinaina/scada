namespace scada.Models
{
    // should be saved to DB
    public class TagHistory
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public Tag Tag { get; set; }

        public TagHistory(Tag tag, double value) 
        {
            Tag = tag;
            Value = value;
            Timestamp = DateTime.Now;
        }
    }
}
