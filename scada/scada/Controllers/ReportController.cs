using Microsoft.AspNetCore.Mvc;
using scada.Models;
using scada.Services;

namespace scada.Controllers
{
    [ApiController]
    [Route("api/report/")]
    public class ReportController : ControllerBase
    {
        private readonly ITagHistoryService _tagHistoryService;
        private readonly IAlarmHistoryService _alarmHistoryService;

        public ReportController(ITagHistoryService tagHistoryService, IAlarmHistoryService alarmHistoryService)
        {
            _tagHistoryService = tagHistoryService;
            _alarmHistoryService = alarmHistoryService;
        }

/*        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                Tag tag = _tagHistoryService.Get(id);
                return Ok(tag);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }*/
    }
}
