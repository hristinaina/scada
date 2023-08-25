using Azure;
using scada.Models;

namespace scada.DTO
{
    public class TagHistoryDTO
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public string Units { get; set; }
        public DateTime Date { get; set; }

        public TagHistoryDTO(Tag tag, TagHistory tagHistory)
        {
            TagId = tag.Id;
            Name = tag.TagName;
            Type = getType(tag);
            if (tagHistory != null) Value = tagHistory.Value;
            if (tagHistory != null) Date = tagHistory.Timestamp;
            if (tag is AITag aiTag) Units = aiTag.Units;
            if (tag is AOTag aoTag) Units = aoTag.Units;
        }

        private string getType(Tag tag)
        {
            if (tag is AITag aiTag) return "Analog Input";
            else if (tag is DITag diTag) return "Digital Input";
            else if (tag is AOTag aoTag) return "Analog Output";
            else if (tag is DOTag doTag) return "Digital Output";
            else return "Unknow type";
        }

        public override string ToString()
        {
            return $"AlarmId: {TagId}, Type: {Type}, TagName: {Name}";
        }

    }
}
