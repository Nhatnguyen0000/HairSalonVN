using HairSalonVN.Web.Models.Booking;
using HairSalonVN.Web.Models.Shared;
using HairSalonVN.Web.Models.Service;

namespace HairSalonVN.Web.Services
{
    public class ServiceApiService : ApiClientBase
    {
        public ServiceApiService(HttpClient http, IHttpContextAccessor ctx)
            : base(http, ctx) { }

        // Lấy tất cả dịch vụ
        public Task<ApiResponse<IEnumerable<ServiceCardViewModel>>?> GetAllAsync()
            => GetAsync<IEnumerable<ServiceCardViewModel>>("services");

        // Lấy dịch vụ theo ID
        public Task<ApiResponse<ServiceCardViewModel>?> GetByIdAsync(Guid id)
            => GetAsync<ServiceCardViewModel>($"services/{id}");

        // Lấy dịch vụ theo ID (alias)
        public Task<ApiResponse<ServiceCardViewModel>?> GetByServiceAsync(Guid id)
            => GetAsync<ServiceCardViewModel>($"services/{id}");

        // Lấy danh sách staff theo dịch vụ
        public Task<ApiResponse<IEnumerable<StaffSlotViewModel>>?> GetStaffByServiceAsync(
            Guid serviceId)
            => GetAsync<IEnumerable<StaffSlotViewModel>>(
                $"staff?serviceId={serviceId}");

        // Lấy tất cả staff
        public Task<ApiResponse<IEnumerable<StaffSlotViewModel>>?> GetAllStaffAsync()
            => GetAsync<IEnumerable<StaffSlotViewModel>>("staff");

        // Lấy staff theo ID
        public Task<ApiResponse<StaffSlotViewModel>?> GetStaffByIdAsync(Guid id)
            => GetAsync<StaffSlotViewModel>($"staff/{id}");

        // Tạo dịch vụ mới
        public Task<ApiResponse<ServiceCardViewModel>?> CreateAsync(
            ServiceManageViewModel vm)
            => PostAsync<ServiceCardViewModel>("services", new
            {
                vm.Name,
                vm.Description,
                vm.Price,
                vm.DurationMinutes,
                ImageUrl = ""
            });

        // Cập nhật dịch vụ
        public Task<ApiResponse<ServiceCardViewModel>?> UpdateAsync(
            Guid id, ServiceManageViewModel vm)
            => PutAsync<ServiceCardViewModel>($"services/{id}", new
            {
                vm.Name,
                vm.Description,
                vm.Price,
                vm.DurationMinutes,
                vm.IsActive,
                ImageUrl = ""
            });

        // Xóa dịch vụ
        public Task<ApiResponse<object>?> DeleteAsync(Guid id)
            => DeleteAsync<object>($"services/{id}");
    }
}
