using Microsoft.AspNetCore.Mvc;
using BasicBilling.API.Models;
using System.Linq;
using System.Diagnostics;

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

        [HttpPost("bills")]
        public IActionResult CreateBill([FromBody] BillPaymentRequest request)
        {
            if (ModelState.IsValid)
            {
                var clients = _dbContext.Clients.ToList(); // Fetch all clients from the database

                foreach (var client in clients)
                {
                    // Create a new bill for each client
                    Bill newBill = new Bill
                    {
                        ClientId = client.Id,
                        Period = request.Period,
                        Category = request.Category,
                        PaymentStatus = "Pending" // Bills start as unpaid
                    };

                    _dbContext.Bills.Add(newBill);
                }

                _dbContext.SaveChanges();

                return Ok(new { Message = "Bills created successfully for all clients." });
            }

            return BadRequest(ModelState);
        }

        [HttpGet("pending")]
        public IActionResult GetPendingBills(int clientId)
        {
            var pendingBills = _dbContext.Bills
                .Where(b => b.ClientId == clientId && b.PaymentStatus == "Pending")
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
                bill.PaymentStatus = "Paid";
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

        [HttpGet("payment-history")]
        public IActionResult GetClientPaymentHistory(int clientId)
        {
            var paymentHistory = _dbContext.Bills
                .Where(b => b.ClientId == clientId)
                .OrderByDescending(b => b.Period)
                .ToList();

            return Ok(paymentHistory);
        }
    }
}
