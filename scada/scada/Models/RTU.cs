namespace scada.Models
{
    // will be created from program.cs file
    public class RTU
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }

    }
}
