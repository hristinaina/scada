using Microsoft.EntityFrameworkCore;
using scada.Database;
using scada.Models;

namespace scada.Repositories
{
    public class TagHistoryRepository
    {
        public List<TagHistory> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagHistory = dbContext.TagHistory.ToList();
                return tagHistory;
            }
            
        }

        public void Insert(TagHistory tagHistory)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.TagHistory.Add(tagHistory);
                dbContext.SaveChanges();
            }           
        }
    }
}
