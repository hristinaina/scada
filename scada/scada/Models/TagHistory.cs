namespace scada.Models
{
    // should be saved to DB
    public class TagHistory
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public int TagId { get; set; }

        public TagHistory(int tagId, double value) 
        {
            TagId = tagId;
            Value = value;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Value: {Value}, TagId: {TagId}, TimeStamp: {Timestamp}";
        }
    }
}
