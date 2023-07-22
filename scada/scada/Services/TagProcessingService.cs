using scada.Models;

namespace scada.Services
{
    //todo add annotations
    public class TagProcessingService
    {
        /*
         should be called when tag value changes:
            1. for input tags = in trending app after scanning 
            2. for output tags = when value is changed (manually)
        */
        public void SaveTagValue(Tag tag, double value)
        {
            TagHistory tagHistory = new TagHistory(tag, value);
            //todo save tagHistory to db
        }
    }
}
