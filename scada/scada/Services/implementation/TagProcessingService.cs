using Microsoft.AspNetCore.Razor.TagHelpers;
using scada.Data;
using scada.Data.Config;
using scada.Drivers;
using scada.Models;
using scada.Repositories;
using scada.Services.interfaces;

namespace scada.Services
{

    public class TagProcessingService: ITagProcessingService
    {
        private List<Tag> _tags;
        private List<AITag> _analog;
        private List<DITag> _digital;

        private TagHistoryRepository _tagHistoryRepository;

        public TagProcessingService(TagHistoryRepository tagHistoryRepository) {
            _tags = XmlSerializationHelper.LoadFromXml<Tag>();
            _analog = ConfigHelper.ParseLoadedObjects<AITag>(_tags);
            _digital = ConfigHelper.ParseLoadedObjects<DITag>(_tags);
            _tagHistoryRepository = tagHistoryRepository;
        }

        private readonly object _lock = new object();

        /*
         should be called when tag value changes:
            1. for input tags = in trending app after scanning 
            2. for output tags = when value is changed (manually)
        */
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
                if (tag.IsScanning)
                {
                    Thread t;
                    if(tag.Driver == DriverEnum.SIM)
                        t = new Thread(ScanSimulationAnalog);
                    else
                        t = new Thread(ScanRTUAnalog);
                    
                    t.Start(tag);
                }
            }

            foreach (var tag in _digital)
            {
                if (tag.IsScanning)
                {
                    Thread t;
                    if (tag.Driver == DriverEnum.SIM)
                        t = new Thread(ScanSimulationDigital);
                    else
                        t = new Thread(ScanRTUDigital);

                    t.Start(tag);
                }
            }
        }

        private void ScanSimulationAnalog(object param)
        {
            AITag tag = (AITag)param;
            double currentValue = -1000000;

            while (true) 
            {
                if (tag.IsScanning)
                {
                    try
                    {
                        currentValue = SimulationDriver.GetValue(tag.Address);
                    }catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue); 
                        // dodaj u config
                    }

                    // rad sa alarmima
                }
                else
                    break;

                Thread.Sleep(tag.ScanTime);
            }
        }

        private void ScanRTUAnalog(object param)
        {
            AITag tag = (AITag)param;
            double currentValue = -1000000;

            while (true)
            {
                if (tag.IsScanning) 
                {
                    try
                    {
                        currentValue = RTUDriver.GetValue(tag.Address);
                        Console.WriteLine(tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue); 
                        // dodaj u config
                    }
                }
                    
                else
                    break;

                Thread.Sleep(tag.ScanTime);
            }
        }

        private void ScanSimulationDigital(object param)
        {
            DITag tag = (DITag)param;
            double currentValue = -1000000;

            while (true)
            {
                if (tag.IsScanning)
                {
                    try
                    {
                        currentValue = SimulationDriver.GetValue(tag.Address);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue);
                        // dodaj u config
                    }

                    // rad sa alarmima
                }
                else
                    break;

                Thread.Sleep(tag.ScanTime);
            }
        }

        private void ScanRTUDigital(object param)
        {
            DITag tag = (DITag)param;
            double currentValue = -1000000;

            while (true)
            {
                if (tag.IsScanning)
                {
                    try
                    {
                        currentValue = RTUDriver.GetValue(tag.Address);
                        Console.WriteLine(tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue);
                        // dodaj u config
                    }
                }

                else
                    break;

                Thread.Sleep(tag.ScanTime);
            }
        }
    }
}
