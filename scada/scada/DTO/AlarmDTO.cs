namespace scada.DTO
{
    public class AlarmDTO
    {
        public required String Type { get; set; }
        public required int Priority { get; set; }
        public required double Limit { get; set; }
        public required int TagId { get; set; }

    }
}
