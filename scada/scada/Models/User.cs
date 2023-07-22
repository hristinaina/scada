namespace scada.Models
{
    // should be connected to DB
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }

    }

    public enum UserType
    {
        Admin,
        Client,
    }
}
