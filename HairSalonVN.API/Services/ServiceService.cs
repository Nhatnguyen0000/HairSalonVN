using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HairSalonVN.API.Services.DTOs.Common;
using HairSalonVN.API.Services.DTOs.Services;
using HairSalonVN.API.Services.Interfaces;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.API.Services
{
    public class ServiceManagementService : IServiceService
    {
        private readonly IRepository<Database.Entities.Service> _svcRepo;
        private readonly IRepository<StaffService> _ssRepo;

        public ServiceManagementService(
            IRepository<Database.Entities.Service> svcRepo,
            IRepository<StaffService> ssRepo)
        {
            _svcRepo = svcRepo;
            _ssRepo = ssRepo;
        }

        public async Task<IEnumerable<Database.Entities.Service>> GetAllActiveAsync()
        {
            return await _svcRepo.FindAsync(s => s.IsActive);
        }

        public async Task<Database.Entities.Service?> GetByIdAsync(Guid id)
        {
            return await _svcRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<object>> GetByServiceStaffAsync(Guid serviceId)
        {
            var list = await _ssRepo.FindAsync(
                ss => ss.ServiceId == serviceId,
                ss => ss.Staff!,
                ss => ss.Staff!.User!);

            return list.Select(ss => (object)new
            {
                StaffId = ss.Staff!.Id,
                StaffName = ss.Staff.User?.FullName ?? "",
                Specialty = ss.Staff.Specialty ?? "",
                AvatarUrl = ss.Staff.AvatarUrl ?? "",
                IsAvailable = ss.Staff.IsAvailable
            }).ToList();
        }

        public async Task<Database.Entities.Service> CreateAsync(ServiceCreateDto dto)
        {
            var svc = new Database.Entities.Service
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim() ?? "",
                ImageUrl = dto.ImageUrl ?? "",
                Price = dto.Price,
                DurationMinutes = dto.DurationMinutes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _svcRepo.AddAsync(svc);
            await _svcRepo.SaveChangesAsync();
            return svc;
        }

        public async Task<ApiResponse<Database.Entities.Service>> UpdateAsync(Guid id, ServiceUpdateDto dto)
        {
            var svc = await _svcRepo.GetByIdAsync(id);
            if (svc == null)
                return ApiResponse<Database.Entities.Service>.Fail("Khong tim thay dich vu");

            svc.Name = dto.Name.Trim();
            svc.Description = dto.Description?.Trim() ?? "";
            svc.ImageUrl = dto.ImageUrl ?? "";
            svc.Price = dto.Price;
            svc.DurationMinutes = dto.DurationMinutes;
            svc.IsActive = dto.IsActive;

            _svcRepo.Update(svc);
            await _svcRepo.SaveChangesAsync();

            return ApiResponse<Database.Entities.Service>.Ok(svc);
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var svc = await _svcRepo.GetByIdAsync(id);
            if (svc == null) return;
            svc.IsActive = false;
            _svcRepo.Update(svc);
            await _svcRepo.SaveChangesAsync();
        }
    }
}
