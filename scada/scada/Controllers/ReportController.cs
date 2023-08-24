using Microsoft.AspNetCore.Mvc;
using scada.DTO;
using scada.Exceptions;
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

        [HttpGet("tagId/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                List<TagHistoryDTO> tagsDTO = _tagHistoryService.GetByTagId(id);
                return Ok(tagsDTO);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
