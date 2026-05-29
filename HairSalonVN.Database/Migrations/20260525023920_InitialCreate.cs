using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HairSalonVN.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Customer"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    BookingCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsGuestBooking = table.Column<bool>(type: "bit", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffServices",
                columns: table => new
                {
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffServices", x => new { x.StaffId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_StaffServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffServices_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingHours_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationMinutes", "ImageUrl", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cắt kiểu theo yêu cầu với các style hiện đại, gội sấy hoàn chỉnh, massage da đầu. Bao gồm tư vấn kiểu tóc phù hợp với khuôn mặt.", 45, "https://images.unsplash.com/photo-1599351431202-1e0f0137899a?w=800&h=600&fit=crop", true, "Cắt Tóc Nam", 120000m },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cắt tạo kiểu theo xu hướng, tư vấn phong cách phù hợp với khuôn mặt. Sử dụng sản phẩm chăm sóc tóc cao cấp.", 60, "https://images.unsplash.com/photo-1522337360788-8b13dee7a37e?w=800&h=600&fit=crop", true, "Cắt Tóc Nữ", 150000m },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Uốn xoăn tự nhiên, sử dụng sản phẩm cao cấp từ Hàn Quốc, bảo hành 1 tháng. Tạo kiểu xoăn sóng biển hoặc xoăn bông.", 180, "https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?w=800&h=600&fit=crop", true, "Uốn Tóc Xoăn", 450000m },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nhuộm toàn bộ hoặc highlight, sử dụng thuốc nhuộm hữu cơ Organic. Màu bền đẹp, ít hư tổn tóc.", 120, "https://images.unsplash.com/photo-1620331311520-246422fd82f9?w=800&h=600&fit=crop", true, "Nhuộm Tóc", 350000m },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gội dưỡng chất, massage da đầu thư giãn với tinh dầu thiên nhiên. Giúp giảm stress và cải thiện tuần hoàn máu.", 30, "https://images.unsplash.com/photo-1634449571010-02389ed0f9b0?w=800&h=600&fit=crop", true, "Gội Đầu Massage", 80000m },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Duỗi nano collagen, phục hồi tóc hư tổn, kết quả mềm mượt tự nhiên. Bảo hành 3 tháng.", 180, "https://images.unsplash.com/photo-1605497788044-5a32c7078486?w=800&h=600&fit=crop", true, "Duỗi Tóc", 500000m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Phone", "Role" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@luxehair.vn", "Admin — Trần Văn Quản", true, "$2a$11$GZWzw9RdwFx8YXOnMUjnculZMCm.NegH54GZ6BYq/oyCqt0qM0JjS", "0900000001", "Admin" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tai.nguyen@luxehair.vn", "Nguyễn Minh Tài", true, "$2a$11$zkpFyZBVOnBblBaSu51xVOb04i8TAEx0tnDLHQac7x6C6bJfk5B96", "0900000002", "Staff" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nam.tran@luxehair.vn", "Stylist — Trần Hoàng Nam", true, "$2a$11$ddvqpWXmCheKT.GBDmpsluNWdPinaSF.twXe8wTN1TXLLp6vDfVyK", "0900000003", "Staff" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "huong.le@luxehair.vn", "Stylist — Lê Thị Hương", true, "$2a$11$NL69sNdqnweriVabkq7ALeFq6VgOmeYCZ.TV/cgm1JVVv1YOUcC62", "0900000004", "Staff" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mai.pham@gmail.com", "Khách hàng — Phạm Thị Mai", true, "$2a$11$b0CpDl.qFSYYxc01cEA0TOacHUkRQU7OW23RGpuNrGmAKKTgthuc.", "0912345678", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hung.le@yahoo.com", "Lê Văn Hùng", true, "$2a$11$SATwcUDIC2BaJHA5FTPeKOF7A27APXARUEiU6obaNjTCKMz6081MK", "0923456789", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "chau.tran@outlook.com", "Trần Minh Châu", true, "$2a$11$ZzmRlcYO7JOPEB1vknwADOg/BPR5YZdSDfJosnPlxafAe3bjGEl4S", "0934567890", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "phi.nguyen@gmail.com", "Nguyễn Hoàng Phi", true, "$2a$11$b8qZoxwSI72WB2Oc9NG3wub7gpWw2ESkwl1yLgR6nMNl2JIOVH2Tm", "0945678901", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "lan.vo@gmail.com", "Võ Thị Lan", true, "$2a$11$vg5xH8JWAT2g.GqFKVFtlufbLcZGGdO/PieZuAWtHOzr20PRt3t/u", "0956789012", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "tuan.dang@email.com", "Đặng Minh Tuấn", true, "$2a$11$pY1G1j9xUDao2qizKldTVeMpyQRg0j4PDuRQmCHRQwq9fcw7RB5R2", "0967890123", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hoa.bui@gmail.com", "Bùi Thị Hòa", true, "$2a$11$.6o38CO8ZPzWsI.iSFyOjuo0XqZlXWG0cMCznn0tvlGSK679GaNPC", "0978901234", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000027"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "anh.cao@gmail.com", "Cao Đức Anh", true, "$2a$11$x9CDVlyjk5q6dQMFmOvF6.XEuIcjp/HrlwjYtjUfy9nOOeA3j4asi", "0989012345", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000028"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ha.trinh@gmail.com", "Trịnh Thu Hà", true, "$2a$11$gJTweringooQ5kTr5CIwAeel63JR9/FwbnzmX6rkRIeRuGqKp1JOC", "0990123456", "Customer" },
                    { new Guid("00000000-0000-0000-0000-000000000029"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "bao.hoang@gmail.com", "Hoàng Gia Bảo", true, "$2a$11$Zy0kY12Pb7.0kLnxSgfoEOiaF1KHR6RJfUx.rkun0AhGXYDKl7tkm", "0901234567", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "AvatarUrl", "Bio", "CreatedAt", "IsAvailable", "Specialty", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=400&fit=crop&crop=face", "8 năm kinh nghiệm, chuyên tóc Hàn Quốc và tóc nam tính. Đã làm việc tại nhiều salon cao cấp ở Seoul.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Cắt & Nhuộm", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&h=400&fit=crop&crop=face", "Chuyên gia uốn xoăn, duỗi tóc với kỹ thuật Nhật Bản. Tốt nghiệp trường làm đẹp Tokyo.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Uốn & Duỗi", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400&h=400&fit=crop&crop=face", "5 năm kinh nghiệm, chuyên nhuộm highlight và ombre. Được đào tạo bài bản về kỹ thuật nhuộm châu Âu.", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Nhuộm & Tạo Mẫu", new Guid("00000000-0000-0000-0000-000000000007") }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "BookingCode", "CreatedAt", "CustomerId", "GuestEmail", "GuestName", "GuestPhone", "IsGuestBooking", "Notes", "ServiceId", "StaffId", "Status" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0001-000000000001"), new DateTime(2024, 3, 15, 9, 0, 0, 0, DateTimeKind.Utc), "APT001", new DateTime(2024, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, false, "Cắt tóc ngắn, tạo kiểu bob", new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000002"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000002"), new DateTime(2024, 3, 16, 10, 0, 0, 0, DateTimeKind.Utc), "APT002", new DateTime(2024, 3, 12, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000021"), null, null, null, false, "Cắt kiểu undercut nam", new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000002"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000003"), new DateTime(2024, 3, 18, 11, 0, 0, 0, DateTimeKind.Utc), "APT003", new DateTime(2024, 3, 14, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, false, "Nhuộm highlight nâu mật ong", new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000006"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000004"), new DateTime(2024, 3, 20, 14, 0, 0, 0, DateTimeKind.Utc), "APT004", new DateTime(2024, 3, 15, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000023"), null, null, null, false, "Uốn xoăn sóng biển", new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000004"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000005"), new DateTime(2024, 3, 22, 9, 0, 0, 0, DateTimeKind.Utc), "APT005", new DateTime(2024, 3, 18, 15, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, false, "Cắt tóc mái bay", new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000006"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000006"), new DateTime(2024, 3, 25, 10, 0, 0, 0, DateTimeKind.Utc), "APT006", new DateTime(2024, 3, 20, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000025"), null, null, null, false, "Nhuộm toàn bộ màu nâu caramel", new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000002"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000007"), new DateTime(2024, 4, 1, 8, 0, 0, 0, DateTimeKind.Utc), "APT007", new DateTime(2024, 3, 28, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, false, "Duỗi tóc thẳng mượt", new Guid("00000000-0000-0000-0000-000000000015"), new Guid("00000000-0000-0000-0000-000000000004"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000008"), new DateTime(2024, 4, 5, 15, 0, 0, 0, DateTimeKind.Utc), "APT008", new DateTime(2024, 4, 3, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000027"), null, null, null, false, "Gội massage thư giãn", new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000002"), "Completed" },
                    { new Guid("00000000-0000-0000-0001-000000000009"), new DateTime(2024, 4, 10, 9, 0, 0, 0, DateTimeKind.Utc), "APT009", new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, false, "Khách hủy lịch", new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000004"), "Cancelled" },
                    { new Guid("00000000-0000-0000-0001-000000000010"), new DateTime(2024, 4, 12, 11, 0, 0, 0, DateTimeKind.Utc), "APT010", new DateTime(2024, 4, 8, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000029"), null, null, null, false, "Salon hủy do nhân viên nghỉ ốm", new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000006"), "Cancelled" },
                    { new Guid("00000000-0000-0000-0001-000000000011"), new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc), "APT011", new DateTime(2026, 5, 1, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000020"), null, null, null, false, "Uốn xoăn lọn công chúa", new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000006"), "Confirmed" },
                    { new Guid("00000000-0000-0000-0001-000000000012"), new DateTime(2026, 5, 12, 9, 0, 0, 0, DateTimeKind.Utc), "APT012", new DateTime(2026, 5, 5, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000022"), null, null, null, false, "Cắt tóc fade", new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000002"), "Pending" },
                    { new Guid("00000000-0000-0000-0001-000000000013"), new DateTime(2026, 5, 15, 14, 0, 0, 0, DateTimeKind.Utc), "APT013", new DateTime(2026, 5, 6, 15, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000024"), null, null, null, false, "Nhuộm ombre xanh navy", new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000004"), "Confirmed" },
                    { new Guid("00000000-0000-0000-0001-000000000014"), new DateTime(2026, 5, 18, 10, 0, 0, 0, DateTimeKind.Utc), "APT014", new DateTime(2026, 5, 7, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000026"), null, null, null, false, "Cắt tóc tầng", new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000002"), "Pending" },
                    { new Guid("00000000-0000-0000-0001-000000000015"), new DateTime(2026, 5, 20, 16, 0, 0, 0, DateTimeKind.Utc), "APT015", new DateTime(2026, 5, 8, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000028"), null, null, null, false, "Massage da đầu VIP", new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000006"), "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "StaffServices",
                columns: new[] { "ServiceId", "StaffId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new Guid("00000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "WorkingHours",
                columns: new[] { "Id", "DayOfWeek", "EndTime", "StaffId", "StartTime" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0001-000000000000"), 0, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000001"), 1, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000002"), 2, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000003"), 3, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000004"), 4, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000005"), 5, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0001-000000000006"), 6, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000002"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000000"), 0, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000001"), 1, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000002"), 2, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000003"), 3, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000004"), 4, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000005"), 5, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0002-000000000006"), 6, new TimeSpan(0, 18, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000004"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000000"), 0, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 9, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000001"), 1, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000002"), 2, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000003"), 3, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000004"), 4, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000005"), 5, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) },
                    { new Guid("00000000-0000-0000-0003-000000000006"), 6, new TimeSpan(0, 17, 0, 0, 0), new Guid("00000000-0000-0000-0000-000000000006"), new TimeSpan(0, 8, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "AppointmentId", "Method", "PaidAt", "Status" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0002-000000000001"), 150000m, new Guid("00000000-0000-0000-0001-000000000001"), "Cash", new DateTime(2024, 3, 15, 10, 30, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000002"), 120000m, new Guid("00000000-0000-0000-0001-000000000002"), "Transfer", new DateTime(2024, 3, 16, 11, 0, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000003"), 350000m, new Guid("00000000-0000-0000-0001-000000000003"), "Cash", new DateTime(2024, 3, 18, 13, 30, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000004"), 450000m, new Guid("00000000-0000-0000-0001-000000000004"), "Card", new DateTime(2024, 3, 20, 18, 0, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000005"), 150000m, new Guid("00000000-0000-0000-0001-000000000005"), "Transfer", new DateTime(2024, 3, 22, 10, 30, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000006"), 350000m, new Guid("00000000-0000-0000-0001-000000000006"), "Cash", new DateTime(2024, 3, 25, 12, 0, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000007"), 500000m, new Guid("00000000-0000-0000-0001-000000000007"), "Card", new DateTime(2024, 4, 1, 11, 30, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000008"), 80000m, new Guid("00000000-0000-0000-0001-000000000008"), "Cash", new DateTime(2024, 4, 5, 16, 0, 0, 0, DateTimeKind.Utc), "Paid" },
                    { new Guid("00000000-0000-0000-0002-000000000009"), 450000m, new Guid("00000000-0000-0000-0001-000000000011"), "Transfer", new DateTime(2026, 5, 10, 12, 0, 0, 0, DateTimeKind.Utc), "Pending" },
                    { new Guid("00000000-0000-0000-0002-000000000010"), 350000m, new Guid("00000000-0000-0000-0001-000000000013"), "Card", new DateTime(2026, 5, 15, 17, 0, 0, 0, DateTimeKind.Utc), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AppointmentId", "Comment", "CreatedAt", "CustomerId", "Rating" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0003-000000000001"), new Guid("00000000-0000-0000-0001-000000000001"), "Dịch vụ tuyệt vời! Chị Hương cắt tóc rất đẹp, tư vấn nhiệt tình. Sẽ quay lại lần sau!", new DateTime(2024, 3, 16, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000020"), 5 },
                    { new Guid("00000000-0000-0000-0003-000000000002"), new Guid("00000000-0000-0000-0001-000000000002"), "Anh Tài cắt tóc đẹp, nhưng hơi đông nên phải chờ 15 phút. Chất lượng OK.", new DateTime(2024, 3, 17, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000021"), 4 },
                    { new Guid("00000000-0000-0000-0003-000000000003"), new Guid("00000000-0000-0000-0001-000000000003"), "Màu nhuộm y như mong đợi! Tóc mềm mượt, không bị khô. Cảm ơn salon rất nhiều!", new DateTime(2024, 3, 19, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000022"), 5 },
                    { new Guid("00000000-0000-0000-0003-000000000004"), new Guid("00000000-0000-0000-0001-000000000004"), "Uốn xoăn siêu đẹp! Tóc vào nếp hoàn hảo, bồng bềnh tự nhiên. Đáng giá tiền!", new DateTime(2024, 3, 21, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000023"), 5 },
                    { new Guid("00000000-0000-0000-0003-000000000005"), new Guid("00000000-0000-0000-0001-000000000005"), "Kiểu tóc rất hợp với khuôn mặt tôi. Nhân viên dễ thương, không gian đẹp.", new DateTime(2024, 3, 23, 15, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000024"), 4 },
                    { new Guid("00000000-0000-0000-0003-000000000006"), new Guid("00000000-0000-0000-0001-000000000006"), "Nâu caramel siêu xinh! Màu đều, lên màu chuẩn. Lần đầu nhuộm mà hoàn toàn hài lòng!", new DateTime(2024, 3, 26, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000025"), 5 },
                    { new Guid("00000000-0000-0000-0003-000000000007"), new Guid("00000000-0000-0000-0001-000000000007"), "Tóc duỗi thẳng mượt như nhung! Đúng chuẩn như mơ. Sẽ giới thiệu bạn bè!", new DateTime(2024, 4, 2, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000026"), 5 },
                    { new Guid("00000000-0000-0000-0003-000000000008"), new Guid("00000000-0000-0000-0001-000000000008"), "Massage da đầu thư giãn cực kỳ! Sau tuần làm việc mệt mỏi thì đây là liệu pháp hoàn hảo.", new DateTime(2024, 4, 6, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000027"), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BookingCode",
                table: "Appointments",
                column: "BookingCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StaffId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "StaffId", "AppointmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AppointmentId",
                table: "Payments",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppointmentId",
                table: "Reviews",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CustomerId",
                table: "Reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                table: "Staff",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffServices_ServiceId",
                table: "StaffServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_StaffId_DayOfWeek",
                table: "WorkingHours",
                columns: new[] { "StaffId", "DayOfWeek" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "StaffServices");

            migrationBuilder.DropTable(
                name: "WorkingHours");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
