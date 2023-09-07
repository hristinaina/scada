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
            try
            {
                List<Tag> tags = _service.Get();
                return Ok(tags);
            }
            catch(NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }

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


        [HttpPut("edit")]
        public IActionResult EditTag([FromBody] EditTagDTO th)
        {
            try
            {
                _service.EditTag(th);
                return Ok("Successfully changed value!");
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

        [HttpPost("alarm")]
        public IActionResult InsertAlarm([FromBody] AlarmDTO alarmDTO)
        {
            try
            {
                _service.InsertAlarm(alarmDTO);
                return Ok(new { Message = "Alarm added successfully." });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpDelete("alarm/{id}")]
        public IActionResult DeleteAlarm([FromRoute] int id)
        {
            try
            {
                _service.DeleteAlarm(id);
                return Ok(new { Message = "Alarm deleted successfully!" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("alarm/{id}")]
        public IActionResult GetAlarms(int id)
        {
            try
            {
                List<Alarm> alarms =_service.GetAlarmsByTagId(id);
                return Ok(alarms);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
