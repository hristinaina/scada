using System.Xml.Serialization;

namespace scada.Models
{
    [XmlInclude(typeof(DOTag))]
    [XmlInclude(typeof(DITag))]
    [XmlInclude(typeof(AOTag))]
    [XmlInclude(typeof(AITag))]
    public abstract class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
