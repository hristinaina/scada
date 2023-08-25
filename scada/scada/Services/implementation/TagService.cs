using scada.Data;
using scada.Models;
using scada.Services.interfaces;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.DTO;
using scada.Drivers;
using scada.Data.Config;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        private List<Tag> _tags;

        public TagService() 
        {
            _tags = Load();    
        }

        private List<Tag> Load()
        {
            return XmlSerializationHelper.LoadFromXml<Tag>();
        }

        public List<Tag> Get()
        {
            return _tags;
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

        public Tag Insert(TagDTO tagDTO)
        {
            Tag tag = convert(tagDTO);

            if (tag != null)
            {
                tag.Id = generateId();
                _tags.Add(tag);
                XmlSerializationHelper.SaveToXml(_tags);
                return tag;
            }

            throw new BadRequestException("Invalid tag data"); ;
        }

        private Tag convert(TagDTO tagDTO)
        {
            return tagDTO.Type switch
            {
                "DOTag" => JsonConvert.DeserializeObject<DOTag>(tagDTO.Data.ToString()),
                "DITag" => JsonConvert.DeserializeObject<DITag>(tagDTO.Data.ToString()),
                "AOTag" => JsonConvert.DeserializeObject<AOTag>(tagDTO.Data.ToString()),
                "AITag" => JsonConvert.DeserializeObject<AITag>(tagDTO.Data.ToString()),
                _ => null // handle unknown types
            };
        }

        private int generateId()
        {
            int id = 1;
            foreach (Tag tag in _tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }

        /*
         should be called when tag value changes:
            1. for input tags = in trending app after scanning 
            2. for output tags = when value is changed (manually)
        */
        public void SaveTagValue(int tag, double value)
        {
            TagHistory tagHistory = new TagHistory(tag, value);
            //todo save tagHistory to db
        }

        public void ReceiveRTUValue(RTUData rtu)
        {
            RTUDriver.SetValue(rtu.Address, rtu.Value);
        }

        public List<DITag> GetDITags()
        {
            return ConfigHelper.ParseLoadedObjects<DITag>(_tags);
        }

        public List<AITag> GetAITags()
        {
            return ConfigHelper.ParseLoadedObjects<AITag>(_tags);
        }
    }
}
