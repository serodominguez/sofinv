using BackEnd.Core.Entities;

namespace BackEnd.Core.Interfaces
{
    public interface IBrandsRepository
    {
        Task<IEnumerable<Brands>> GetBrands();
    }
}
