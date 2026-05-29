using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Appointment;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Services;
using HairSalonVN.API.Services.DTOs.Staff;

namespace HairSalonVN.API.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponseDto>> GetAllAsync(Guid requesterId, string role);
        Task<IEnumerable<AppointmentResponseDto>> GetMyAppointmentsAsync(Guid requesterId, string role);
        Task<IEnumerable<AppointmentResponseDto>> GetByStaffIdAsync(Guid staffId);
        Task<IEnumerable<AppointmentResponseDto>> GetMyShiftAppointmentsAsync(Guid userId);
        Task<IEnumerable<AppointmentResponseDto>> GetTodayAppointmentsAsync(Guid userId);
        Task<AppointmentResponseDto?> GetByIdAsync(Guid id, Guid requesterId, string role);
        Task<ApiResponse<AppointmentResponseDto>> CreateAsync(AppointmentCreateDto dto, Guid customerId);
        Task<ApiResponse<AppointmentResponseDto>> GuestCreateAsync(AppointmentCreateDto dto);
        Task<ApiResponse<object>> UpdateStatusAsync(Guid id, string newStatus, Guid requesterId, string role);
        Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(Guid staffId, Guid serviceId, DateTime date);
    }

    public interface IServiceService
    {
        Task<IEnumerable<Database.Entities.Service>> GetAllActiveAsync();
        Task<Database.Entities.Service?> GetByIdAsync(Guid id);
        Task<IEnumerable<object>> GetByServiceStaffAsync(Guid serviceId);
        Task<Database.Entities.Service> CreateAsync(ServiceCreateDto dto);
        Task<ApiResponse<Database.Entities.Service>> UpdateAsync(Guid id, ServiceUpdateDto dto);
        Task SoftDeleteAsync(Guid id);
    }

    public interface IStaffService
    {
        Task<IEnumerable<Database.Entities.Staff>> GetAllAsync();
        Task<IEnumerable<Database.Entities.Staff>> GetByServiceIdAsync(Guid serviceId);
        Task<Database.Entities.Staff?> GetByIdAsync(Guid id);
        Task<Database.Entities.Staff?> GetByUserIdAsync(Guid userId);
        Task<ApiResponse<object>> UpdateWorkingHoursAsync(Guid staffId, List<WorkingHourUpdateDto> dto);
    }
}
