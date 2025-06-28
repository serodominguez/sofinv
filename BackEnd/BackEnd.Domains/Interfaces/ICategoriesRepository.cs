using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Categories>> ReadCategories();
        Task<IEnumerable<Categories>> SearchCategory(string text);
        Task<IEnumerable<Categories>> SelectCategories();
        Task<string> CreateCategory(Categories category);
        Task<string> UpdateCategory(Categories category);
        Task EnabledCategory(int id);
        Task DisabledCategory(int id);
    }
}
