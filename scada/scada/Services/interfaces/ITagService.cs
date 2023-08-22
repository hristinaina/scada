using scada.Models;

namespace scada.Services.interfaces
{
    public interface ITagService
    {
        public List<Tag> Get();

        public Tag? Get(int id);

        public bool Delete(int id);

        public Tag Insert(Tag tag);
    }
}
