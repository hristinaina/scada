using Azure;
using scada.Database;
using scada.DTO;
using scada.Exceptions;
using scada.Models;
using scada.Services.implementation;

namespace scada.Services
{
    public class TagHistoryService : ITagHistoryService
    {

        public List<TagHistory> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagHistory = dbContext.TagHistory.ToList();
                return tagHistory;
            }
        }

        public List<TagHistoryDTO> GetByTagId(int id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                Tag tag = new TagService().Get(id);
                List<TagHistory> tagHistory = new List<TagHistory>();
                foreach (TagHistory th in dbContext.TagHistory.ToList()) if (th.TagId == id) tagHistory.Add(th);

                List<TagHistoryDTO> dto = new List<TagHistoryDTO>();
                foreach (TagHistory th in tagHistory) dto.Add(new TagHistoryDTO(tag, th));
                return dto;
            }
        }

    }
}
