using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IWarehousesRepository
    {
        Task<IEnumerable<DetailsWarehouse>> SearchStock(string text, int id);
        Task<IEnumerable<DetailsWarehouse>> SelectProduct(string text, int id);
    }
}
