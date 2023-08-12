using scada.Models;

namespace scada.Services.interfaces
{
    public interface ITagService
    {
        public List<Tag> Get();
        public bool Delete(int id);
    }
}
