using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface ICustomersRepository
    {
        Task<IEnumerable<Customers>> ReadCustomers();
        Task<IEnumerable<Customers>> SearchCustomer(string text);
        Task<IEnumerable<Customers>> SelectCustomers();
        Task<string> CreateCustomer(Customers customer);
        Task<string> UpdateCustomer(Customers customer);
        Task EnabledCustomer(int id);
        Task DisabledCustomer(int id);
    }
}
