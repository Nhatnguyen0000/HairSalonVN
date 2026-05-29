namespace HairSalonVN.Web.Models.Admin
{
    public class PaymentsPageViewModel
    {
        public IEnumerable<PaymentViewModel> Payments { get; set; } = [];
        public decimal TotalPaid { get; set; }
        public decimal TotalThisMonth { get; set; }
        public int CountThisMonth { get; set; }
    }
}
