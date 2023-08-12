using Microsoft.AspNetCore.Mvc;
using BasicBilling.API.Models;
using System.Linq;

namespace BasicBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly BillingDbContext _context;

        public BillsController(BillingDbContext context)
        {
            _context = context;
        }

        // endpoints
        [HttpPost("payment")]
        public IActionResult ProcessBillPayment([FromBody] BillPaymentRequest request)
        {

            // Validate the request, update the bill state, etc
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            var bill = _context.Bills.FirstOrDefault(b =>
                b.ClientId == request.ClientId &&
                b.ServiceType == request.ServiceType &&
                b.MonthYear == request.MonthYear);

            if (bill == null)
            {
                return NotFound("Bill not found.");
            }

            if (bill.IsPaid)
            {
                return BadRequest("Bill is already paid.");
            }

            // Update the bill's IsPaid status
            bill.IsPaid = true;
            _context.SaveChanges();

            return Ok(new { Message = "Payment processed successfully." });
        }

        [HttpGet("pending/{clientId}")]
        public IActionResult GetPendingBills(int clientId)
        {
            var pendingBills = _context.Bills
                .Where(b => b.ClientId == clientId && !b.IsPaid)
                .ToList();

            return Ok(pendingBills);
        }

        [HttpGet("payment-history/{clientId}")]
        public IActionResult GetPaymentHistory(int clientId)
        {
            var paymentHistory = _context.Bills
                .Where(b => b.ClientId == clientId && b.IsPaid)
                .ToList();

            return Ok(paymentHistory);
        }

    }
}
