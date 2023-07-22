namespace scada.Models
{
    public class AOTag : Tag
    {
        public string Units { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public double Value { get; set; }
    }
}
