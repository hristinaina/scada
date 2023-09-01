using scada.Database;
using scada.Models;
using scada.Repositories;

namespace scada.Services
{
    public class TagHistoryService : ITagHistoryService
    {
        private TagHistoryRepository _repository;

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
    }
}
