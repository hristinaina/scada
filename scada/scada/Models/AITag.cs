namespace scada.Models
{
    // should be loaded from CONFIG file, not DB
    public class AITag : Tag
    {
        public bool IsScanning { get; set; }
        public int ScanTime { get; set; }   // in milliseconds ?
        public DriverEnum Driver { get; set; }
        public string Units { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public List<Alarm> Alarms { get; set; }
    }
}
