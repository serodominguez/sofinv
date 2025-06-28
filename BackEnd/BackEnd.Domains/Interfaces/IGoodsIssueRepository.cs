using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IGoodsIssueRepository
    {
        Task<IEnumerable<GoodsIssue>> ReadIssues(int id);
        Task<IEnumerable<GoodsIssue>> SearchIssue(string text, int id);
        Task<IEnumerable<DetailsIssue>> ReadDetails(int id);
        Task<string> CreateIssue(GoodsIssue issue);
        Task DisabledIssue(int id);
    }
}
