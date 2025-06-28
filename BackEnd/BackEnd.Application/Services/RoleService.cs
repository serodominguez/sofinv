using BackEnd.Application.Dtos.RolesDtos;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class RoleService
    {
        private readonly IRolesRepository _roleRepository;

        public RoleService(IRolesRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleReadDto>> ReadRoles()
        {
            var roles = await _roleRepository.ReadRoles();
            return roles.Select(r => new RoleReadDto
            {
                PK_ROLE = r.PK_ROLE,
                ROLE_NAME = r.ROLE_NAME,
                STATUS = r.STATUS,
                CREATION_DATE = r.CREATION_DATE.HasValue ? r.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<RoleSelectDto>> SelectRoles()
        {
            var roles = await _roleRepository.SelectRoles();
            return roles.Select(r => new RoleSelectDto
            {
                PK_ROLE = r.PK_ROLE,
                ROLE_NAME = r.ROLE_NAME
            });
        }
    }
}
