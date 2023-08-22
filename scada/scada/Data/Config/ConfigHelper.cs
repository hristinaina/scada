using scada.Models;

namespace scada.Data.Config
{
    public class ConfigHelper
    {

        public static List<Tag> PopulateData()
        {
            List<Tag> tags = new List<Tag>
            {
                new DOTag { Id = 1, TagName = "DO_Tag", Value = 1 },
                new DITag { Id = 2, TagName = "DI_Tag", IsScanning = true, ScanTime = 1000, Driver = DriverEnum.SIM },
                new AOTag { Id = 3, TagName = "AO_Tag", Units = "V", LowLimit = 0, HighLimit = 10, Value = 5.5 },
                new AITag
                {
                    Id = 4,
                    TagName = "AI_Tag",
                    IsScanning = true,
                    ScanTime = 2000,
                    Driver = DriverEnum.RTU,
                    Units = "C",
                    LowLimit = -10,
                    HighLimit = 50,
                    Alarms = new List<Alarm>
                    {
                        new Alarm { Id = 1, Type = AlarmType.HIGH, Priority = 1, Border = 40 },
                        new Alarm { Id = 2, Type = AlarmType.LOW, Priority = 2, Border = -5 }
                    }
                }
            };

            return tags;
        }

        public static List<T> ParseLoadedObjects<T> (List<Tag> tags) where T : Tag
        {
            List<T> tTags = new List<T>();
            foreach (Tag tag in tags)
            {
                if (tag is T tTag)
                {
                    tTags.Add((T)tag);
                }
            }

            return tTags;
        }
    }
}
