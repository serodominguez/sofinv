using BackEnd.Application.Dtos.DetailsIssueDtos;
using BackEnd.Application.Dtos.GoodsIssueDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsIssueController : ControllerBase
    {
        private readonly GoodIssueService _goodIssueService;
        public GoodsIssueController(GoodIssueService goodReceiptService)
        {
            _goodIssueService = goodReceiptService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateIssue([FromBody] GoodIssueCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _goodIssueService.CreateIssue(createDto);
                if (result == "Salida creada exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateIssue), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadIssues(int id)
        {
            IEnumerable<GoodIssueReadDto> issues = await Task.Run(() => _goodIssueService.ReadIssues(id));
            return Ok(issues);
        }

        [HttpGet()]
        public async Task<ActionResult<GoodIssueReadDto>> SearchIssue([FromQuery] string text, [FromQuery] int id)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var issues = await _goodIssueService.SearchIssue(text, id);
            if (issues == null)
            {
                return NotFound();
            }
            return Ok(issues);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadDetails(int id)
        {
            IEnumerable<DetailIssueReadDto> details = await Task.Run(() => _goodIssueService.ReadDetails(id));
            return Ok(details);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledIssue(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _goodIssueService.DisabledIssue(id);
            return NoContent();
        }
    }
}
