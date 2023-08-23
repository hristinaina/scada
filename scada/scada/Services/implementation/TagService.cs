using scada.Data;
using scada.Models;
using scada.Services.interfaces;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.DTO;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
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
            foreach (Tag tag in _tags) if (tag.Id == id)  return tag; 
            throw new NotFoundException("Tag not found!");
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
            throw new NotFoundException("Tag not found!");
        }

        public Tag Insert(TagDTO tagInput)
        {
            Tag tag = tagInput.Type switch
            {
                "DOTag" => JsonConvert.DeserializeObject<DOTag>(tagInput.Data.ToString()),
                "DITag" => JsonConvert.DeserializeObject<DITag>(tagInput.Data.ToString()),
                "AOTag" => JsonConvert.DeserializeObject<AOTag>(tagInput.Data.ToString()),
                "AITag" => JsonConvert.DeserializeObject<AITag>(tagInput.Data.ToString()),
                _ => null // handle unknown types
            };

            if (tag != null)
            {
                tag.Id = generateId();
                _tags.Add(tag);
                XmlSerializationHelper.SaveToXml(_tags);
                return tag;
            }

            throw new BadRequestException("Invalid tag data"); ;
        }

        private int generateId()
        {
            int id = 1;
            foreach (Tag tag in _tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }
    }
}
