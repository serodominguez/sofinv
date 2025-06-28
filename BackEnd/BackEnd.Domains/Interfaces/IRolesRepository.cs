using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IRolesRepository
    {
        Task<IEnumerable<Roles>> ReadRoles();
        Task<IEnumerable<Roles>> SelectRoles();
    }
}
