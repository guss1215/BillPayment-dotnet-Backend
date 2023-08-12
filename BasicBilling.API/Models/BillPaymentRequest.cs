namespace BasicBilling.API.Models
{
    public class BillPaymentRequest
    {
        public int ClientId { get; set; }
        public string ServiceType { get; set; }
        public string MonthYear { get; set; }
    }
}
