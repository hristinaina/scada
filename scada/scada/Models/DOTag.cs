namespace scada.Models
{
    // should be loaded from CONFIG file, not DB
    public class DOTag : Tag
    {
        public int Value {  get; set; }  // can be either 0 or 1
    }
}
