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

        public bool login(string email, string password)
        {
            List<User> users = get();
            foreach (var user in users)
            {
                if (user.Email == email && user.Password == password) return true;
            }
            return false;
        }
    }
}
