using BackEnd.Application.Dtos.RolesDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult> ReadRoles()
        {
            IEnumerable<RoleReadDto> roles = await Task.Run(() => _roleService.ReadRoles());
            return Ok(roles);
        }

        [HttpGet]
        public async Task<ActionResult> SelectRoles()
        {
            IEnumerable<RoleSelectDto> roles = await Task.Run(() => _roleService.SelectRoles());
            return Ok(roles);
        }
    }
}
