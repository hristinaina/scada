using scada.DTO;
using scada.Data;
using scada.Data.Config;
using scada.Drivers;
using scada.Models;
using scada.Repositories;
using scada.Services.interfaces;
using Microsoft.AspNetCore.SignalR;
using scada.WebSockets;
using Google.Protobuf.WellKnownTypes;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.Services.implementation;
using Azure;
using System.Threading;

namespace scada.Services
{

    public class TagProcessingService : ITagProcessingService
    {
        private readonly IHubContext<WebSocket> _tagHub;

        private ITagHistoryService _tagHistoryService;
        private TagHistoryRepository _tagHistoryRepository;
        private ITagService _tagService;
        private static IDictionary<int, Thread> threads = new Dictionary<int, Thread>();

        public TagProcessingService(TagHistoryRepository tagHistoryRepository, ITagService tagService, ITagHistoryService tagHistoryService, IHubContext<WebSocket> tagHub) {
            _tagHistoryRepository = tagHistoryRepository;
            _tagService = tagService;
            _tagHistoryService = tagHistoryService;
            _tagHub = tagHub;
        }

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
                        currentValue = driver.GetValue(tag.Address);
                        Console.WriteLine("SCANING " + tag.TagName + " | " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    SaveTagValue(tag.Id, currentValue); 
                    // dodaj u config

                    this.SendCurrentValue(new TrendingTagDTO(tag, currentValue));

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

            while (threads.ContainsKey(tag.Id))
            {
                if (((DITag)_tagService.Get(tag.Id)).IsScanning)
                {
                    try
                    {
                        currentValue = driver.GetValue(tag.Address);
                        Console.WriteLine("SCANING " + tag.TagName + " | " + currentValue);
                    }
                    catch (Exception ex) { continue; }

                    SaveTagValue(tag.Id, currentValue);
                    // dodaj u config

                    this.SendCurrentValue(new TrendingTagDTO(tag, currentValue));
                }

                Thread.Sleep(tag.ScanTime);
            }
        }

        public bool Delete(int id)
        {
            foreach (Tag tag in _tagService.Get())
            {
                if (tag.Id == id)
                {
                    _tagHistoryService.Delete(id);
                    _tagService.RemoveTag(tag);

                    //delete a thread
                    if (tag is AITag || tag is DITag)
                    {
                        Thread t = threads[tag.Id];
                        threads.Remove(tag.Id);
                        return true;
                    }
                }
            }
            throw new NotFoundException("Tag not found!");
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

        private async Task SendCurrentValue(TrendingTagDTO tag)
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
    }
}
