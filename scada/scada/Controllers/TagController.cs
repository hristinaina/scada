using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services;
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
        private readonly ITagProcessingService _tagProcessingService ;


        public TagController(ITagService tagService, TagProcessingService tagProcessingService)
        {
            _service = tagService;
            _tagProcessingService = tagProcessingService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            List<Tag> tags = _service.Get();
            return Ok(tags);
        }

        [HttpGet("do")]
        public IActionResult GetDOTags()
        {
            List<DOTag> tags = _service.GetDOTags();
            return Ok(tags);
        }

        [HttpGet("ao")]
        public IActionResult GetAOTags()
        {
            List<AOTag> tags = _service.GetAOTags();
            return Ok(tags);
        }

        [HttpGet("di")]
        public IActionResult GetDITags()
        {
            List<DITag> tags = _service.GetDITags();
            return Ok(tags);
        }

        [HttpGet("ai")]
        public IActionResult GetAITags()
        {
            List<AITag> tags = _service.GetAITags();
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

        [HttpPut("scan{id}")]
        public IActionResult ChangeScan([FromRoute] int id)
        {
            try
            {
                _service.ChangeScan(id);
                return Ok("Successfully changed scan!");
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try { 
                _tagProcessingService.Delete(id);
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
                Tag tag = _tagProcessingService.Insert(tagInput);
                return Ok(tag);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("rtu")]
        public IActionResult ReceiveRtu([FromBody] RTUData rtu)
        {
            _service.ReceiveRTUValue(rtu);
            return Ok(new { Message = "Value changed." });
        }
    }
}
