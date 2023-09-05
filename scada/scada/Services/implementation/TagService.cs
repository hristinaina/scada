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
        private ITagHistoryService _tagHistoryService = new TagHistoryService();
        private IAlarmHistoryService _alarmHistoryService = new AlarmHistoryService();

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

        public bool Delete(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag.Id == id) { 
                    _tags.Remove(tag);
                    _tagHistoryService.Delete(id);
                    XmlSerializationHelper.SaveToXml(_tags);
                    return true; 
                }
            }
            throw new NotFoundException("Tag not found!");
        }

        public Tag Insert(TagDTO tagDTO)
        {
            List<String> addresses = getAllAddresses();
            Tag tag = convert(tagDTO);

            if (tag != null)
            {
                if (tagDTO.Type == "AOTag" || tagDTO.Type == "DOTag")
                {
                    if (addresses.Contains(tag.Address))
                        throw new BadRequestException("Address already in use!");
                }
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
            int id = 0;
            foreach (Tag tag in _tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }

        private List<String> getAllAddresses()
        {
            List<String> addresses = new List<String>();
            addresses.AddRange(new[] { "a1", "a2", "a3", "a4", "a5",
                                       "d1", "d2", "d3", "d4", "d5"});
            return addresses;
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

        public Alarm InsertAlarm(AlarmDTO alarmDTO)
        {
            Alarm alarm = new Alarm(alarmDTO);
            alarm.Id = generateAlarmId(alarmDTO.TagId);
            AITag aiTag = GetAITags().FirstOrDefault(item => item.Id == alarmDTO.TagId);
            if (isAlarmAdded(aiTag, alarm.Type)) throw new BadRequestException("Alarm already added.");
            if (!checkAlarmLimit(aiTag, alarm)) throw new BadRequestException("Invalid data!");
            aiTag.Alarms.Add(alarm);
            Delete(aiTag.Id);
            _tags.Add(aiTag);
            XmlSerializationHelper.SaveToXml(_tags);
            return alarm;
        }

        private bool isAlarmAdded(AITag aiTag, AlarmType type)
        {
            foreach (Alarm alarm in aiTag.Alarms) if (alarm.Type == type) return true;
            return false;
        }

        private bool checkAlarmLimit(AITag aiTag, Alarm alarm)
        {
            bool isLow = alarm.Type == AlarmType.LOW;

            foreach (Alarm a in aiTag.Alarms)
            {
                if ((isLow && a.Type == AlarmType.HIGH && a.Limit < alarm.Limit) ||
                    (!isLow && a.Type == AlarmType.LOW && a.Limit > alarm.Limit))
                {
                    return false;
                }
            }

            return true;
        }

        private int generateAlarmId(int tagId)
        {
            List<Alarm> alarms = GetAllAlarms();
            int id = 0;
            foreach (Alarm alarm in alarms) if (alarm.Id > id) id = alarm.Id;
            return ++id;
        }

        public bool DeleteAlarm(int alarmId)
        {
            AITag aiTag = GetTagByAlarmId(alarmId);
            if (aiTag == null) throw new NotFoundException("Tag with specified alarm not found!");
            
            foreach (Alarm alarm in aiTag.Alarms) 
            { 
                if (alarm.Id == alarmId) 
                { 
                    aiTag.Alarms.Remove(alarm);
                    Delete(aiTag.Id);
                    _tags.Add(aiTag);
                    _alarmHistoryService.Delete(alarmId);
                    XmlSerializationHelper.SaveToXml(_tags); 
                    return true;
                } 
            }
            throw new NotFoundException("Alarm not found!");
        }

        public List<Alarm> GetAlarmsByTagId(int id)
        {
            AITag aiTag = GetAITags().FirstOrDefault(item => item.Id == id);
            if (aiTag == null) throw new NotFoundException("Tag not found!");
            return aiTag.Alarms;
        }
    }
}
