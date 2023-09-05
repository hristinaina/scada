using scada.DTO;
using scada.Models;

namespace scada.Services.interfaces
{
    public interface ITagService
    {
        public List<Tag> Get();

        public List<DITag> GetDITags();

        public List<AITag> GetAITags();

        public Tag? Get(int id);

        public List<DOTag> GetDOTags();

        public List<AOTag> GetAOTags();

        public void ReceiveRTUValue(RTUData rtu);

        public List<Alarm> GetAllAlarms();

        public Alarm GetAlarmById(int id);

        public AITag GetTagByAlarmId(int id);

        public void RemoveTag(Tag tag);

        void InsertTag(Tag tag);
        void ChangeScan(int id);
        void EditTag(EditTagDTO th);
    }
}
