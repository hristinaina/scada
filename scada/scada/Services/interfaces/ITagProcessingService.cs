using scada.Drivers;
using scada.DTO;

namespace scada.Services
{
    public interface ITagProcessingService
    {
        public void SaveTagValue(int tag, double value);
        public void ReceiveRTUValue(RTUData rtu);
        public void Run();

    }
}
