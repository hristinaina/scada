using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using scada.Models;
using scada.Services.interfaces;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/tag/")]
    public class TagController : ControllerBase
    {

        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<Tag> tags = _tagService.Get();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            Tag tag = _tagService.Get(id);
            if (tag != null) return Ok(tag);
            return NotFound("Tag not found!");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            bool isDeleted = _tagService.Delete(id);
            if (isDeleted) { return Ok("Successfully deleted!"); }
            else return NotFound("Tag not found!");
        }
    }
}
