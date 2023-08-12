using Microsoft.AspNetCore.Mvc;
using BasicBilling.API.Models;
using System.Linq;

namespace BasicBilling.API.Controllers
{
    [ApiController]
    [Route("billing")]
    public class BillsController : ControllerBase
    {
        private readonly BillingDbContext _dbContext;

        public BillsController(BillingDbContext context)
        {
            _dbContext = context;
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

            var bill = _dbContext.Bills.FirstOrDefault(
                b =>
                    b.ClientId == request.ClientId
                    && b.Category == request.Category
                    && b.Period == request.Period
            );

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
            _dbContext.SaveChanges();

            return Ok(new { Message = "Payment processed successfully." });
        }

        [HttpPost("bills")]
        public IActionResult CreateBill([FromBody] BillPaymentRequest request)
        {
            if (ModelState.IsValid)
            {
                // Create a new bill based on the request data
                Bill newBill = new Bill
                {
                    ClientId = request.ClientId,
                    Period = request.Period,
                    Category = request.Category,
                    IsPaid = false // Assuming the bill starts as unpaid
                };

                _dbContext.Bills.Add(newBill);
                _dbContext.SaveChanges();

                return Ok(new { Message = "Bill created successfully." });
            }

            return BadRequest(ModelState);
        }

        [HttpGet("pending")]
        public IActionResult GetPendingBills(int clientId)
        {
            var pendingBills = _dbContext.Bills
                .Where(b => b.ClientId == clientId && !b.IsPaid)
                .ToList();

            return Ok(pendingBills);
        }

        [HttpPost("pay")]
        public IActionResult MarkBillsAsPaid([FromBody] BillPaymentRequest request)
        {
            var paidBills = _dbContext.Bills
                .Where(
                    b =>
                        b.ClientId == request.ClientId
                        && b.Period == request.Period
                        && b.Category == request.Category
                )
                .ToList();

            foreach (var bill in paidBills)
            {
                bill.IsPaid = true;
            }

            _dbContext.SaveChanges();

            return Ok(new { Message = "Bills marked as paid successfully." });
        }

        [HttpGet("search")]
        public IActionResult SearchBills(string category)
        {
            var bills = _dbContext.Bills.Where(b => b.Category == category).ToList();

            return Ok(bills);
        }
    }
}
