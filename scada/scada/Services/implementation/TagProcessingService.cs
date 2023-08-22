using scada.Drivers;
using scada.Models;

namespace scada.Services
{
  
    public class TagProcessingService : ITagProcessingService
    {
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
    }
}
