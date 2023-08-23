using scada.Data;
using scada.Models;
using scada.Services.interfaces;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        // TODO : create exception classes
        private List<Tag> _tags;

        public TagService() 
        {
            _tags = Get();    
        }
        public List<Tag> Get()
        {
            return XmlSerializationHelper.LoadFromXml<Tag>();
        }

        public Tag? Get(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag.Id == id) { return tag; }
            }
            return null;
        }

        public bool Delete(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag.Id == id) { 
                    _tags.Remove(tag);
                    XmlSerializationHelper.SaveToXml(_tags);
                    return true; 
                }
            }
            return false;
        }

        public Tag Insert(Tag tag)
        {
            tag.Id = generateId();
            _tags.Add(tag);
            XmlSerializationHelper.SaveToXml(_tags);
            return tag;
        }

        private int generateId()
        {
            int id = 1;
            foreach (Tag tag in _tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }
    }
}
