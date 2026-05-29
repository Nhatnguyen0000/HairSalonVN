using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Staff;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.API.Services
{
    public class StaffManagementService : IStaffService
    {
        private readonly IRepository<Staff> _staffRepo;
        private readonly IRepository<WorkingHour> _whRepo;

        public StaffManagementService(
            IRepository<Staff> staffRepo,
            IRepository<WorkingHour> whRepo)
        {
            _staffRepo = staffRepo;
            _whRepo = whRepo;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _staffRepo.FindAsync(s => true, s => s.User!, s => s.WorkingHours!);
        }

        public async Task<IEnumerable<Staff>> GetByServiceIdAsync(Guid serviceId)
        {
            return await _staffRepo.FindAsync(
                s => s.IsAvailable && s.StaffServices!.Any(ss => ss.ServiceId == serviceId),
                s => s.User!,
                s => s.WorkingHours!,
                s => s.StaffServices!);
        }

        public async Task<Staff?> GetByIdAsync(Guid id)
        {
            var result = await _staffRepo.FindAsync(
                s => s.Id == id,
                s => s.User!,
                s => s.WorkingHours!,
                s => s.StaffServices!);
            return result.FirstOrDefault();
        }

        public async Task<Staff?> GetByUserIdAsync(Guid userId)
        {
            var result = await _staffRepo.FindAsync(
                s => s.UserId == userId,
                s => s.User!,
                s => s.WorkingHours!,
                s => s.StaffServices!);
            return result.FirstOrDefault();
        }

        public async Task<ApiResponse<object>> UpdateWorkingHoursAsync(Guid staffId, List<WorkingHourUpdateDto> dto)
        {
            var staff = await _staffRepo.GetByIdAsync(staffId);
            if (staff == null)
                return ApiResponse<object>.Fail("Khong tim thay nhan vien");

            if (dto == null || dto.Any(w => w.DayOfWeek < 0 || w.DayOfWeek > 6 || w.StartTime >= w.EndTime))
                return ApiResponse<object>.Fail("Gio lam viec khong hop le");

            var existing = await _whRepo.FindWithTrackingAsync(w => w.StaffId == staffId);
            foreach (var item in existing)
                _whRepo.Remove(item);

            foreach (var wh in dto)
            {
                var newWh = new WorkingHour
                {
                    Id = Guid.NewGuid(),
                    StaffId = staffId,
                    DayOfWeek = wh.DayOfWeek,
                    StartTime = wh.StartTime,
                    EndTime = wh.EndTime
                };
                await _whRepo.AddAsync(newWh);
            }

            await _whRepo.SaveChangesAsync();
            return ApiResponse<object>.Ok(null, "Cap nhat gio lam viec thanh cong");
        }
    }
}
