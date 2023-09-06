using scada.Data;
using scada.Models;
using scada.Services.interfaces;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.DTO;
using scada.Drivers;
using scada.Data.Config;
using Azure;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        private static List<Tag> _tags;

        private readonly object _lock = new object();

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

        public void ReceiveRTUValue(RTUData rtu)
        {
            RTUDriver.SetValue(rtu.Address, rtu.Value);
        }


        public void RemoveTag(Tag tag)
        {
            _tags.Remove(tag);
            XmlSerializationHelper.SaveToXml(_tags);
        }

        public void InsertTag(Tag tag)
        {
            _tags.Add(tag);
            XmlSerializationHelper.SaveToXml(_tags);  
        }

        public void ChangeScan(int id)
        {
            Tag tag = Get(id);
            if (tag is AITag) 
            {
                AITag aitag = (AITag)tag;
                aitag.IsScanning = !aitag.IsScanning;
                _tags.Remove(aitag);
                _tags.Add(aitag);
            }
            else if (tag is DITag)
            {
                DITag ditag = (DITag)tag;
                ditag.IsScanning = !ditag.IsScanning;
                _tags.Remove(ditag);
                _tags.Add(ditag);
            }
            XmlSerializationHelper.SaveToXml(_tags);
        }

        public void EditTag(EditTagDTO th)
        {
            Tag tag = Get(th.TagId);
            if (tag is AOTag)
            {
                AOTag aotag = (AOTag)tag;
                aotag.Value = th.Value;
                _tags.Remove(aotag);
                _tags.Add(aotag);
            }
            else if (tag is DOTag)
            {
                DOTag dotag = (DOTag)tag;
                dotag.Value = (int)th.Value;
                _tags.Remove(dotag);
                _tags.Add(dotag);
            }
            XmlSerializationHelper.SaveToXml(_tags);
            RTUDriver.SetValue(tag.Address, th.Value);
        }

        public Alarm InsertAlarm(AlarmDTO alarmDTO)
        {
            Alarm alarm = new Alarm(alarmDTO);
            alarm.Id = generateAlarmId(alarmDTO.TagId);
            AITag aiTag = GetAITags().FirstOrDefault(item => item.Id == alarmDTO.TagId);
            if (isAlarmAdded(aiTag, alarm.Type)) throw new BadRequestException("Alarm already added.");
            if (!checkAlarmLimit(aiTag, alarm)) throw new BadRequestException("Invalid data!");
            _tags.Remove(aiTag);
            aiTag.Alarms.Add(alarm);
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
                    _tags.Remove(aiTag);
                    aiTag.Alarms.Remove(alarm);
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
