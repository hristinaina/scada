using Google.Protobuf.WellKnownTypes;
using scada.Models;
using System.Runtime.CompilerServices;

namespace scada.DTO
{
    public class TrendingTagDTO
    {
        public string TagName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int ScanTime { get; set; }
        public string Range { get; set; }
        public string Value { get; set; }

        public TrendingAlarmDTO Alarm { get; set; }

        public TrendingTagDTO(DITag tag, double value) 
        {
            TagName = tag.TagName;
            Type = "digital";
            Description = tag.Description;
            ScanTime = tag.ScanTime;
            Range = "";
            Value = (value == 1) ? "on" : "off";
            Alarm = new TrendingAlarmDTO();
        }

        public TrendingTagDTO(AITag tag, double value, TrendingAlarmDTO alarm)
        {
            TagName = tag.TagName;
            Type = "analog";
            Description = tag.Description;
            ScanTime = tag.ScanTime;
            Range = "(" + tag.HighLimit + ", " + tag.LowLimit + ")";
            Value =  Math.Round(value, 2).ToString() + " " + tag.Units;
            Alarm = alarm;
        }
    }
}