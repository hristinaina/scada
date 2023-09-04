using scada.DTO;
using scada.Data;
using scada.Data.Config;
using scada.Drivers;
using scada.Models;
using scada.Repositories;
using scada.Services.interfaces;
using Microsoft.AspNetCore.SignalR;
using scada.Hubs;
using Google.Protobuf.WellKnownTypes;

namespace scada.Services
{

    public class TagProcessingService : ITagProcessingService
    {
        private List<Tag> _tags;
        private List<AITag> _analog;
        private List<DITag> _digital;
        private readonly IHubContext<TagHub> _tagHub;

        private TagHistoryRepository _tagHistoryRepository;

        public TagProcessingService(TagHistoryRepository tagHistoryRepository, IHubContext<TagHub> tagHub)
        {
            _tags = XmlSerializationHelper.LoadFromXml<Tag>();
            _analog = ConfigHelper.ParseTags<AITag>(_tags);
            _digital = ConfigHelper.ParseTags<DITag>(_tags);
            _tagHistoryRepository = tagHistoryRepository;
            _tagHub = tagHub;
        }

        private readonly object _lock = new object();

        public void SaveTagValue(int tag, double value)
        {
            TagHistory tagHistory = new TagHistory(tag, value);
            _tagHistoryRepository.Insert(tagHistory);
        }

        public void ReceiveRTUValue(RTUData rtu)
        {
            RTUDriver.SetValue(rtu.Address, rtu.Value);
        }

        public void Run()
        {
            foreach (var tag in _analog) 
            { 
                    Thread t;
                    t = new Thread(ScanAnalog);
                    t.Start(tag);
            }

            foreach (var tag in _digital)
            {
                    Thread t;
                    t = new Thread(ScanDigital);
                    t.Start(tag);
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

            while (true) 
            {
                if (tag.IsScanning)
                {
                    try
                    {
                        currentValue = this.calculateAnalogValue(tag, driver.GetValue(tag.Address));
                        Console.WriteLine("SCANING " + tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue); 
                    }

                    TrendingAlarmDTO alarmDTO = new TrendingAlarmDTO();
                    foreach (Alarm alarm in tag.Alarms)
                    {
                        if (alarm.Type == AlarmType.HIGH && currentValue >= alarm.Limit || alarm.Type == AlarmType.LOW && currentValue <= alarm.Limit)
                        {
                            alarmDTO.Description = alarm.Type + " then " + alarm.Limit;
                            alarmDTO.Priority = alarm.Priority;
                            // TODO dodaj u bazu
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

            while (true)
            {
                if (tag.IsScanning)
                {
                    try
                    {
                        currentValue = this.calculateDigitalValue(driver.GetValue(tag.Address));
                        Console.WriteLine("SCANING " + tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue); 
                    }

                    this.sendCurrentValue(new TrendingTagDTO(tag, currentValue));
                }

                Thread.Sleep(tag.ScanTime);
            }
        }

        private async Task sendCurrentValue(TrendingTagDTO tag)
        {           
            await _tagHub.Clients.All.SendAsync("ReceiveMessage", tag);
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
