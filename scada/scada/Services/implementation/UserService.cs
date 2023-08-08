using scada.Database;
using scada.Models;

namespace scada.Services
{
    public class UserService : IUserService
    {
        public List<User> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<User> users = dbContext.Users.ToList();
                return users;
            }
        }

        public bool Login(string email, string password)
        {
            List<User> users = Get();
            foreach (var user in users)
            {
                if (user.Email == email && user.Password == password) return true;
            }
            return false;
        }
    }
}
