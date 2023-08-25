using scada.Database;
using scada.Models;
using System.Reflection.Metadata.Ecma335;

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
