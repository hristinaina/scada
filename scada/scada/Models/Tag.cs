namespace scada.Models
{
    public abstract class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
