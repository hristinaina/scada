namespace scada.Models
{
    // should be loaded from CONFIG file, not DB
    public class DITag : Tag
    {
        public bool IsScanning { get; set; }
        public int ScanTime { get; set; }   // in milliseconds ?
        public DriverEnum Driver { get; set; }

    }
}
