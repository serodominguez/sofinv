using BackEnd.Domains.Entities;

namespace BackEnd.Domains.Interfaces
{
    public interface IGoodsReceiptRepository
    {
        Task<IEnumerable<GoodsReceipt>> ReadReceipts(int id);
        Task<IEnumerable<GoodsReceipt>> SearchReceipt(string text, int id);
        Task<IEnumerable<DetailsReceipt>> ReadDetails(int id);
        Task<string> CreateReceipt(GoodsReceipt receipt);
        Task DisabledReceipt(int id);
    }
}
