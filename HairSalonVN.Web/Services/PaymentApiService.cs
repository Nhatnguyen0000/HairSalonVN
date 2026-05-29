using HairSalonVN.Web.Models.Admin;
using HairSalonVN.Web.Models.Shared;

namespace HairSalonVN.Web.Services
{
    public class PaymentApiService : ApiClientBase
    {
        public PaymentApiService(HttpClient http, IHttpContextAccessor ctx)
            : base(http, ctx) { }

        public Task<ApiResponse<IEnumerable<PaymentViewModel>>?> GetAllAsync()
            => GetAsync<IEnumerable<PaymentViewModel>>("payments");

        public Task<ApiResponse<PaymentSummaryDto>?> GetSummaryAsync()
            => GetAsync<PaymentSummaryDto>("payments/summary");
    }

    public class PaymentSummaryDto
    {
        public decimal TotalThisMonth { get; set; }
        public int CountThisMonth { get; set; }
        public decimal TotalAll { get; set; }
    }
}
