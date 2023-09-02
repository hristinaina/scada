using scada.DTO;
using scada.Data;
using scada.Data.Config;
using scada.Drivers;
using scada.Models;
using scada.Repositories;
using scada.Services.interfaces;

namespace scada.Services
{

    public class TagProcessingService : ITagProcessingService
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
                        currentValue = driver.GetValue(tag.Address);
                        Console.WriteLine("SCANING " + tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue); 
                        // dodaj u config
                    }

                    // rad sa alarmima
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
                        currentValue = driver.GetValue(tag.Address);
                        Console.WriteLine("SCANING " + tag.Id + " " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    lock (_lock)
                    {
                        SaveTagValue(tag.Id, currentValue);
                        // dodaj u config
                    }
                }

                Thread.Sleep(tag.ScanTime);
            }
        }
    }
}
