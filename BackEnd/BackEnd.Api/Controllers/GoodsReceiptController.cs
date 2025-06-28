using BackEnd.Application.Dtos.DetailsReceiptDtos;
using BackEnd.Application.Dtos.GoodsReceiptDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsReceiptController : ControllerBase
    {
        private readonly GoodReceiptService _goodReceiptService;
        public GoodsReceiptController(GoodReceiptService goodReceiptService)
        {
            _goodReceiptService = goodReceiptService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateReceipt([FromBody] GoodReceiptCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _goodReceiptService.CreateReceipt(createDto);
                if (result == "Entrada creado exitosamente.") 
                {
                    return CreatedAtAction(nameof(CreateReceipt), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadReceipts(int id)
        {
            IEnumerable<GoodReceiptReadDto> receipts = await Task.Run(() => _goodReceiptService.ReadReceipts(id));
            return Ok(receipts);
        }

        [HttpGet()]
        public async Task<ActionResult<GoodReceiptReadDto>> SearchReceipt([FromQuery] string text, [FromQuery] int id)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var receipts = await _goodReceiptService.SearchReceipt(text, id);
            if (receipts == null)
            {
                return NotFound();
            }
            return Ok(receipts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ReadDetails( int id)
        {
            IEnumerable<DetailReceiptReadDto> details = await Task.Run(() => _goodReceiptService.ReadDetails(id));
            return Ok(details);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledReceipt(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _goodReceiptService.DisabledReceipt(id);
            return NoContent();
        }
    }
}
