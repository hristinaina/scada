using scada.Data;
using scada.Models;
using scada.Services.interfaces;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        // TODO : add property _tags to reduce number of Get() method calls
        // also add filePath variable
        // in constructor with no parameters populate _tags
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

        public Tag Insert(Tag tag)
        {
            List<Tag> tags = Get();
            tag.Id = generateId();
            tags.Add(tag);
            XmlSerializationHelper.SaveToXml(tags);
            return tag;
        }

        private int generateId()
        {
            int id = 1;
            List<Tag> tags = Get();
            foreach (Tag tag in tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }
    }
}
