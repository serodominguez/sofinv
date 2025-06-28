using BackEnd.Application.Dtos.SuppliersDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SuppliersController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSupplier([FromBody] SupplierCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _supplierService.CreateSupplier(createDto);
                if(result == "Proveedor creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateSupplier), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadSuppliers()
        {
            IEnumerable<SupplierReadDto> suppliers = await Task.Run(() => _supplierService.ReadSupplier());
            return Ok(suppliers);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<SupplierReadDto>> SearchSupplier(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var suppliers = await _supplierService.SearchSupplier(text);
            if (suppliers == null)
            {
                return NotFound();
            }
            return Ok(suppliers);
        }

        [HttpGet]
        public async Task<ActionResult> SelectSuppliers()
        {
            IEnumerable<SupplierSelectDto> suppliers = await Task.Run(() => _supplierService.SelectSuppliers());
            return Ok(suppliers);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSupplier([FromBody] SupplierUpdateDto updateDto)
        {
            if (updateDto.PK_SUPPLIER <= 0)
            {
                return BadRequest(new { message = "El proveedor no es valido." });
            }

            try
            {
                var result = await _supplierService.UpdateSupplier(updateDto);
                if (result == "Actualización del proveedor exitosamente.") 
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
        public async Task<ActionResult> EnabledSupplier(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _supplierService.EnabledSupplier(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledSupplier(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _supplierService.DisabledSupplier(id);
            return NoContent();
        }
    }
}
