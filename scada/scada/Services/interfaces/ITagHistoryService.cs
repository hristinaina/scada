using scada.DTO;
using scada.Models;

namespace scada.Services
{
    public interface ITagHistoryService
    {
        public List<TagHistory> Get();
        public List<TagHistoryDTO> GetByTagId(int id);
        List<TagHistoryDTO> GetLastValueOfAITags();
        List<TagHistoryDTO> GetLastValueOfDITags();
    }
}
