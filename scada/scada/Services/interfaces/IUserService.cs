using scada.Models;

namespace scada.Services
{
    public interface IUserService
    {
        public List<User> Get();

        public bool Login(string email, string password);
    }
}
