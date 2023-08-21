using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/tag/")]
    public class TagController : ControllerBase 
    {
        private readonly ITagProcessingService _tagProcessingService;

        public TagController(ITagProcessingService tagProcessingService)
        {
            _tagProcessingService = tagProcessingService;
        }

        [HttpPost("rtu")]
        public IActionResult ReceiveRtu([FromBody] RTU rtu)
        {
            _tagProcessingService.ReceiveRTUValue(rtu);
            return Ok(new { Message = "Value changed." });
        }
    }
}
