using scada.Database;
using scada.DTO;
using scada.Models;
using scada.Services.implementation;
using scada.Repositories;

namespace scada.Services
{
    public class TagHistoryService : ITagHistoryService
    {
        private TagHistoryRepository _repository;

        public TagHistoryService()
        {

        }

        public TagHistoryService(TagHistoryRepository tagHistoryRepository)
        {
            this._repository = tagHistoryRepository;
        }

        public List<TagHistory> Get()
        {
            return _repository.Get();
        }

        public void Insert(TagHistory tagHistory)
        {
            _repository.Insert(tagHistory);
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
                dto = dto.OrderBy(item => item.Value).ToList();
                return dto;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetLastValueOfAITags()
        {
            List<AITag> tags = new TagService().GetAITags();

            using (var dbContext = new ApplicationDbContext())
            {
                var result = tags.GroupJoin(
                        dbContext.TagHistory.ToList(),
                        aitag => aitag.Id,
                        tagHistory => tagHistory.TagId,
                        (aitag, tagHistoryGroup) => new
                        {
                            AITag = aitag,
                            LastTagHistory = tagHistoryGroup
                                .OrderBy(th => th.Timestamp)
                                .LastOrDefault() // Get the last TagHistory with the least Timestamp value
                        })
                    .Select(result => new TagHistoryDTO(result.AITag, result.LastTagHistory))
                    .ToList();
                result = result.OrderByDescending(item => item.Date).ToList();
                return result;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetLastValueOfDITags()
        {
            List<DITag> tags = new TagService().GetDITags();

            using (var dbContext = new ApplicationDbContext())
            {
                var result = tags.GroupJoin(
                        dbContext.TagHistory.ToList(),
                        ditag => ditag.Id,
                        tagHistory => tagHistory.TagId,
                        (ditag, tagHistoryGroup) => new
                        {
                            DITag = ditag,
                            LastTagHistory = tagHistoryGroup
                                .OrderBy(th => th.Timestamp)
                                .LastOrDefault() // Get the last TagHistory with the least Timestamp value
                        })
                    .Select(result => new TagHistoryDTO(result.DITag, result.LastTagHistory))
                    .ToList();
                result = result.OrderByDescending(item => item.Date).ToList();
                return result;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetTagsByTime(FilterDTO filter)
        {
            List<TagHistoryDTO> dto = new List<TagHistoryDTO>();

            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> filteredTagHistories = dbContext.TagHistory.ToList()
                .Where(th => th.Timestamp >= filter.StartDate && th.Timestamp <= filter.EndDate)
                .ToList();

                foreach (TagHistory th in filteredTagHistories)
                {
                    dto.Add(new TagHistoryDTO(new TagService().Get(th.TagId), th));
                }
            }
            dto = dto.OrderByDescending(item => item.Date).ToList();
            return dto;
        }

        public bool Delete(int id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagsToDelete = dbContext.TagHistory.Where(u => u.TagId == id).ToList();

                if (tagsToDelete != null)
                {
                    dbContext.TagHistory.RemoveRange(tagsToDelete);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
