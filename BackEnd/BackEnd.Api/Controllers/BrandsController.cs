using BackEnd.Application.Dtos.BrandsDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandsController : ControllerBase
    { 
        private readonly BrandService _brandService;

        public BrandsController(BrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBrand([FromBody] BrandCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _brandService.CreateBrand(createDto);
                if (result == "Marca creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateBrand), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadBrands()
        {
            IEnumerable<BrandReadDto> brands = await Task.Run(() => _brandService.ReadBrands());
            return Ok(brands);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<BrandReadDto>> SearchBrand(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var brands = await _brandService.SearchBrand(text);
            if (brands == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpGet]
        public async Task<ActionResult> SelectBrands()
        {
            IEnumerable<BrandSelectDto> brands = await Task.Run(() => _brandService.SelectBrands());
            return Ok(brands);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBrand([FromBody] BrandUpdateDto updateDto)
        {
            if(updateDto.PK_BRAND <= 0 )
            {
                return BadRequest(new { message = "La marca no es valida." });
            }

            try
            {
                var result = await _brandService.UpdateBrand(updateDto);
                if (result == "Actualización de la marca exitosamente.")
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
        public async Task<ActionResult> EnabledBrand(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _brandService.EnabledBrand(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledBrand(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _brandService.DisabledBrand(id);
            return NoContent();
        }
    }
}
