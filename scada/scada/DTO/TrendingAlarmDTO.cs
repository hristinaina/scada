namespace scada.DTO
{
    public class TrendingAlarmDTO
    {
        public string Description { get; set; }
        public int Priority { get; set; }

        public TrendingAlarmDTO()
        {
            Description = "";
            Priority = 0;
        }

        public TrendingAlarmDTO(string description, int priority) 
        {
            Description = description;
            Priority = priority;
        }
    }
}