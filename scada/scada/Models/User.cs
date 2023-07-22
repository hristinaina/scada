namespace scada.Models
{
    // should be saved to DB
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }

    public enum Role
    {
        Admin,
        Client,
    }
}
