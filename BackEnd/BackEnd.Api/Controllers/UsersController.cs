using BackEnd.Application.Dtos.UsersDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public UsersController(UserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] UserCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.CreateUser(createDto);

                if (result == "Usuario creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateUser), createDto);
                }

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadUsers()
        {
            IEnumerable<UserReadDto> users = await Task.Run(() => _userService.ReadUsers());
            return Ok(users);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<UserReadDto>> SearchUser(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var users = await _userService.SearchUser(text);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto updateDto)
        {
            if (updateDto.PK_USER <= 0)
            {
                return BadRequest(new { message = "El usuario no es valido." });
            }

            if (updateDto.UPDATEPASSWORD == true)
            {
                var result = await _userService.UpdatePassword(updateDto);

                try
                {
                    if (result == "Actualización de la contraseña exitosamente.")
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
            else
            {
                var result = await _userService.UpdateUser(updateDto);
                try
                {
                    if (result == "Actualización del usuario exitosamente.")
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
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EnabledUser(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _userService.EnabledUser(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledUser(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _userService.DisabledUser(id);
            return NoContent();
        }
    }
}
