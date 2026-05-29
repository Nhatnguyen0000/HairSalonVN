
using HairSalonVN.Database.Constants;
using HairSalonVN.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalonVN.Database.Seeders;

public static class DataSeeder
{
    // Seed passwords — change in appsettings.json SeedSettings
    private const string SeedAdminPassword = "Admin@123";
    private const string SeedStaffPassword = "Staff@123";
    private const string SeedCustomerPassword = "Customer@123";

    // Fixed GUIDs để seed data không thay đổi mỗi migration

    // ── Users ───────────────────────────────────────────────
    private static readonly Guid AdminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid StaffU1 = Guid.Parse("00000000-0000-0000-0000-000000000003");
    private static readonly Guid StaffU2 = Guid.Parse("00000000-0000-0000-0000-000000000005");
    private static readonly Guid StaffU3 = Guid.Parse("00000000-0000-0000-0000-000000000007");

    // ── Staff ───────────────────────────────────────────────
    private static readonly Guid Staff1Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private static readonly Guid Staff2Id = Guid.Parse("00000000-0000-0000-0000-000000000004");
    private static readonly Guid Staff3Id = Guid.Parse("00000000-0000-0000-0000-000000000006");

    // ── Services ────────────────────────────────────────────
    private static readonly Guid Svc1 = Guid.Parse("00000000-0000-0000-0000-000000000010"); // Cắt Tóc Nam
    private static readonly Guid Svc2 = Guid.Parse("00000000-0000-0000-0000-000000000011"); // Cắt Tóc Nữ
    private static readonly Guid Svc3 = Guid.Parse("00000000-0000-0000-0000-000000000012"); // Uốn Tóc Xoăn
    private static readonly Guid Svc4 = Guid.Parse("00000000-0000-0000-0000-000000000013"); // Nhuộm Tóc
    private static readonly Guid Svc5 = Guid.Parse("00000000-0000-0000-0000-000000000014"); // Gội Đầu Massage
    private static readonly Guid Svc6 = Guid.Parse("00000000-0000-0000-0000-000000000015"); // Duỗi Tóc

    // ── Customers ────────────────────────────────────────────
    private static readonly Guid Cust1 = Guid.Parse("00000000-0000-0000-0000-000000000020");
    private static readonly Guid Cust2 = Guid.Parse("00000000-0000-0000-0000-000000000021");
    private static readonly Guid Cust3 = Guid.Parse("00000000-0000-0000-0000-000000000022");
    private static readonly Guid Cust4 = Guid.Parse("00000000-0000-0000-0000-000000000023");
    private static readonly Guid Cust5 = Guid.Parse("00000000-0000-0000-0000-000000000024");
    private static readonly Guid Cust6 = Guid.Parse("00000000-0000-0000-0000-000000000025");
    private static readonly Guid Cust7 = Guid.Parse("00000000-0000-0000-0000-000000000026");
    private static readonly Guid Cust8 = Guid.Parse("00000000-0000-0000-0000-000000000027");
    private static readonly Guid Cust9 = Guid.Parse("00000000-0000-0000-0000-000000000028");
    private static readonly Guid Cust10 = Guid.Parse("00000000-0000-0000-0000-000000000029");

    // ── Appointments ────────────────────────────────────────
    private static readonly Guid Appt1 = Guid.Parse("00000000-0000-0000-0001-000000000001");
    private static readonly Guid Appt2 = Guid.Parse("00000000-0000-0000-0001-000000000002");
    private static readonly Guid Appt3 = Guid.Parse("00000000-0000-0000-0001-000000000003");
    private static readonly Guid Appt4 = Guid.Parse("00000000-0000-0000-0001-000000000004");
    private static readonly Guid Appt5 = Guid.Parse("00000000-0000-0000-0001-000000000005");
    private static readonly Guid Appt6 = Guid.Parse("00000000-0000-0000-0001-000000000006");
    private static readonly Guid Appt7 = Guid.Parse("00000000-0000-0000-0001-000000000007");
    private static readonly Guid Appt8 = Guid.Parse("00000000-0000-0000-0001-000000000008");
    private static readonly Guid Appt9 = Guid.Parse("00000000-0000-0000-0001-000000000009");
    private static readonly Guid Appt10 = Guid.Parse("00000000-0000-0000-0001-000000000010");
    private static readonly Guid Appt11 = Guid.Parse("00000000-0000-0000-0001-000000000011");
    private static readonly Guid Appt12 = Guid.Parse("00000000-0000-0000-0001-000000000012");
    private static readonly Guid Appt13 = Guid.Parse("00000000-0000-0000-0001-000000000013");
    private static readonly Guid Appt14 = Guid.Parse("00000000-0000-0000-0001-000000000014");
    private static readonly Guid Appt15 = Guid.Parse("00000000-0000-0000-0001-000000000015");

    // ── Payments ────────────────────────────────────────────
    private static readonly Guid Pay1 = Guid.Parse("00000000-0000-0000-0002-000000000001");
    private static readonly Guid Pay2 = Guid.Parse("00000000-0000-0000-0002-000000000002");
    private static readonly Guid Pay3 = Guid.Parse("00000000-0000-0000-0002-000000000003");
    private static readonly Guid Pay4 = Guid.Parse("00000000-0000-0000-0002-000000000004");
    private static readonly Guid Pay5 = Guid.Parse("00000000-0000-0000-0002-000000000005");
    private static readonly Guid Pay6 = Guid.Parse("00000000-0000-0000-0002-000000000006");
    private static readonly Guid Pay7 = Guid.Parse("00000000-0000-0000-0002-000000000007");
    private static readonly Guid Pay8 = Guid.Parse("00000000-0000-0000-0002-000000000008");
    private static readonly Guid Pay9 = Guid.Parse("00000000-0000-0000-0002-000000000009");
    private static readonly Guid Pay10 = Guid.Parse("00000000-0000-0000-0002-000000000010");

    // ── Reviews ─────────────────────────────────────────────
    private static readonly Guid Rev1 = Guid.Parse("00000000-0000-0000-0003-000000000001");
    private static readonly Guid Rev2 = Guid.Parse("00000000-0000-0000-0003-000000000002");
    private static readonly Guid Rev3 = Guid.Parse("00000000-0000-0000-0003-000000000003");
    private static readonly Guid Rev4 = Guid.Parse("00000000-0000-0000-0003-000000000004");
    private static readonly Guid Rev5 = Guid.Parse("00000000-0000-0000-0003-000000000005");
    private static readonly Guid Rev6 = Guid.Parse("00000000-0000-0000-0003-000000000006");
    private static readonly Guid Rev7 = Guid.Parse("00000000-0000-0000-0003-000000000007");
    private static readonly Guid Rev8 = Guid.Parse("00000000-0000-0000-0003-000000000008");

    public static void Seed(ModelBuilder b)
    {
        var dt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // ══════════════════════════════════════════════════════
        // USERS
        // ══════════════════════════════════════════════════════

        // ── Admin User ────────────────────────────────────────
        b.Entity<User>().HasData(new User
        {
            Id = AdminId,
            FullName = "Nguyễn Minh Nhật",
            Email = "admin@luxehair.vn",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedAdminPassword),
            Phone = "0900000001",
            Role = Roles.Admin,
            IsActive = true,
            CreatedAt = dt
        });

        // ── Staff Users ───────────────────────────────────────
        b.Entity<User>().HasData(
            new User
            {
                Id = StaffU1,
                FullName = "Nguyễn Minh Tài",
                Email = "tai.nguyen@luxehair.vn",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedStaffPassword),
                Phone = "0900000002",
                Role = Roles.Staff,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = StaffU2,
                FullName = "Stylist — Trần Hoàng Nam",
                Email = "nam.tran@luxehair.vn",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedStaffPassword),
                Phone = "0900000003",
                Role = Roles.Staff,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = StaffU3,
                FullName = "Stylist — Lê Thị Hương",
                Email = "huong.le@luxehair.vn",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedStaffPassword),
                Phone = "0900000004",
                Role = Roles.Staff,
                IsActive = true,
                CreatedAt = dt
            }
        );

        // ── Customer Users ─────────────────────────────────────
        b.Entity<User>().HasData(
            new User
            {
                Id = Cust1,
                FullName = "Khách hàng — Phạm Thị Mai",
                Email = "mai.pham@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0912345678",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust2,
                FullName = "Lê Văn Hùng",
                Email = "hung.le@yahoo.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0923456789",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust3,
                FullName = "Trần Minh Châu",
                Email = "chau.tran@outlook.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0934567890",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust4,
                FullName = "Nguyễn Hoàng Phi",
                Email = "phi.nguyen@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0945678901",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust5,
                FullName = "Võ Thị Lan",
                Email = "lan.vo@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0956789012",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust6,
                FullName = "Đặng Minh Tuấn",
                Email = "tuan.dang@email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0967890123",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust7,
                FullName = "Bùi Thị Hòa",
                Email = "hoa.bui@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0978901234",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust8,
                FullName = "Cao Đức Anh",
                Email = "anh.cao@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0989012345",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust9,
                FullName = "Trịnh Thu Hà",
                Email = "ha.trinh@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0990123456",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            },
            new User
            {
                Id = Cust10,
                FullName = "Hoàng Gia Bảo",
                Email = "bao.hoang@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(SeedCustomerPassword),
                Phone = "0901234567",
                Role = Roles.Customer,
                IsActive = true,
                CreatedAt = dt
            }
        );

        // ══════════════════════════════════════════════════════
        // STAFF PROFILES
        // ══════════════════════════════════════════════════════

        b.Entity<Staff>().HasData(
            new Staff
            {
                Id = Staff1Id,
                UserId = StaffU1,
                Specialty = "Cắt & Nhuộm",
                Bio = "8 năm kinh nghiệm, chuyên tóc Hàn Quốc và tóc nam tính. Đã làm việc tại nhiều salon cao cấp ở Seoul.",
                AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=400&fit=crop&crop=face",
                IsAvailable = true,
                CreatedAt = dt
            },
            new Staff
            {
                Id = Staff2Id,
                UserId = StaffU2,
                Specialty = "Uốn & Duỗi",
                Bio = "Chuyên gia uốn xoăn, duỗi tóc với kỹ thuật Nhật Bản. Tốt nghiệp trường làm đẹp Tokyo.",
                AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&h=400&fit=crop&crop=face",
                IsAvailable = true,
                CreatedAt = dt
            },
            new Staff
            {
                Id = Staff3Id,
                UserId = StaffU3,
                Specialty = "Nhuộm & Tạo Mẫu",
                Bio = "5 năm kinh nghiệm, chuyên nhuộm highlight và ombre. Được đào tạo bài bản về kỹ thuật nhuộm châu Âu.",
                AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400&h=400&fit=crop&crop=face",
                IsAvailable = true,
                CreatedAt = dt
            }
        );

        // ══════════════════════════════════════════════════════
        // WORKING HOURS (CN–T7: 0=Chủ nhật … 6=Thứ bảy)
        // ══════════════════════════════════════════════════════

        for (int day = 0; day <= 6; day++)
        {
            var isSunday = day == 0;
            b.Entity<WorkingHour>().HasData(
                new WorkingHour
                {
                    Id = Guid.Parse($"00000000-0000-0000-0001-{day:D12}"),
                    StaffId = Staff1Id,
                    DayOfWeek = day,
                    StartTime = TimeSpan.FromHours(isSunday ? 9 : 8),
                    EndTime = TimeSpan.FromHours(isSunday ? 17 : 18)
                },
                new WorkingHour
                {
                    Id = Guid.Parse($"00000000-0000-0000-0002-{day:D12}"),
                    StaffId = Staff2Id,
                    DayOfWeek = day,
                    StartTime = TimeSpan.FromHours(isSunday ? 9 : 9),
                    EndTime = TimeSpan.FromHours(isSunday ? 17 : 18)
                },
                new WorkingHour
                {
                    Id = Guid.Parse($"00000000-0000-0000-0003-{day:D12}"),
                    StaffId = Staff3Id,
                    DayOfWeek = day,
                    StartTime = TimeSpan.FromHours(isSunday ? 9 : 8),
                    EndTime = TimeSpan.FromHours(isSunday ? 17 : 17)
                }
            );
        }

        // ══════════════════════════════════════════════════════
        // SERVICES
        // ══════════════════════════════════════════════════════

        b.Entity<Service>().HasData(
            new Service
            {
                Id = Svc1,
                Name = "Cắt Tóc Nam",
                Description = "Cắt kiểu theo yêu cầu với các style hiện đại, gội sấy hoàn chỉnh, massage da đầu. Bao gồm tư vấn kiểu tóc phù hợp với khuôn mặt.",
                ImageUrl = "https://images.unsplash.com/photo-1599351431202-1e0f0137899a?w=800&h=600&fit=crop",
                Price = 120_000,
                DurationMinutes = 45,
                IsActive = true,
                CreatedAt = dt
            },
            new Service
            {
                Id = Svc2,
                Name = "Cắt Tóc Nữ",
                Description = "Cắt tạo kiểu theo xu hướng, tư vấn phong cách phù hợp với khuôn mặt. Sử dụng sản phẩm chăm sóc tóc cao cấp.",
                ImageUrl = "https://images.unsplash.com/photo-1522337360788-8b13dee7a37e?w=800&h=600&fit=crop",
                Price = 150_000,
                DurationMinutes = 60,
                IsActive = true,
                CreatedAt = dt
            },
            new Service
            {
                Id = Svc3,
                Name = "Uốn Tóc Xoăn",
                Description = "Uốn xoăn tự nhiên, sử dụng sản phẩm cao cấp từ Hàn Quốc, bảo hành 1 tháng. Tạo kiểu xoăn sóng biển hoặc xoăn bông.",
                ImageUrl = "https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?w=800&h=600&fit=crop",
                Price = 450_000,
                DurationMinutes = 180,
                IsActive = true,
                CreatedAt = dt
            },
            new Service
            {
                Id = Svc4,
                Name = "Nhuộm Tóc",
                Description = "Nhuộm toàn bộ hoặc highlight, sử dụng thuốc nhuộm hữu cơ Organic. Màu bền đẹp, ít hư tổn tóc.",
                ImageUrl = "https://images.unsplash.com/photo-1620331311520-246422fd82f9?w=800&h=600&fit=crop",
                Price = 350_000,
                DurationMinutes = 120,
                IsActive = true,
                CreatedAt = dt
            },
            new Service
            {
                Id = Svc5,
                Name = "Gội Đầu Massage",
                Description = "Gội dưỡng chất, massage da đầu thư giãn với tinh dầu thiên nhiên. Giúp giảm stress và cải thiện tuần hoàn máu.",
                ImageUrl = "https://images.unsplash.com/photo-1634449571010-02389ed0f9b0?w=800&h=600&fit=crop",
                Price = 80_000,
                DurationMinutes = 30,
                IsActive = true,
                CreatedAt = dt
            },
            new Service
            {
                Id = Svc6,
                Name = "Duỗi Tóc",
                Description = "Duỗi nano collagen, phục hồi tóc hư tổn, kết quả mềm mượt tự nhiên. Bảo hành 3 tháng.",
                ImageUrl = "https://images.unsplash.com/photo-1605497788044-5a32c7078486?w=800&h=600&fit=crop",
                Price = 500_000,
                DurationMinutes = 180,
                IsActive = true,
                CreatedAt = dt
            }
        );

        // ══════════════════════════════════════════════════════
        // STAFF ↔ SERVICE MAPPING
        // ══════════════════════════════════════════════════════

        b.Entity<StaffService>().HasData(
            // Staff 1 - Cắt & Nhuộm (Nguyễn Minh Tài)
            new StaffService { StaffId = Staff1Id, ServiceId = Svc1 },
            new StaffService { StaffId = Staff1Id, ServiceId = Svc2 },
            new StaffService { StaffId = Staff1Id, ServiceId = Svc4 },
            new StaffService { StaffId = Staff1Id, ServiceId = Svc5 },
            // Staff 2 - Uốn & Duỗi (Trần Hoàng Nam)
            new StaffService { StaffId = Staff2Id, ServiceId = Svc1 },
            new StaffService { StaffId = Staff2Id, ServiceId = Svc2 },
            new StaffService { StaffId = Staff2Id, ServiceId = Svc3 },
            new StaffService { StaffId = Staff2Id, ServiceId = Svc6 },
            new StaffService { StaffId = Staff2Id, ServiceId = Svc5 },
            // Staff 3 - Nhuộm & Tạo mẫu (Lê Thị Hương)
            new StaffService { StaffId = Staff3Id, ServiceId = Svc2 },
            new StaffService { StaffId = Staff3Id, ServiceId = Svc3 },
            new StaffService { StaffId = Staff3Id, ServiceId = Svc4 },
            new StaffService { StaffId = Staff3Id, ServiceId = Svc5 }
        );

        // ══════════════════════════════════════════════════════
        // APPOINTMENTS
        // ══════════════════════════════════════════════════════

        // Past appointments (Completed)
        b.Entity<Appointment>().HasData(
            new Appointment
            {
                Id = Appt1,
                CustomerId = Cust1,
                StaffId = Staff1Id,
                ServiceId = Svc2,
                AppointmentDate = new DateTime(2024, 3, 15, 9, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT001",
                Notes = "Cắt tóc ngắn, tạo kiểu bob",
                CreatedAt = new DateTime(2024, 3, 10, 10, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt2,
                CustomerId = Cust2,
                StaffId = Staff1Id,
                ServiceId = Svc1,
                AppointmentDate = new DateTime(2024, 3, 16, 10, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT002",
                Notes = "Cắt kiểu undercut nam",
                CreatedAt = new DateTime(2024, 3, 12, 14, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt3,
                CustomerId = Cust3,
                StaffId = Staff3Id,
                ServiceId = Svc4,
                AppointmentDate = new DateTime(2024, 3, 18, 11, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT003",
                Notes = "Nhuộm highlight nâu mật ong",
                CreatedAt = new DateTime(2024, 3, 14, 9, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt4,
                CustomerId = Cust4,
                StaffId = Staff2Id,
                ServiceId = Svc3,
                AppointmentDate = new DateTime(2024, 3, 20, 14, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT004",
                Notes = "Uốn xoăn sóng biển",
                CreatedAt = new DateTime(2024, 3, 15, 11, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt5,
                CustomerId = Cust5,
                StaffId = Staff3Id,
                ServiceId = Svc2,
                AppointmentDate = new DateTime(2024, 3, 22, 9, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT005",
                Notes = "Cắt tóc mái bay",
                CreatedAt = new DateTime(2024, 3, 18, 15, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt6,
                CustomerId = Cust6,
                StaffId = Staff1Id,
                ServiceId = Svc4,
                AppointmentDate = new DateTime(2024, 3, 25, 10, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT006",
                Notes = "Nhuộm toàn bộ màu nâu caramel",
                CreatedAt = new DateTime(2024, 3, 20, 8, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt7,
                CustomerId = Cust7,
                StaffId = Staff2Id,
                ServiceId = Svc6,
                AppointmentDate = new DateTime(2024, 4, 1, 8, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT007",
                Notes = "Duỗi tóc thẳng mượt",
                CreatedAt = new DateTime(2024, 3, 28, 12, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt8,
                CustomerId = Cust8,
                StaffId = Staff1Id,
                ServiceId = Svc5,
                AppointmentDate = new DateTime(2024, 4, 5, 15, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Completed,
                BookingCode = "APT008",
                Notes = "Gội massage thư giãn",
                CreatedAt = new DateTime(2024, 4, 3, 10, 0, 0, DateTimeKind.Utc)
            },
            // Cancelled appointments
            new Appointment
            {
                Id = Appt9,
                CustomerId = Cust9,
                StaffId = Staff2Id,
                ServiceId = Svc3,
                AppointmentDate = new DateTime(2024, 4, 10, 9, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Cancelled,
                BookingCode = "APT009",
                Notes = "Khách hủy lịch",
                CreatedAt = new DateTime(2024, 4, 5, 14, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt10,
                CustomerId = Cust10,
                StaffId = Staff3Id,
                ServiceId = Svc4,
                AppointmentDate = new DateTime(2024, 4, 12, 11, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Cancelled,
                BookingCode = "APT010",
                Notes = "Salon hủy do nhân viên nghỉ ốm",
                CreatedAt = new DateTime(2024, 4, 8, 9, 0, 0, DateTimeKind.Utc)
            },
            // Confirmed/Pending appointments (upcoming)
            new Appointment
            {
                Id = Appt11,
                CustomerId = Cust1,
                StaffId = Staff3Id,
                ServiceId = Svc3,
                AppointmentDate = new DateTime(2026, 5, 10, 10, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Confirmed,
                BookingCode = "APT011",
                Notes = "Uốn xoăn lọn công chúa",
                CreatedAt = new DateTime(2026, 5, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt12,
                CustomerId = Cust3,
                StaffId = Staff1Id,
                ServiceId = Svc1,
                AppointmentDate = new DateTime(2026, 5, 12, 9, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Pending,
                BookingCode = "APT012",
                Notes = "Cắt tóc fade",
                CreatedAt = new DateTime(2026, 5, 5, 11, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt13,
                CustomerId = Cust5,
                StaffId = Staff2Id,
                ServiceId = Svc4,
                AppointmentDate = new DateTime(2026, 5, 15, 14, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Confirmed,
                BookingCode = "APT013",
                Notes = "Nhuộm ombre xanh navy",
                CreatedAt = new DateTime(2026, 5, 6, 15, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt14,
                CustomerId = Cust7,
                StaffId = Staff1Id,
                ServiceId = Svc2,
                AppointmentDate = new DateTime(2026, 5, 18, 10, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Pending,
                BookingCode = "APT014",
                Notes = "Cắt tóc tầng",
                CreatedAt = new DateTime(2026, 5, 7, 9, 0, 0, DateTimeKind.Utc)
            },
            new Appointment
            {
                Id = Appt15,
                CustomerId = Cust9,
                StaffId = Staff3Id,
                ServiceId = Svc5,
                AppointmentDate = new DateTime(2026, 5, 20, 16, 0, 0, DateTimeKind.Utc),
                Status = AppointmentStatus.Confirmed,
                BookingCode = "APT015",
                Notes = "Massage da đầu VIP",
                CreatedAt = new DateTime(2026, 5, 8, 8, 0, 0, DateTimeKind.Utc)
            }
        );

        // ══════════════════════════════════════════════════════
        // PAYMENTS (cho các appointment đã hoàn thành)
        // ══════════════════════════════════════════════════════

        b.Entity<Payment>().HasData(
            new Payment
            {
                Id = Pay1,
                AppointmentId = Appt1,
                Amount = 150_000,
                Method = "Cash",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 15, 10, 30, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay2,
                AppointmentId = Appt2,
                Amount = 120_000,
                Method = "Transfer",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 16, 11, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay3,
                AppointmentId = Appt3,
                Amount = 350_000,
                Method = "Cash",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 18, 13, 30, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay4,
                AppointmentId = Appt4,
                Amount = 450_000,
                Method = "Card",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 20, 18, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay5,
                AppointmentId = Appt5,
                Amount = 150_000,
                Method = "Transfer",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 22, 10, 30, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay6,
                AppointmentId = Appt6,
                Amount = 350_000,
                Method = "Cash",
                Status = "Paid",
                PaidAt = new DateTime(2024, 3, 25, 12, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay7,
                AppointmentId = Appt7,
                Amount = 500_000,
                Method = "Card",
                Status = "Paid",
                PaidAt = new DateTime(2024, 4, 1, 11, 30, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay8,
                AppointmentId = Appt8,
                Amount = 80_000,
                Method = "Cash",
                Status = "Paid",
                PaidAt = new DateTime(2024, 4, 5, 16, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay9,
                AppointmentId = Appt11,
                Amount = 450_000,
                Method = "Transfer",
                Status = "Pending",
                PaidAt = new DateTime(2026, 5, 10, 12, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = Pay10,
                AppointmentId = Appt13,
                Amount = 350_000,
                Method = "Card",
                Status = "Pending",
                PaidAt = new DateTime(2026, 5, 15, 17, 0, 0, DateTimeKind.Utc)
            }
        );

        // ══════════════════════════════════════════════════════
        // REVIEWS (cho các appointment đã hoàn thành)
        // ══════════════════════════════════════════════════════

        b.Entity<Review>().HasData(
            new Review
            {
                Id = Rev1,
                AppointmentId = Appt1,
                CustomerId = Cust1,
                Rating = 5,
                Comment = "Dịch vụ tuyệt vời! Chị Hương cắt tóc rất đẹp, tư vấn nhiệt tình. Sẽ quay lại lần sau!",
                CreatedAt = new DateTime(2024, 3, 16, 10, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev2,
                AppointmentId = Appt2,
                CustomerId = Cust2,
                Rating = 4,
                Comment = "Anh Tài cắt tóc đẹp, nhưng hơi đông nên phải chờ 15 phút. Chất lượng OK.",
                CreatedAt = new DateTime(2024, 3, 17, 14, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev3,
                AppointmentId = Appt3,
                CustomerId = Cust3,
                Rating = 5,
                Comment = "Màu nhuộm y như mong đợi! Tóc mềm mượt, không bị khô. Cảm ơn salon rất nhiều!",
                CreatedAt = new DateTime(2024, 3, 19, 9, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev4,
                AppointmentId = Appt4,
                CustomerId = Cust4,
                Rating = 5,
                Comment = "Uốn xoăn siêu đẹp! Tóc vào nếp hoàn hảo, bồng bềnh tự nhiên. Đáng giá tiền!",
                CreatedAt = new DateTime(2024, 3, 21, 11, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev5,
                AppointmentId = Appt5,
                CustomerId = Cust5,
                Rating = 4,
                Comment = "Kiểu tóc rất hợp với khuôn mặt tôi. Nhân viên dễ thương, không gian đẹp.",
                CreatedAt = new DateTime(2024, 3, 23, 15, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev6,
                AppointmentId = Appt6,
                CustomerId = Cust6,
                Rating = 5,
                Comment = "Nâu caramel siêu xinh! Màu đều, lên màu chuẩn. Lần đầu nhuộm mà hoàn toàn hài lòng!",
                CreatedAt = new DateTime(2024, 3, 26, 10, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev7,
                AppointmentId = Appt7,
                CustomerId = Cust7,
                Rating = 5,
                Comment = "Tóc duỗi thẳng mượt như nhung! Đúng chuẩn như mơ. Sẽ giới thiệu bạn bè!",
                CreatedAt = new DateTime(2024, 4, 2, 9, 0, 0, DateTimeKind.Utc)
            },
            new Review
            {
                Id = Rev8,
                AppointmentId = Appt8,
                CustomerId = Cust8,
                Rating = 4,
                Comment = "Massage da đầu thư giãn cực kỳ! Sau tuần làm việc mệt mỏi thì đây là liệu pháp hoàn hảo.",
                CreatedAt = new DateTime(2024, 4, 6, 11, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
