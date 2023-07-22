namespace scada.Models
{
    public class DITag : Tag
    {
        public bool IsScanning { get; set; }
        public int ScanTime { get; set; }   // in milliseconds ?
        public DriverEnum Driver { get; set; }

    }
}
