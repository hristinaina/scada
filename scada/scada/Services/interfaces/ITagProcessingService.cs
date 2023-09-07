using scada.Drivers;
using scada.DTO;
using scada.Models;

namespace scada.Services
{
    public interface ITagProcessingService
    {
        public void Run();
        public bool Delete(int id);
        public Tag Insert(TagDTO tag);
    }
}
