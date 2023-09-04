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

        private readonly object _lock = new object();

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
            lock (_lock)
            {
                return _tags;
            }
        }

        public Tag? Get(int id)
        {
            foreach (Tag tag in _tags) if (tag.Id == id)  return tag; 
            throw new NotFoundException("Tag not found!");
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();

            foreach (Tag tag in _tags) { 
                if (tag is AITag)
                {
                    AITag aitag = (AITag) tag;
                    alarms.AddRange(aitag.Alarms);
                }
            }

            return alarms;
        }

        public Alarm GetAlarmById(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag is AITag)
                {
                    AITag aitag = (AITag)tag;
                    Alarm alarm = aitag.Alarms.FirstOrDefault(item => item.Id == id);
                    if (alarm != null) return alarm;
                }
            }
            throw new NotFoundException("Alarm not found!");
        }

        public AITag GetTagByAlarmId(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag is AITag)
                {
                    AITag aitag = (AITag)tag;
                    Alarm alarm = aitag.Alarms.FirstOrDefault(item => item.Id == id);
                    if (alarm != null) return aitag;
                }
            }
            throw new NotFoundException("Tag not found!");
        }

        public List<DOTag> GetDOTags() 
        {
            return ConfigHelper.ParseTags<DOTag>(_tags); 
        }

        public List<AOTag> GetAOTags()
        {
            return ConfigHelper.ParseTags<AOTag>(_tags);
        }

        public List<DITag> GetDITags()
        {
            return ConfigHelper.ParseTags<DITag>(_tags);
        }

        public List<AITag> GetAITags()
        {
            return ConfigHelper.ParseTags<AITag>(_tags);
        }

        public void ReceiveRTUValue(RTUData rtu)
        {
            RTUDriver.SetValue(rtu.Address, rtu.Value);
        }


        public void RemoveTag(Tag tag)
        {
            lock (_lock)
            {
                _tags.Remove(tag);
            }
            XmlSerializationHelper.SaveToXml(_tags);
        }

        public void InsertTag(Tag tag){
            lock (_lock)
            {
                _tags.Add(tag);
            }
            XmlSerializationHelper.SaveToXml(_tags);  
        }
    }
}
