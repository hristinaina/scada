using scada.Models;

namespace scada.Services
{
    public interface IUserService
    {
        public List<User> get();

        public bool login(string email, string password);
    }
}
