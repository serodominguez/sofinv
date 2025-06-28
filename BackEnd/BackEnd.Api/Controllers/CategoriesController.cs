using BackEnd.Application.Dtos.CategoriesDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _categoryService.CreateCategory(createDto);
                if (result == "Categoría creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateCategory), createDto);
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            } 
        }

        [HttpGet]
        public async Task<ActionResult> ReadCategories()
        {
            IEnumerable<CategoryReadDto> categories = await Task.Run(() => _categoryService.ReadCategories());
            return Ok(categories);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<CategoryReadDto>> SearchCategory(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }
              
            var categories = await _categoryService.SearchCategory(text);
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }
        [HttpGet]
        public async Task<ActionResult> SelectCategories()
        {
            IEnumerable<CategorySelectDto> categories = await Task.Run(() => _categoryService.SelectCategories());
            return Ok(categories);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryUpdateDto updateDto)
        {
            if (updateDto.PK_CATEGORY <= 0)
            {
                return BadRequest(new { message = "La categoría no es valida." });
            }

            try
            {
                var result = await _categoryService.UpdateCategory(updateDto);
                if (result == "Actualización de la categoría exitosamente.") 
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
        public async Task<ActionResult> EnabledCategory(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _categoryService.EnabledCategory(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledCategory(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _categoryService.DisabledCategory(id);
            return NoContent();
        }
    }
}
