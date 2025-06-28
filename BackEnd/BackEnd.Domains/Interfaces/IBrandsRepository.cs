using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IBrandsRepository
    {
       Task<IEnumerable<Brands>> ReadBrands();
       Task<IEnumerable<Brands>> SearchBrand(string text);
       Task<IEnumerable<Brands>> SelectBrands();
       Task<string> CreateBrand(Brands brand);
       Task<string> UpdateBrand(Brands brand);
       Task EnabledBrand(int id);
       Task DisabledBrand(int id);
    }
}
