using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IStoresRepository
    {
        Task<IEnumerable<Stores>> ReadStores();
        Task<IEnumerable<Stores>> SearchStore(string text);
        Task<IEnumerable<Stores>> SelectStores();
        Task<string> CreateStore(Stores store);
        Task<string> UpdateStore(Stores store);
        Task EnabledStore(int id);
        Task DisabledStore(int id);
    }
}
