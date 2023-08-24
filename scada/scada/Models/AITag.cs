using Microsoft.Identity.Client;

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

        public AITag() 
        {
            Units = "";
            Alarms = new List<Alarm>();
        }

        public AITag(bool isScanning, int scanTime, DriverEnum driver, 
                     string units, double lowLimit, double highLimit, List<Alarm> alarms)
        {
            IsScanning = isScanning;
            ScanTime = scanTime;
            Driver = driver;
            Units = units;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Alarms = alarms;
        }
    }
}
