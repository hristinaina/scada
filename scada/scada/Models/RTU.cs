namespace scada.Models
{
    //only used as pojo class, not to be saved
    public class RTU
    {
        public string Address { get; set; }
        public int Value { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

    }
}
