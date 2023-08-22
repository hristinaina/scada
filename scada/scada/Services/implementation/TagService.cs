using scada.Data;
using scada.Models;
using scada.Services.interfaces;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        public List<Tag> Get()
        {
            return XmlSerializationHelper.LoadFromXml<Tag>();
        }

        public Tag? Get(int id)
        {
            List<Tag> tags = Get();
            foreach (Tag tag in tags)
            {
                if (tag.Id == id) { return tag; }
            }
            return null;
        }

        public bool Delete(int id)
        {
            List<Tag> tags = Get();
            foreach (Tag tag in tags)
            {
                if (tag.Id == id) { 
                    tags.Remove(tag);
                    XmlSerializationHelper.SaveToXml(tags);
                    return true; 
                }
            }
            return false;
        }


    }
}
