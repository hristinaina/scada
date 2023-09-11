using scada.DTO;
using scada.Data;
using scada.Data.Config;
using scada.Drivers;
using scada.Models;
using scada.Repositories;
using Microsoft.AspNetCore.SignalR;
using scada.Hubs;
using Google.Protobuf.WellKnownTypes;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.Services.implementation;
using Azure;
using System.Threading;
using scada.Logging;
using scada.Services.interfaces;
using scada.Database;

namespace scada.Services
{
    public class TagProcessingService : ITagProcessingService
    {
        private readonly IHubContext<TagHub> _tagHub;
        private ITagHistoryService _tagHistoryService;
        private TagHistoryRepository _tagHistoryRepository;
        private AlarmHistoryRepository _alarmHistoryRepository;

        private AlarmLogging _alarmLogging;
        private ITagService _tagService;
        private static IDictionary<int, Thread> threads = new Dictionary<int, Thread>();

        public TagProcessingService(TagHistoryRepository tagHistoryRepository, AlarmHistoryRepository alarmHistoryRepository, ITagService tagService, ITagHistoryService tagHistoryService,IHubContext<TagHub> tagHub, AlarmLogging alarmLogging) {
            _tagHistoryRepository = tagHistoryRepository;
            _tagService = tagService;
            _tagHistoryService = tagHistoryService;
            _alarmHistoryRepository = alarmHistoryRepository;
            _tagHub = tagHub;
            _alarmLogging = alarmLogging;
        }

        private readonly object _lock = new object();

        private void saveTagValue(int tag, double value)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagHistories = dbContext.TagHistory.ToList();
                TagHistory lastTagHistory = tagHistories
                .Where(history => history.TagId == tag)
                .OrderByDescending(history => history.Timestamp)
                .FirstOrDefault();

                if (lastTagHistory != null)
                {
                    if (lastTagHistory.Value == value) return;
                }
            }

            TagHistory tagHistory = new TagHistory(tag, value);
            _tagHistoryRepository.Insert(tagHistory);
        }

        private void saveAlarm(int tagId, int alarmId)
        {
            AlarmHistory alarmHistory = new AlarmHistory(tagId, alarmId);
            _alarmHistoryRepository.Insert(alarmHistory);
        }

        public void Run()
        {
            foreach (var tag in ConfigHelper.ParseTags<AITag>(_tagService.Get()))
            { 
                Thread t;
                t = new Thread(ScanAnalog);
                threads[tag.Id] = t;
                t.Start(tag);
            }

            foreach (var tag in ConfigHelper.ParseTags<DITag>(_tagService.Get()))
            {
                Thread t;
                t = new Thread(ScanDigital);
                threads[tag.Id] = t;
                t.Start(tag);
            }

            foreach (var tag in ConfigHelper.ParseTags<DOTag>(_tagService.Get()))
            {
                RTUDriver.SetValue(tag.Address, tag.Value);
            }

            foreach (var tag in ConfigHelper.ParseTags<AOTag>(_tagService.Get()))
            {
                RTUDriver.SetValue(tag.Address, tag.Value);
            }

        }

        private void ScanAnalog(object param)
        {
            AITag tag = (AITag)param;
            double currentValue = 0;

            IDriver driver;
            if (tag.Driver == DriverEnum.SIM)
                driver = new SimulationDriver();
            else
                driver = new RTUDriver();

            while (threads.ContainsKey(tag.Id)) 
            {
                if ( ((AITag)_tagService.Get(tag.Id)).IsScanning)
                {
                    try
                    {
                        currentValue = this.calculateAnalogValue(tag, driver.GetValue(tag.Address));
                        Console.WriteLine("SCANING " + tag.TagName + " | " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    saveTagValue(tag.Id, currentValue); 
                    // dodaj u config

                    TrendingAlarmDTO alarmDTO = new TrendingAlarmDTO();
                    foreach (Alarm alarm in tag.Alarms)
                    {
                        if (alarm.Type == AlarmType.HIGH && currentValue >= alarm.Limit || alarm.Type == AlarmType.LOW && currentValue <= alarm.Limit)
                        {
                            alarmDTO.Description = alarm.Type + "ER than " + alarm.Limit;
                            alarmDTO.Priority = alarm.Priority;
                            
                            lock (_lock)
                            {
                                this.saveAlarm(tag.Id, alarm.Id);
                                this._alarmLogging.Logging(alarm, tag.TagName);
                            }

                        }
                    }

                    this.sendCurrentValue(new TrendingTagDTO(tag, currentValue, alarmDTO));
                }

                Thread.Sleep(tag.ScanTime);
            }
        }

        private void ScanDigital(object param)
        {
            DITag tag = (DITag)param;
            double currentValue = 0;

            IDriver driver;
            if (tag.Driver == DriverEnum.SIM)
                driver = new SimulationDriver();
            else
                driver = new RTUDriver();

            while (threads.ContainsKey(tag.Id))
            {
                if (((DITag)_tagService.Get(tag.Id)).IsScanning)
                {
                    try
                    {
                        currentValue = this.calculateDigitalValue(driver.GetValue(tag.Address));
                        Console.WriteLine("SCANING " + tag.TagName + " | " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    saveTagValue(tag.Id, currentValue);
                    

                    this.sendCurrentValue(new TrendingTagDTO(tag, currentValue));
                }

                Thread.Sleep(tag.ScanTime);
            }
        }

        public bool Delete(int id)
        {
            List<Tag> tagsToRemove = new List<Tag>();

            foreach (Tag tag in _tagService.Get())
            {
                if (tag.Id == id)
                {
                    _tagHistoryService.Delete(id);
                    tagsToRemove.Add(tag);

                    //delete a thread
                    if (tag is AITag || tag is DITag)
                    {
                        if (threads.ContainsKey(tag.Id))
                        {
                            threads.Remove(tag.Id);
                        }
                        
                    }
                }
            }

            if (tagsToRemove.Count == 0)
            {
                throw new NotFoundException("Tag not found!");
            }

            foreach (var tagToRemove in tagsToRemove)
            {
                _tagService.RemoveTag(tagToRemove);
            }
            return true;
        }

        public Tag Insert(TagDTO tagDTO)
        {
            List<String> addresses = RTUDriver.GetAddresses();
            Tag tag = convert(tagDTO);

            if (tag != null)
            {
                if (tagDTO.Type == "AOTag" || tagDTO.Type == "DOTag")
                {
                    if (addresses.Contains(tag.Address))
                        throw new BadRequestException("Address already in use!");

                    if (tagDTO.Type == "DOTag")
                    {
                        DOTag dotag = (DOTag)tag;
                        RTUDriver.SetValue(tag.Address, dotag.Value);
                    }
                    else if (tagDTO.Type == "AOTag")
                    {
                        AOTag aotag = (AOTag)tag;
                        RTUDriver.SetValue(tag.Address, aotag.Value);
                    }
                }
                tag.Id = generateId();
                _tagService.InsertTag(tag);
                createThread(tag, tagDTO.Type);
                return tag;
            }

            throw new BadRequestException("Invalid tag data"); ;
        }

        private void createThread(Tag tag, string type)
        {
            Thread t;
            if (type == "AITag")
            {
                t = new Thread(ScanAnalog);
                threads.Add(tag.Id, t);
                t.Start(tag);
            }

            else if (type == "DITag")
            {
                t = new Thread(ScanDigital);
                threads.Add(tag.Id, t);
                t.Start(tag);
            }
            //if the type is not input then do nothing
        }

        private async Task sendCurrentValue(TrendingTagDTO tag)
        {           
            await _tagHub.Clients.All.SendAsync("ReceiveMessage", tag);
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
            foreach (Tag tag in _tagService.Get()) if (tag.Id > id) id = tag.Id;
            return ++id;
        }
        
        private double calculateAnalogValue(AITag tag, double value)
        {
            return value > tag.HighLimit ? tag.HighLimit : (value < tag.LowLimit ? tag.LowLimit : value);
        }

        private double calculateDigitalValue(double value)
        {
            return value > 0 ? 1 : 0;
        }
    }
}
