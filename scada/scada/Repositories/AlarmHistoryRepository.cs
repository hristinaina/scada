using Microsoft.EntityFrameworkCore;
using scada.Database;
using scada.Models;

namespace scada.Repositories
{
    public class AlarmHistoryRepository
    {
        public List<AlarmHistory> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                return dbContext.AlarmHistory.ToList();
            }
            
        }

        public void Insert(AlarmHistory alarmHistory)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.AlarmHistory.Add(alarmHistory);
                dbContext.SaveChanges();
            }           
        }
    }
}
