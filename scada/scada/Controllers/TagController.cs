using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services.interfaces;
using scada.Exceptions;
using scada.DTO;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/tag/")]
    public class TagController : ControllerBase
    {

        private readonly ITagService _service;

        public TagController(ITagService tagService)
        {
            _service = tagService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<Tag> tags = _service.Get();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                Tag tag = _service.Get(id);
                return Ok(tag);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try { 
                _service.Delete(id);
                return Ok("Successfully deleted!"); 
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Insert([FromBody] TagDTO tagInput)
        {
            try
            {
                Tag tag = _service.Insert(tagInput);
                return Ok(tag);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}