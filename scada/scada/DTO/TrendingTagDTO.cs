using scada.Models;

namespace scada.DTO
{
    public class TrendingTagDTO
    {
        public string TagName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int ScanTime { get; set; }
        public string Range { get; set; }
        public double Value { get; set; }

        public TrendingTagDTO(DITag tag, double value) 
        {
            TagName = tag.TagName;
            Type = "digital";
            Description = tag.Description;
            ScanTime = tag.ScanTime;
            Range = null;
            Value = value;
        }

        public TrendingTagDTO(AITag tag, double value)
        {
            TagName = tag.TagName;
            Type = "analog";
            Description = tag.Description;
            ScanTime = tag.ScanTime;
            Range = "(" + tag.HighLimit + ", " + tag.LowLimit + ")";
            Value = value;
        }
    }
}