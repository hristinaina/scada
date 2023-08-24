using scada.DTO;
using scada.Models;

namespace scada.Services.interfaces
{
    public interface ITagService
    {
        public List<Tag> Get();

        public Tag? Get(int id);

        public List<DOTag> GetDOTags();

        public List<AOTag> GetAOTags();

        public bool Delete(int id);

        public Tag Insert(TagDTO tag);

        public void SaveTagValue(int tag, double value);
        
        public void ReceiveRTUValue(RTUData rtu);
    }
}
