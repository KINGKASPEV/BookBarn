using BookBarn.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBarn.Controllers
{
    [Route("api/purchase-history")]
    [ApiController]
    [Authorize]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;

        public PurchaseHistoryController(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseHistory(string userId)
        {
            var response = await _purchaseHistoryService.GetPurchaseHistoryAsync(userId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
