using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface ISuppliersRepository
    {
        Task<IEnumerable<Suppliers>> ReadSuppliers();
        Task<IEnumerable<Suppliers>> SearchSupplier(string text);
        Task<IEnumerable<Suppliers>> SelectSuppliers();
        Task<string> CreateSupplier(Suppliers supplier);
        Task<string> UpdateSupplier(Suppliers supplier);
        Task EnabledSupplier(int id);
        Task DisabledSupplier(int id);
    }
}
