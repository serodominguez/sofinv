using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<Users>> ReadUsers();
        Task<IEnumerable<Users>> SearchUser(string text);
        Task<Users?>CredentialsUser(string userName);
        Task<string> CreateUser(Users user, string password);
        Task<string> UpdateUser(Users user);
        Task<string> UpdatePassword(Users user, string password);
        Task EnabledUser(int id);
        Task DisabledUser(int id);
    }
}