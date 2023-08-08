using scada.Models;

namespace scada.Services
{
    public interface ITagHistoryService
    {
        public List<TagHistory> Get();
    }
}
