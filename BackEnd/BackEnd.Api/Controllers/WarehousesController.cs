using BackEnd.Application.Dtos.BrandsDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    //[Authorize(Roles = "ADMINISTRADORES, ALMACENEROS, OPERARIOS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly WarehouseService _warehouseService;

        public WarehousesController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<BrandReadDto>> SearchStock([FromQuery] string text, [FromQuery] int id)
        {
            if (text == null || id < 0)
            {
                return BadRequest();
            }

            var details = await _warehouseService.SearchStock(text, id);
            if (details == null)
            {
                return NotFound();
            }
            return Ok(details);
        }

        [HttpGet]
        public async Task<ActionResult<BrandReadDto>> SelectProduct([FromQuery] string text, [FromQuery] int id)
        {
            if (text == null || id < 0)
            {
                return BadRequest();
            }

            var details = await _warehouseService.SelectProduct(text, id);
            if (details == null)
            {
                return NotFound();
            }
            return Ok(details);
        }
    }
}
