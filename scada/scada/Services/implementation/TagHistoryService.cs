using scada.Database;
using scada.Models;

namespace scada.Services
{
    public class TagHistoryService : ITagHistoryService
    {

        public List<TagHistory> get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagHistory = dbContext.TagHistory.ToList();
                return tagHistory;
            }
        }
    }
}
