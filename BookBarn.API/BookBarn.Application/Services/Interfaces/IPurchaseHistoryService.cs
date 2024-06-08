using BookBarn.Application.DTOs.PurchaseHistory;
using BookBarn.Domain;

namespace BookBarn.Application.Services.Interfaces
{
    public interface IPurchaseHistoryService
    {
        Task<ApiResponse<IEnumerable<PurchaseHistoryDto>>> GetPurchaseHistoryAsync(string userId);
    }
}
