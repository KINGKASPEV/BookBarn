using BookBarn.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookBarn.Controllers
{
    [Route("api/purchase-history")]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;

        public PurchaseHistoryController(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            var response = await _purchaseHistoryService.GetPurchaseHistoryAsync(userId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
