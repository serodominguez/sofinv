using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Products>> ReadProducts();
        Task<IEnumerable<Products>> SearchProduct(string text);
        Task<string> CreateProduct(Products product);
        Task<string> UpdateProduct(Products product);
        Task EnabledProduct(int id);
        Task DisabledProduct(int id);
    }
}
