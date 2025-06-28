using BackEnd.Application.Dtos.StoresDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly StoreService _storeService;
        public StoresController(StoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStore([FromBody] StoreCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _storeService.CreateStore(createDto);
                if (result == "Sucursal creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateStore), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadStores()
        {
            IEnumerable<StoreReadDto> stores = await Task.Run(() => _storeService.ReadStores());
            return Ok(stores);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<StoreReadDto>> SearchStore(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var stores = await _storeService.SearchStore(text);
            if (stores == null)
            {
                return NotFound();
            }
            return Ok(stores);
        }

        [HttpGet]
        public async Task<ActionResult> SelectStores()
        {
            IEnumerable<StoreSelectDto> stores = await Task.Run(() => _storeService.SelectStores());
            return Ok(stores);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStore([FromBody] StoreUpdateDto updateDto)
        {
            if (updateDto.PK_STORE <= 0)
            {
                return BadRequest(new { message = "La sucursal no es valida." });
            }

            try
            {
                var result = await _storeService.UpdateStore(updateDto);
                if (result == "Actualización de la sucursal exitosamente.")
                {
                    return NoContent();
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EnabledStore(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _storeService.EnabledStore(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledStore(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _storeService.DisabledStore(id);
            return NoContent();
        }
    }
}
