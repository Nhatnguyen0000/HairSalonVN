using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Shared;

namespace HairSalonVN.Web.Services
{
    public class ReviewApiService : ApiClientBase
    {
        public ReviewApiService(HttpClient http, IHttpContextAccessor ctx)
            : base(http, ctx) { }

        public Task<ApiResponse<IEnumerable<ReviewViewModel>>?> GetAllAsync()
            => GetAsync<IEnumerable<ReviewViewModel>>("reviews");

        public Task<ApiResponse<IEnumerable<ReviewViewModel>>?> GetByServiceAsync(Guid serviceId)
            => GetAsync<IEnumerable<ReviewViewModel>>($"reviews/service/{serviceId}");

        public Task<ApiResponse<IEnumerable<ReviewViewModel>>?> GetByStaffAsync(Guid staffId)
            => GetAsync<IEnumerable<ReviewViewModel>>($"reviews/staff/{staffId}");

        public Task<ApiResponse<ReviewViewModel>?> GetByAppointmentAsync(Guid appointmentId)
            => GetAsync<ReviewViewModel>($"reviews/appointment/{appointmentId}");

        public Task<ApiResponse<object>?> CreateAsync(CreateReviewViewModel vm)
            => PostAsync<object>("reviews", vm);
    }
}
