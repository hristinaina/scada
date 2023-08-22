using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services.interfaces;
using Newtonsoft.Json;
using scada.Data;

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

        [HttpPost]
        public IActionResult Insert([FromBody] TagInputModel tagInput)
        {
            Tag tag = tagInput.Type switch
            {
                "DOTag" => JsonConvert.DeserializeObject<DOTag>(tagInput.Data.ToString()),
                "DITag" => JsonConvert.DeserializeObject<DITag>(tagInput.Data.ToString()),
                "AOTag" => JsonConvert.DeserializeObject<AOTag>(tagInput.Data.ToString()),
                "AITag" => JsonConvert.DeserializeObject<AITag>(tagInput.Data.ToString()),
                _ => null // Handle unknown types
            };

            if (tag != null)
            {
                _tagService.Insert(tag);
                return Ok(tag);
            }

            return BadRequest("Invalid tag data");
        }
    }
}

public class TagInputModel
{
    public string Type { get; set; }
    public object Data { get; set; }
}
