using Google.Protobuf.WellKnownTypes;

namespace scada.Models
{
    public class AlarmHistory
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int TagId { get; set; }
        public int AlarmId { get; set; }

        public AlarmHistory(int tagId, int alarmId)
        {
            Timestamp = DateTime.Now;
            TagId = tagId;
            AlarmId = alarmId;
        }

        public override string ToString()
        {
            return $"Id: {Id}, AlarmId: {AlarmId}, TagId: {TagId}, TimeStamp: {Timestamp}";
        }
    }
}
