using BackEnd.Application.Dtos.ProductsDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _productService.CreateProduct(createDto);
                if (result == "Producto creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateProduct), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }

        }

        [HttpGet]
        public async Task<ActionResult> ReadProducts()
        {
            IEnumerable<ProductReadDto> products = await Task.Run(() => _productService.ReadProducts());
            return Ok(products);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<ProductReadDto>> SearchProduct(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var products = await _productService.SearchProduct(text);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductUpdateDto updateDto)
        {
            if (updateDto.PK_BRAND <= 0)
            {
                return BadRequest(new { message = "El producto no es valido." });
            }

            try
            {
                var result = await _productService.UpdateProduct(updateDto);
                if (result == "Actualización del producto exitosamente.")
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
        public async Task<ActionResult> EnabledProduct(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _productService.EnabledProduct(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledProduct(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _productService.DisabledProduct(id);
            return NoContent();
        }
    }
}
