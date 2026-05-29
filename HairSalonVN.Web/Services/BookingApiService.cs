using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Shared;

namespace HairSalonVN.Web.Services
{
    public class BookingApiService : ApiClientBase
    {
        public BookingApiService(HttpClient http, IHttpContextAccessor ctx)
            : base(http, ctx) { }

        // Lấy tất cả lịch hẹn (Admin thấy tất cả)
        public Task<ApiResponse<IEnumerable<AppointmentViewModel>>?> GetAllAsync()
            => GetAsync<IEnumerable<AppointmentViewModel>>("appointments");

        // Lấy lịch hẹn của user hiện tại (Customer/Staff)
        public Task<ApiResponse<IEnumerable<AppointmentViewModel>>?> GetMyAsync()
            => GetAsync<IEnumerable<AppointmentViewModel>>("appointments/my");

        // Lấy lịch hẹn theo staff cụ thể (Admin)
        public Task<ApiResponse<IEnumerable<AppointmentViewModel>>?> GetByStaffAsync(Guid staffId)
            => GetAsync<IEnumerable<AppointmentViewModel>>($"appointments/staff/{staffId}");

        // Lấy lịch hẹn hôm nay của staff
        public Task<ApiResponse<IEnumerable<AppointmentViewModel>>?> GetTodayAsync()
            => GetAsync<IEnumerable<AppointmentViewModel>>("appointments/today");

        // Lấy lịch hẹn được gán cho Staff đang đăng nhập (Dashboard Staff)
        public Task<ApiResponse<IEnumerable<AppointmentViewModel>>?> GetMyShiftAsync()
            => GetAsync<IEnumerable<AppointmentViewModel>>("appointments/by-staff");

        // Lấy khung giờ trống
        public Task<ApiResponse<IEnumerable<AvailableSlotViewModel>>?> GetSlotsAsync(
            Guid staffId, Guid serviceId, DateTime date)
            => GetAsync<IEnumerable<AvailableSlotViewModel>>(
                $"appointments/available-slots" +
                $"?staffId={staffId}&serviceId={serviceId}" +
                $"&date={date:yyyy-MM-dd}");

        // Tạo lịch hẹn (Customer đã đăng nhập)
        public Task<ApiResponse<AppointmentViewModel>?> CreateAsync(
            AppointmentCreateViewModel vm)
            => PostAsync<AppointmentViewModel>("appointments", new
            {
                vm.ServiceId,
                vm.StaffId,
                vm.AppointmentDate,
                vm.Notes
            });

        // Guest booking - sử dụng endpoint riêng
        public Task<ApiResponse<AppointmentViewModel>?> GuestCreateAsync(
            AppointmentCreateViewModel vm)
            => PostAsync<AppointmentViewModel>("appointments/guest", new
            {
                vm.ServiceId,
                vm.StaffId,
                vm.AppointmentDate,
                vm.Notes,
                IsGuest = true,
                vm.GuestName,
                vm.GuestPhone,
                vm.GuestEmail
            });

        // Cập nhật trạng thái
        public Task<ApiResponse<object>?> UpdateStatusAsync(
            Guid id, string status)
            => PutAsync<object>(
                $"appointments/{id}/status",
                new { Status = status });
    }
}
