using AutoMapper;
using BookBarn.Application.DTOs.PurchaseHistory;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain.Entities;
using BookBarn.Domain;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookBarn.Application.Services.Implementations
{
    public class PurchaseHistoryService : IPurchaseHistoryService
    {
        private readonly IGenericRepository<PurchaseHistory> _purchaseHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseHistoryService> _logger;

        public PurchaseHistoryService(IGenericRepository<PurchaseHistory> purchaseHistoryRepository, IMapper mapper, ILogger<PurchaseHistoryService> logger)
        {
            _purchaseHistoryRepository = purchaseHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //public async Task<ApiResponse<IEnumerable<PurchaseHistoryDto>>> GetPurchaseHistoryAsync(string userId)
        //{
        //    try
        //    {
        //        var purchaseHistories = await _purchaseHistoryRepository.FindAsync(ph => ph.AppUserId == userId);
        //        var purchaseHistoryDtos = _mapper.Map<IEnumerable<PurchaseHistoryDto>>(purchaseHistories);
        //        return ApiResponse<IEnumerable<PurchaseHistoryDto>>.Success(purchaseHistoryDtos, "Purchase history retrieved successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while retrieving purchase history.");
        //        return ApiResponse<IEnumerable<PurchaseHistoryDto>>.Failed(false, "An error occurred while retrieving the purchase history.", 500, new List<string> { ex.Message });
        //    }
        //}

        public async Task<ApiResponse<IEnumerable<PurchaseHistoryDto>>> GetPurchaseHistoryAsync(string userId)
        {
            try
            {
                var purchaseHistories = await _purchaseHistoryRepository.FindAndIncludeAsync(
                    ph => ph.AppUserId == userId,
                    ph => ph.Checkout,
                    ph => ph.Checkout.Cart,
                    ph => ph.Checkout.Cart.Books
                );

                var purchaseHistoryDtos = _mapper.Map<IEnumerable<PurchaseHistoryDto>>(purchaseHistories);
                return ApiResponse<IEnumerable<PurchaseHistoryDto>>.Success(purchaseHistoryDtos, "Purchase history retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase history.");
                return ApiResponse<IEnumerable<PurchaseHistoryDto>>.Failed(false, "An error occurred while retrieving the purchase history.", 500, new List<string> { ex.Message });
            }
        }


    }
}
