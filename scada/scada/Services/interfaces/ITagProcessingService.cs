using scada.Drivers;
using scada.Models;

namespace scada.Services
{
    public interface ITagProcessingService
    {
        public void SaveTagValue(int tag, double value);
        public void ReceiveRTUValue(RTU rtu);
    }
}
