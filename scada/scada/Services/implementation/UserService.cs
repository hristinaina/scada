using scada.Database;
using scada.Models;

namespace scada.Services
{
    public class UserService : IUserService
    {
        public List<User> get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<User> users = dbContext.Users.ToList();
                return users;
            }
        }
    }
}
