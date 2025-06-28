using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEnd.Application.Dtos.UsersDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _config;

        public LoginController(UserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userService.CredentialsUser(loginDto.USER);

            if (user == null)
            {
                return BadRequest(new { message = "El nombre de usuario no existe." });
            }

            if (user.PASSWORD_HASH == null || user.PASSWORD_SALT == null)
            {
                return BadRequest(new { message = "Error en la autenticación. Inténtalo de nuevo." });
            }

            if (!ValidatePasswordHash(loginDto.PASSWORD, user.PASSWORD_HASH, user.PASSWORD_SALT))
            {
                return BadRequest(new { message = "La contraseña es incorrecta." });
            }

            if (string.IsNullOrWhiteSpace(user.USER_NAME) || string.IsNullOrWhiteSpace(user.ROLE_NAME) || string.IsNullOrWhiteSpace(user.STORE_NAME))
            {
                return BadRequest(new { message = "Error en la información del usuario. Inténtalo de nuevo." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.PK_USER.ToString()),
                new Claim(ClaimTypes.Name, user.USER_NAME),
                new Claim(ClaimTypes.Role, user.ROLE_NAME),

                new Claim("pk_user",user.PK_USER.ToString()),
                new Claim("user_name", user.USER_NAME),
                new Claim("role", user.ROLE_NAME),
                new Claim("store_name", user.STORE_NAME),
                new Claim("pk_store", user.PK_STORE.ToString()),
                new Claim("pk_warehouse", user.PK_WAREHOUSE.ToString())
            };

            return Ok(
                    new { token = GenerateToken(claims) }
                );

        }

        private bool ValidatePasswordHash(string password, byte[] passwordHashStored, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNew = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashStored).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNew));
            }
        }

        private string GenerateToken(List<Claim> claims)
        {
            var jwtKey = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              expires: DateTime.Now.AddMinutes(300),
              signingCredentials: creds,
              claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
