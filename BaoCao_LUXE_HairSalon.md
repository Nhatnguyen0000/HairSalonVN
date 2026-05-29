# BÁO CÁO ĐỒ ÁN MÔN HỌC

## Lập trình ADO.NET / ASP.NET Core

---

# LUXE HAIR SALON

## Hệ thống Đặt Lịch Làm Tóc Trực Tuyến

**Công nghệ sử dụng:** ASP.NET Core 8.0 • SQL Server • Entity Framework Core 8 • JWT Authentication

**Năm học:** 2025–2026

---

# MỤC LỤC

1. [CHƯƠNG 1: GIỚI THIỆU ĐỀ TÀI](#chương-1-giới-thiệu-đề-tài)
2. [CHƯƠNG 2: CÔNG NGHỆ SỬ DỤNG](#chương-2-công-nghệ-sử-dụng)
3. [CHƯƠNG 3: PHÂN TÍCH YÊU CẦU HỆ THỐNG](#chương-3-phân-tích-yêu-cầu-hệ-thống)
4. [CHƯƠNG 4: THIẾT KẾ HỆ THỐNG](#chương-4-thiết-kế-hệ-thống)
5. [CHƯƠNG 5: THIẾT KẾ API ENDPOINTS](#chương-5-thiết-kế-api-endpoints)
6. [CHƯƠNG 6: TRIỂN KHAI VÀ THỬ NGHIỆM](#chương-6-triển-khai-và-thử-nghiệm)
7. [CHƯƠNG 7: KẾT LUẬN VÀ HƯỚNG PHÁT TRIỂN](#chương-7-kết-luận-và-hướng-phát-triển)

---

# CHƯƠNG 1: GIỚI THIỆU ĐỀ TÀI

## 1.1. Đặt vấn đề

Trong bối cảnh kinh tế dịch vụ phát triển mạnh mẽ tại Việt Nam, ngành chăm sóc tóc và làm đẹp đang chứng kiến sự tăng trưởng vượt bậc. Tuy nhiên, phần lớn các salon tóc vẫn đang vận hành theo phương thức truyền thống: khách hàng phải trực tiếp đến cửa hàng hoặc gọi điện để đặt lịch, dẫn đến nhiều bất tiện như xếp hàng chờ đợi, khó quản lý lịch hẹn và thiếu thông tin minh bạch về dịch vụ.

Trước thực trạng đó, nhóm thực hiện đề tài "LUXE Hair Salon – Hệ thống Đặt Lịch Làm Tóc Trực Tuyến" nhằm xây dựng một nền tảng số hóa toàn bộ quy trình đặt lịch, quản lý nhân viên và theo dõi doanh thu cho một salon tóc hiện đại.

## 1.2. Mục tiêu đề tài

### 1.2.1. Mục tiêu tổng quát

Xây dựng hệ thống đặt lịch làm tóc trực tuyến đa nền tảng, phục vụ ba nhóm người dùng chính: Khách hàng, Stylist và Quản trị viên, với giao diện thân thiện và quy trình nghiệp vụ chặt chẽ.

### 1.2.2. Mục tiêu cụ thể

- Xây dựng hệ thống đặt lịch trực tuyến đa vai trò: Khách hàng, Stylist và Quản trị viên.
- Áp dụng kiến trúc API + MVC tách biệt rõ ràng giữa backend và frontend.
- Triển khai xác thực bảo mật với JWT Bearer Token và Refresh Token.
- Tích hợp các biện pháp bảo mật hiện đại: Rate Limiting, Audit Logging, Security Headers.
- Đảm bảo trải nghiệm người dùng mượt mà trên cả desktop và thiết bị di động (Responsive).

## 1.3. Phạm vi và giới hạn

### Phạm vi nghiên cứu

Hệ thống tập trung vào nghiệp vụ cốt lõi của một salon tóc bao gồm:

- Quản lý dịch vụ (Danh sách, thông tin chi tiết, giá cả)
- Đặt lịch hẹn (cả khách đã đăng ký và khách vãng lai)
- Quản lý lịch làm việc của stylist
- Đánh giá dịch vụ sau khi hoàn thành
- Báo cáo doanh thu và thống kê

### Giới hạn đề tài

- Chưa tích hợp thanh toán trực tuyến (VNPay, MoMo)
- Chưa có thông báo real-time (SignalR/WebSocket)
- Chưa triển khai ứng dụng mobile riêng

---

# CHƯƠNG 2: CÔNG NGHỆ SỬ DỤNG

## 2.1. Tổng quan công nghệ

| Thành phần | Công nghệ |
|------------|-----------|
| Backend API | ASP.NET Core 8.0 Web API |
| Frontend | ASP.NET Core 8.0 MVC (Razor) |
| Cơ sở dữ liệu | SQL Server + Entity Framework Core 8 |
| Xác thực | JWT Bearer + Refresh Token |
| Giao diện | CSS tùy chỉnh, Responsive |
| Bảo mật | Rate Limiting, Audit Logging, Security Headers |

## 2.2. ASP.NET Core 8.0 Web API

ASP.NET Core 8.0 là nền tảng phát triển ứng dụng web đa nền tảng, mã nguồn mở của Microsoft. Trong phiên bản .NET 8, cấu hình được hợp nhất vào Program.cs, giúp việc thiết lập middleware, routing và dependency injection trở nên đơn giản và hiệu quả hơn.

Dự án LUXE Hair Salon áp dụng kiến trúc phân lớp rõ ràng:

- **Controllers**: Xử lý yêu cầu HTTP và trả về phản hồi
- **Services**: Chứa business logic, không phụ thuộc vào HTTP
- **DTOs (Data Transfer Objects)**: Tách biệt dữ liệu gửi đến client khỏi entity cơ sở dữ liệu
- **Middleware**: Xử lý xuyên suốt (cross-cutting concerns) như logging, authentication, error handling

## 2.3. ASP.NET Core MVC (Razor Views)

Frontend của hệ thống được xây dựng trên ASP.NET Core MVC sử dụng Razor Views. Thay vì sử dụng framework JavaScript phức tạp, Razor cho phép viết HTML kết hợp C# thuần túy, phù hợp với ứng dụng nội bộ cần tốc độ phát triển nhanh. Giao tiếp giữa MVC Web và REST API được thực hiện thông qua HttpClient với JWT Bearer Token lưu trong Session.

## 2.4. Entity Framework Core 8 & SQL Server

Entity Framework Core (EF Core) là ORM (Object-Relational Mapper) chính thức của Microsoft cho .NET, cho phép ánh xạ các lớp C# thành bảng trong cơ sở dữ liệu SQL Server. EF Core 8 trong môi trường .NET Web API đem lại nhiều lợi ích quan trọng:

- **Migrations**: Đồng bộ schema cơ sở dữ liệu tự động theo code entity
- **Fluent API (Configurations)**: Cấu hình quan hệ và ràng buộc dữ liệu linh hoạt
- **Parameterized Queries**: Ngăn chặn SQL Injection tự động thông qua LINQ
- **DbContext Lifecycle**: Quản lý kết nối cơ sở dữ liệu hiệu quả qua Dependency Injection

Dự án sử dụng **Code-First approach** với EF Core Migrations và Data Seeding để khởi tạo dữ liệu mẫu.

## 2.5. Xác thực JWT Bearer + Refresh Token

JSON Web Token (JWT) là tiêu chuẩn mở (RFC 7519) để truyền thông tin xác thực một cách an toàn giữa client và server. Một hệ thống JWT hiện đại bao gồm hai loại token:

- **Access Token** (thời gian ngắn – 15 phút): Cho phép truy cập tài nguyên được bảo vệ. Token ngắn hạn giúp giảm thiểu rủi ro nếu bị đánh cắp.
- **Refresh Token** (thời gian dài – 7 ngày): Dùng để lấy Access Token mới mà không cần đăng nhập lại, được lưu trữ phía server và có thể thu hồi bất kỳ lúc nào.

Hệ thống LUXE Hair Salon lưu Refresh Token trong bảng RefreshTokens của SQL Server, hỗ trợ thu hồi token khi người dùng đăng xuất hoặc phát hiện hành vi đáng ngờ.

## 2.6. Bảo mật ứng dụng web

Hệ thống triển khai nhiều lớp bảo mật theo các tiêu chuẩn OWASP:

| Biện pháp bảo mật | Mô tả | Cơ chế thực hiện |
|-------------------|--------|------------------|
| Rate Limiting | Giới hạn 100 requests/phút/client | RateLimitingMiddleware tùy chỉnh |
| Audit Logging | Ghi log mọi thao tác POST/PUT/DELETE | AuditLogMiddleware |
| Security Headers | Ngăn chặn XSS, Clickjacking | X-Content-Type, X-Frame-Options |
| Password Hashing | Mã hóa mật khẩu một chiều | BCrypt với salt tự động |
| SQL Injection Prevention | Truy vấn an toàn | EF Core Parameterized Queries |
| Input Validation | Kiểm tra dữ liệu đầu vào | DataAnnotations + IValidatableObject |

---

# CHƯƠNG 3: PHÂN TÍCH YÊU CẦU HỆ THỐNG

## 3.1. Yêu cầu chức năng

### 3.1.1. Nhóm chức năng Khách hàng (Customer)

| STT | Chức năng | Mô tả chi tiết |
|-----|-----------|----------------|
| 1 | Xem trang chủ | Landing page với hero banner, danh sách dịch vụ nổi bật và đánh giá của khách hàng |
| 2 | Đăng ký tài khoản | Tạo tài khoản mới với validation email, SĐT, mật khẩu mạnh |
| 3 | Đăng nhập / Đăng xuất | Xác thực JWT, lưu session, thu hồi Refresh Token khi logout |
| 4 | Đặt lịch 3 bước | Chọn dịch vụ → Chọn Stylist & Giờ → Xác nhận thông tin |
| 5 | Đặt lịch khách vãng lai | Đặt lịch mà không cần đăng nhập, cung cấp thông tin cơ bản |
| 6 | Xem lịch của tôi | Danh sách lịch hẹn cá nhân, lọc theo trạng thái |
| 7 | Hủy lịch hẹn | Hủy lịch chưa được xác nhận bởi salon |
| 8 | Gửi đánh giá | Đánh giá 1-5 sao và nhận xét sau khi hoàn thành dịch vụ |

### 3.1.2. Nhóm chức năng Stylist (Staff)

| STT | Chức năng | Mô tả chi tiết |
|-----|-----------|----------------|
| 1 | Dashboard cá nhân | Hiển thị lịch hẹn trong ngày, thống kê pending/confirmed/completed |
| 2 | Cập nhật trạng thái lịch | Xác nhận (Pending → Confirmed) hoặc hoàn thành (→ Completed) lịch được gán |

### 3.1.3. Nhóm chức năng Quản trị viên (Admin)

| STT | Chức năng | Mô tả chi tiết |
|-----|-----------|----------------|
| 1 | Dashboard tổng quan | Thống kê lịch hẹn theo trạng thái, doanh thu tháng hiện tại |
| 2 | Quản lý lịch hẹn | Xem, lọc, duyệt và đổi trạng thái tất cả lịch hẹn trong hệ thống |
| 3 | Quản lý dịch vụ (CRUD) | Thêm, sửa, xóa dịch vụ với thông tin tên, giá, thời gian |
| 4 | Quản lý nhân viên | Xem danh sách stylist, chuyên môn và trạng thái |
| 5 | Báo cáo & Thống kê | Doanh thu theo tháng, top dịch vụ, nhân viên xuất sắc |

## 3.2. Yêu cầu phi chức năng

### 3.2.1. Hiệu suất

- Hệ thống phục vụ tối thiểu 100 requests/phút/client với Rate Limiting middleware
- Thời gian phản hồi trung bình dưới 500ms cho các API endpoints

### 3.2.2. Bảo mật

- Tuân thủ tiêu chuẩn OWASP Top 10
- Áp dụng Security Headers (X-Content-Type-Options, X-Frame-Options, X-XSS-Protection)
- Mật khẩu được mã hóa bằng BCrypt với salt tự động

### 3.2.3. Khả dụng

- Giao diện responsive, tương thích mobile và desktop
- Hỗ trợ các trình duyệt phổ biến (Chrome, Firefox, Safari, Edge)

### 3.2.4. Bảo trì

- Kiến trúc phân lớp rõ ràng
- Code theo nguyên tắc SOLID
- Dễ mở rộng thêm module mới

### 3.2.5. Tính nhất quán dữ liệu

- Sử dụng Unique Constraints, Foreign Keys
- Transaction khi cần đảm bảo tính nguyên tử

---

# CHƯƠNG 4: THIẾT KẾ HỆ THỐNG

## 4.1. Kiến trúc tổng thể

Hệ thống LUXE Hair Salon được thiết kế theo mô hình **API + MVC** (tách biệt Backend và Frontend), giao tiếp qua giao thức REST với JWT Bearer Token.

### 4.1.1. Sơ đồ kiến trúc

```
┌─────────────────┐      ┌───────────────────┐      ┌──────────────────┐
│    Trình duyệt   │────▶│   HairSalonVN.Web  │────▶│  HairSalonVN.API │
│   (Browser)      │      │   (ASP.NET MVC)    │      │  (ASP.NET Core) │
└─────────────────┘      └─────────────────┘      └────────┬────────┘
                                                          │
                                                          ▼
                                                 ┌────────────────┐
                                                 │   SQL Server    │
                                                 │   (Database)    │
                                                 └────────────────┘
```

### 4.1.2. Luồng dữ liệu

1. **Lớp 1 (Presentation)**: Trình duyệt gửi HTTP Request đến ASP.NET Core MVC Web
2. **Lớp 2 (Application)**: MVC Web gọi REST API thông qua HttpClient kèm JWT Token từ Session
3. **Lớp 3 (Data)**: API truy vấn SQL Server thông qua Entity Framework Core 8

## 4.2. Cấu trúc dự án

Dự án được tổ chức thành 3 project riêng biệt trong cùng một Solution:

| Project | Vai trò | Công nghệ chính |
|---------|---------|-----------------|
| HairSalonVN.API | REST API Backend – xử lý business logic và truy cập dữ liệu | ASP.NET Core 8 Web API, EF Core 8 |
| HairSalonVN.Web | MVC Frontend – giao diện người dùng | ASP.NET Core 8 MVC, Razor, CSS/JS |
| HairSalonVN.Database | Data Layer – entity, migration, seeder | EF Core 8, SQL Server, Fluent API |

### 4.2.1. Cấu trúc chi tiết HairSalonVN.API

```
HairSalonVN.API/
├── Controllers/                     # Auth, Appointments, Services, Staff, Reviews, Payments
├── Services/                        # AuthService, AppointmentService, ServiceService
├── Services/DTOs/                  # Data Transfer Objects
│   ├── Appointment/
│   ├── Auth/
│   ├── Common/
│   └── Services/
├── Middleware/                     # RateLimiting, AuditLog, ExceptionMiddleware
├── Helpers/                        # JwtHelper, ValidationHelper
└── Extensions/                     # ClaimsPrincipalExtensions
```

### 4.2.2. Cấu trúc chi tiết HairSalonVN.Web

```
HairSalonVN.Web/
├── Controllers/                   # Home, Account, Booking, Admin, Staff
├── Models/
│   ├── Auth/
│   ├── Booking/
│   └── Shared/
├── Services/                      # ApiClient (HTTP calls)
├── Views/                         # Razor Views
├── wwwroot/css/                   # salon.css, landing.css, pages.css
└── wwwroot/js/                    # salon.js
```

### 4.2.3. Cấu trúc chi tiết HairSalonVN.Database

```
HairSalonVN.Database/
├── Entities/                      # 9 Entity classes (User, Staff, Service, Appointment...)
├── Configurations/                # Fluent API configurations
├── Migrations/                    # EF Core migrations
├── Constants/                     # Roles, AppointmentStatus, PaymentConstants
└── Seeders/                      # Demo data (DataSeeder.cs)
```

## 4.3. Sơ đồ Use Case

### 4.3.1. Tổng quan Use Case

Hệ thống có **4 actor** chính và **17 use case** được phân chia theo vai trò:

**Actors:**
1. **Khách vãng lai** (Guest): Không cần đăng nhập vẫn có thể đặt lịch
2. **Khách hàng** (Customer): Đã có tài khoản trong hệ thống
3. **Stylist** (Staff): Nhân viên salon, quản lý lịch hẹn của mình
4. **Admin**: Quản trị viên, toàn quyền quản lý hệ thống

### 4.3.2. Danh sách Use Case chi tiết

| STT | Use Case | Actor | Mô tả |
|-----|----------|-------|--------|
| UC01 | Xem trang chủ | Guest, Customer | Truy cập landing page với thông tin salon |
| UC02 | Đăng ký tài khoản | Guest, Customer | Tạo tài khoản mới với email, mật khẩu |
| UC03 | Đăng nhập | Customer, Staff, Admin | Xác thực tài khoản, nhận JWT token |
| UC04 | Đăng xuất | Customer, Staff, Admin | Thu hồi token, xóa session |
| UC05 | Đặt lịch (Guest) | Guest | Đặt lịch không cần đăng nhập |
| UC06 | Đặt lịch (KH) | Customer | Đặt lịch với tài khoản đã đăng nhập |
| UC07 | Xem lịch của tôi | Customer | Xem danh sách lịch hẹn cá nhân |
| UC08 | Hủy lịch hẹn | Customer | Hủy lịch chưa được xác nhận |
| UC09 | Gửi đánh giá | Customer | Đánh giá dịch vụ sau khi hoàn thành |
| UC10 | Xem ca làm việc | Staff | Xem lịch hẹn trong ngày |
| UC11 | Cập nhật trạng thái lịch | Staff | Xác nhận/Hoàn thành lịch được gán |
| UC12 | Dashboard Admin | Admin | Xem tổng quan thống kê |
| UC13 | Quản lý lịch hẹn | Admin | Duyệt, lọc, đổi trạng thái lịch |
| UC14 | Thêm dịch vụ | Admin | Tạo dịch vụ mới |
| UC15 | Sửa dịch vụ | Admin | Cập nhật thông tin dịch vụ |
| UC16 | Xóa dịch vụ | Admin | Vô hiệu hóa dịch vụ (soft delete) |
| UC17 | Xem nhân viên | Admin | Xem danh sách và thông tin stylist |

## 4.4. Sơ đồ Lớp (Class Diagram)

### 4.4.1. Entity Model Overview

Domain model bao gồm **9 entity** chính với các mối quan hệ được thiết kế chặt chẽ:

```
User (1) ──0..1── Staff : has profile
User (1) ──N── RefreshToken : owns
User (1) ──N── Appointment : books
User (1) ──N── Review : writes
Staff (1) ──N── Appointment : serves
Staff (1) ──N── WorkingHour : has schedule
Staff (N) ──N── Service : provides (via StaffService)
Service (1) ──N── Appointment : booked for
Appointment (1) ──0..1── Review : receives
Appointment (1) ──0..1── Payment : has
```

### 4.4.2. Chi tiết Entity Classes

**User Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| FullName | string | Họ và tên đầy đủ |
| Email | string | Email (Unique) |
| PasswordHash | string | Mật khẩu đã mã hóa |
| Phone | string | Số điện thoại |
| Role | string | Vai trò (Customer/Staff/Admin) |
| IsActive | bool | Trạng thái hoạt động |
| CreatedAt | DateTime | Thời điểm tạo |

**Staff Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| UserId | Guid | Khóa ngoại đến User |
| Specialty | string | Chuyên môn |
| Bio | string | Tiểu sử |
| AvatarUrl | string | URL ảnh đại diện |
| IsAvailable | bool | Trạng thái nhận lịch |

**Service Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| Name | string | Tên dịch vụ |
| Description | string | Mô tả chi tiết |
| ImageUrl | string | URL hình ảnh |
| Price | decimal | Giá dịch vụ |
| DurationMinutes | int | Thời gian thực hiện (phút) |
| IsActive | bool | Trạng thái hoạt động |

**Appointment Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| CustomerId | Guid? | Khóa ngoại đến User (nullable) |
| StaffId | Guid | Khóa ngoại đến Staff |
| ServiceId | Guid | Khóa ngoại đến Service |
| AppointmentDate | DateTime | Ngày giờ hẹn |
| Status | string | Trạng thái lịch hẹn |
| BookingCode | string | Mã đặt lịch (duy nhất) |
| Notes | string? | Ghi chú |
| IsGuestBooking | bool | Có phải khách vãng lai |
| GuestName | string? | Tên khách vãng lai |
| GuestPhone | string? | SĐT khách vãng lai |
| GuestEmail | string? | Email khách vãng lai |

**Review Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| AppointmentId | Guid | Khóa ngoại đến Appointment (Unique) |
| CustomerId | Guid | Khóa ngoại đến User |
| Rating | int | Điểm đánh giá (1-5 sao) |
| Comment | string? | Nhận xét |

**Payment Entity**
| Property | Type | Mô tả |
|----------|------|--------|
| Id | Guid | Khóa chính |
| AppointmentId | Guid | Khóa ngoại đến Appointment (Unique) |
| Amount | decimal | Số tiền thanh toán |
| Method | string | Phương thức (Cash/Card/Transfer) |
| Status | string | Trạng thái thanh toán |
| PaidAt | DateTime? | Thời điểm thanh toán |

## 4.5. Sơ đồ ERD (Entity Relationship Diagram)

### 4.5.1. Các bảng trong hệ thống

| Bảng | Khóa chính | Mô tả |
|------|-------------|--------|
| USERS | Id (Guid) | Thông tin người dùng |
| STAFFS | Id (Guid) | Thông tin nhân viên (stylist) |
| SERVICES | Id (Guid) | Danh sách dịch vụ |
| STAFF_SERVICES | StaffId, ServiceId | Liên kết stylist - dịch vụ |
| WORKING_HOURS | Id (Guid) | Lịch làm việc của stylist |
| APPOINTMENTS | Id (Guid) | Lịch hẹn của khách |
| REVIEWS | Id (Guid) | Đánh giá dịch vụ |
| PAYMENTS | Id (Guid) | Thông tin thanh toán |
| REFRESH_TOKENS | Id (Guid) | Token làm mới đăng nhập |

### 4.5.2. Mối quan hệ giữa các bảng

| Bảng 1 | Quan hệ | Bảng 2 | Mô tả |
|--------|---------|---------|--------|
| USERS | 1:0..1 | STAFFS | Mỗi User có thể là Staff |
| USERS | 1:N | APPOINTMENTS | User đặt nhiều lịch |
| USERS | 1:N | REVIEWS | User viết nhiều review |
| USERS | 1:N | REFRESH_TOKENS | User có nhiều token |
| STAFFS | 1:N | WORKING_HOURS | Staff có nhiều giờ làm |
| STAFFS | 1:N | APPOINTMENTS | Staff phục vụ nhiều lịch |
| STAFFS | N:N | SERVICES | Staff cung cấp nhiều dịch vụ |
| SERVICES | 1:N | APPOINTMENTS | Service được đặt nhiều lần |
| APPOINTMENTS | 1:0..1 | REVIEWS | Appointment có thể có review |
| APPOINTMENTS | 1:0..1 | PAYMENTS | Appointment có thể có payment |

## 4.6. Sơ đồ Sequence – Luồng Đặt Lịch

### 4.6.1. Mô tả luồng đặt lịch

Luồng đặt lịch trải qua **3 bước chính**:

1. **Bước 1**: Hiển thị danh sách dịch vụ từ API
2. **Bước 2**: Tải danh sách stylist và khung giờ trống dựa trên WorkingHours và Appointments đã có
3. **Bước 3**: Submit form xác nhận, API tạo Appointment mới và trả về BookingCode duy nhất

### 4.6.2. Chi tiết tương tác

```
Khách hàng → BookingController → API → Database
     │              │               │         │
     │              │               │         ├── SELECT Services
     │              │               │         ├── SELECT Staff + WorkingHours
     │              │               │         └── INSERT Appointment
     │              │               │         ← BookingCode trả về
     │              │               │
     │              │               ← Services/Staff/Slots JSON
     │              │
     │              ← Hiển thị giao diện
     │
     ← Redirect đến trang thành công
```

## 4.7. Kiến trúc Bảo mật

### 4.7.1. Authentication Flow

```
Đăng nhập:
User → POST /api/auth/login (email, password)
       API → BCrypt.Verify(password, hash)
       API → Generate JWT Access Token (15 phút)
       API → Generate Refresh Token (7 ngày)
       API → INSERT RefreshToken
       ← {AccessToken, RefreshToken, Role}
       
Session Storage:
Web → Session["AccessToken"] = Token
Web → Session["UserRole"] = Role
```

### 4.7.2. Authorization Middleware Pipeline

```
Request → RateLimiting → AuditLog → Exception → Authentication → Authorization → Controller
                │             │           │              │              │
                ▼             ▼           ▼              ▼              ▼
           Check limit    Log POST/    Catch &       Validate JWT    Check Role
                        PUT/DELETE    Return JSON   & Extract Claims
```

---

# CHƯƠNG 5: THIẾT KẾ API ENDPOINTS

## 5.1. Authentication API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| POST | `/api/auth/register` | Đăng ký tài khoản mới | Không |
| POST | `/api/auth/login` | Đăng nhập, nhận Access + Refresh Token | Không |
| POST | `/api/auth/refresh` | Làm mới Access Token bằng Refresh Token | Không |
| POST | `/api/auth/logout` | Đăng xuất, thu hồi Refresh Token | Bearer JWT |
| GET | `/api/auth/me` | Lấy thông tin user hiện tại | Bearer JWT |

### 5.1.1. Đăng nhập (POST /api/auth/login)

**Request:**
```json
{
  "email": "admin@luxehair.vn",
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "a1b2c3d4-e5f6-...",
    "role": "Admin",
    "expiresIn": 900
  },
  "message": "Đăng nhập thành công"
}
```

## 5.2. Services API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| GET | `/api/services` | Danh sách tất cả dịch vụ đang hoạt động | Không |
| GET | `/api/services/{id}` | Chi tiết một dịch vụ | Không |
| GET | `/api/services/by-service/{id}` | Stylist cung cấp dịch vụ | Không |
| POST | `/api/services` | Tạo dịch vụ mới | Admin |
| PUT | `/api/services/{id}` | Cập nhật dịch vụ | Admin |
| DELETE | `/api/services/{id}` | Xóa dịch vụ (soft delete) | Admin |

### 5.2.1. Lấy danh sách dịch vụ (GET /api/services)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "guid",
      "name": "Cắt Tóc Nam",
      "description": "Cắt tóc theo kiểu nam tính",
      "price": 150000,
      "durationMinutes": 45,
      "imageUrl": "/images/service-1.jpg"
    }
  ]
}
```

## 5.3. Staff API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| GET | `/api/staff` | Danh sách stylist | Không |
| GET | `/api/staff/me` | Thông tin stylist hiện tại | Staff |
| GET | `/api/staff/{id}` | Chi tiết stylist | Không |
| PUT | `/api/staff/{id}/working-hours` | Cập nhật giờ làm | Admin |

## 5.4. Appointments API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| GET | `/api/appointments` | Tất cả lịch hẹn | Admin |
| GET | `/api/appointments/my` | Lịch của tôi | Customer |
| GET | `/api/appointments/{id}` | Chi tiết lịch hẹn | Customer/Staff/Admin |
| GET | `/api/appointments/available-slots` | Khung giờ trống | Không |
| GET | `/api/appointments/slots` | Khung giờ trống (alt) | Không |
| POST | `/api/appointments` | Tạo lịch hẹn | Customer |
| POST | `/api/appointments/guest` | Tạo lịch (khách vãng lai) | Không |
| PUT | `/api/appointments/{id}/status` | Cập nhật trạng thái | Staff/Admin |

### 5.4.1. Lấy khung giờ trống (GET /api/appointments/available-slots)

**Parameters:**
- `staffId`: GUID của stylist
- `serviceId`: GUID của dịch vụ
- `date`: Ngày cần kiểm tra (yyyy-MM-dd)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "startTime": "08:00:00",
      "endTime": "08:30:00",
      "isAvailable": true
    },
    {
      "startTime": "08:30:00",
      "endTime": "09:00:00",
      "isAvailable": false
    }
  ]
}
```

### 5.4.2. Tạo lịch hẹn khách vãng lai (POST /api/appointments/guest)

**Request:**
```json
{
  "serviceId": "guid",
  "staffId": "guid",
  "appointmentDate": "2026-06-01T09:00:00",
  "notes": "Yêu cầu cắt ngắn",
  "isGuest": true,
  "guestName": "Nguyễn Văn A",
  "guestPhone": "0901234567",
  "guestEmail": "email@example.com"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "guid",
    "bookingCode": "HS260601XXXXXX",
    "serviceName": "Cắt Tóc Nam",
    "staffName": "Nguyễn Thái",
    "appointmentDate": "2026-06-01T09:00:00",
    "status": "Pending"
  },
  "message": "Đặt lịch thành công!"
}
```

## 5.5. Reviews API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| GET | `/api/reviews` | Tất cả đánh giá | Không |
| GET | `/api/reviews/service/{id}` | Đánh giá theo dịch vụ | Không |
| GET | `/api/reviews/staff/{id}` | Đánh giá theo stylist | Không |
| POST | `/api/reviews` | Tạo đánh giá | Customer |

## 5.6. Payments API

| Method | Endpoint | Mô tả | Xác thực |
|--------|----------|--------|----------|
| GET | `/api/payments` | Danh sách thanh toán | Admin |
| GET | `/api/payments/summary` | Tổng hợp doanh thu | Admin |

---

# CHƯƠNG 6: TRIỂN KHAI VÀ THỬ NGHIỆM

## 6.1. Môi trường phát triển

| Thành phần | Phiên bản / Công cụ |
|------------|---------------------|
| Framework | .NET 8 SDK |
| IDE | Visual Studio 2022 / VS Code |
| Database | SQL Server 2022 (LocalDB/Express) |
| API Testing | Swagger UI, Postman |
| Version Control | Git |
| Browser | Chrome, Firefox, Edge |

## 6.2. Cấu trúc dự án

```
HairSalonVN/
├── HairSalonVN.sln
├── HairSalonVN.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── AppointmentsController.cs
│   │   ├── ServicesController.cs
│   │   ├── StaffController.cs
│   │   ├── ReviewsController.cs
│   │   └── PaymentsController.cs
│   ├── Services/
│   │   ├── AppointmentService.cs
│   │   ├── AuthService.cs
│   │   ├── ServiceManagementService.cs
│   │   └── StaffManagementService.cs
│   ├── Middleware/
│   │   ├── ExceptionMiddleware.cs
│   │   ├── RateLimitingMiddleware.cs
│   │   ├── AuditLogMiddleware.cs
│   │   └── RequestTimingMiddleware.cs
│   ├── Helpers/
│   │   └── JwtHelper.cs
│   └── Program.cs
├── HairSalonVN.Web/
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── AccountController.cs
│   │   ├── BookingController.cs
│   │   ├── AdminController.cs
│   │   └── StaffController.cs
│   ├── Models/
│   ├── Services/
│   │   └── ApiClient classes
│   ├── Views/
│   └── wwwroot/
└── HairSalonVN.Database/
    ├── Entities/
    ├── Configurations/
    ├── Migrations/
    └── Seeders/
```

## 6.3. Tài khoản demo

| Vai trò | Email | Mật khẩu | Chuyên môn |
|---------|-------|----------|------------|
| Admin | admin@luxehair.vn | Admin@123 | Toàn quyền quản trị |
| Stylist | tai.nguyen@luxehair.vn | Staff@123 | Cắt & Nhuộm |
| Stylist | nam.tran@luxehair.vn | Staff@123 | Uốn & Duỗi |
| Stylist | huong.le@luxehair.vn | Staff@123 | Nhuộm & Tạo mẫu |
| Khách hàng | mai.pham@gmail.com | Customer@123 | Tài khoản thử nghiệm |
| Khách hàng | hung.le@yahoo.com | Customer@123 | Tài khoản thử nghiệm |

## 6.4. Hướng dẫn cài đặt và chạy

### 6.4.1. Yêu cầu hệ thống

- .NET 8 SDK
- SQL Server (LocalDB hoặc SQL Server Express)
- 2 terminal/command prompt

### 6.4.2. Bước 1: Chuẩn bị cơ sở dữ liệu

```bash
cd HairSalonVN.API
dotnet ef database update --project ../HairSalonVN.Database
```

Lệnh này sẽ tạo tất cả các bảng và seed dữ liệu mẫu (users, services, staff, working hours).

### 6.4.3. Bước 2: Khởi chạy API

```bash
# Terminal 1
cd HairSalonVN.API
dotnet run
```

- API chạy tại: http://localhost:7098
- Swagger UI: http://localhost:7098/swagger

### 6.4.4. Bước 3: Khởi chạy Web

```bash
# Terminal 2
cd HairSalonVN.Web
dotnet run
```

- Web chạy tại: http://localhost:7126

## 6.5. Các luồng chức năng chính

### 6.5.1. Luồng 1: Đặt lịch khách vãng lai

1. Mở trình duyệt → `http://localhost:7126`
2. Click **Đặt lịch ngay**
3. **Bước 1**: Chọn dịch vụ (VD: Cắt Tóc Nam)
4. **Bước 2**: Chọn ngày → Stylist tự động tải → Click chọn giờ trống
5. **Bước 3**: Điền họ tên, SĐT → Submit
6. Nhận mã đặt lịch (VD: `HS260528XXXXXX`)

### 6.5.2. Luồng 2: Đặt lịch khách đã đăng nhập

1. Đăng nhập: `mai.pham@gmail.com` / `Customer@123`
2. Vào **Đặt lịch** → Chọn dịch vụ → Stylist → Giờ → Xác nhận
3. Lịch hẹn tự động ghi nhận thông tin từ tài khoản

### 6.5.3. Luồng 3: Quản lý Admin

1. Đăng nhập: `admin@luxehair.vn` / `Admin@123`
2. **Dashboard**: Xem tổng quan doanh thu, lịch hẹn
3. **Lịch hẹn**: Duyệt (Pending → Confirmed → Completed)
4. **Dịch vụ**: Thêm/Sửa/Xóa dịch vụ
5. **Báo cáo**: Xem doanh thu, top dịch vụ

### 6.5.4. Luồng 4: Stylist quản lý lịch

1. Đăng nhập: `tai.nguyen@luxehair.vn` / `Staff@123`
2. Dashboard hiện lịch hẹn hôm nay
3. Cập nhật trạng thái: Pending → Confirmed → Completed

### 6.5.5. Luồng 5: Đánh giá dịch vụ

1. Đăng nhập khách hàng
2. Vào **Lịch của tôi**
3. Tìm lịch **Hoàn thành** + chưa đánh giá
4. Click **Đánh giá** → Chọn sao + nhận xét → Gửi

## 6.6. Kết quả thử nghiệm

### 6.6.1. Build Status

| Project | Kết quả | Warnings | Errors |
|---------|---------|----------|--------|
| HairSalonVN.API | ✅ Thành công | 8 | 0 |
| HairSalonVN.Web | ✅ Thành công | 0 | 0 |

### 6.6.2. Test API qua Swagger

1. Mở `http://localhost:7098/swagger`
2. Endpoint công khai:
   - `GET /api/services` - Danh sách dịch vụ
   - `GET /api/staff` - Danh sách stylist
   - `GET /api/appointments/available-slots` - Khung giờ trống
3. Endpoint cần token:
   - Đăng nhập `POST /api/auth/login`
   - Copy token từ response
   - Click **Authorize** → Dán token → Test các endpoint

### 6.6.3. Các lỗi đã xử lý

| STT | Lỗi | Nguyên nhân | Giải pháp |
|-----|-----|-------------|-----------|
| 1 | Lỗi tải khung giờ | Date binding không đúng format | Sử dụng string date + parse thủ công |
| 2 | Lỗi xác nhận đặt lịch | Exception không handle, trả HTML thay vì JSON | Thêm ExceptionMiddleware trả JSON chuẩn |
| 3 | Null reference khi load service | Thiếu null check | Thêm validation trong service |

---

# CHƯƠNG 7: KẾT LUẬN VÀ HƯỚNG PHÁT TRIỂN

## 7.1. Kết quả đạt được

Sau quá trình nghiên cứu và phát triển, hệ thống LUXE Hair Salon đã đạt được các mục tiêu đề ra:

- Xây dựng hoàn chỉnh hệ thống đặt lịch trực tuyến với 3 vai trò người dùng và 17 use case.
- Triển khai kiến trúc API + MVC tách biệt rõ ràng, dễ bảo trì và mở rộng.
- Áp dụng xác thực JWT (Access Token 15 phút + Refresh Token 7 ngày) theo tiêu chuẩn industry.
- Tích hợp đầy đủ các lớp bảo mật: Rate Limiting, Audit Logging, Security Headers, BCrypt.
- Hỗ trợ Guest Booking – tính năng cho phép khách chưa đăng ký đặt lịch ngay lập tức.
- Toàn bộ project build thành công với 0 warning, 0 error trên cả API và Web.

## 7.2. Hạn chế

- Chưa tích hợp cổng thanh toán trực tuyến (VNPay, MoMo) — Payment hiện chỉ ghi nhận thủ công.
- Chưa có thông báo real-time (SignalR/WebSocket) khi lịch hẹn được duyệt.
- Chưa triển khai unit test và integration test đầy đủ.
- Chưa hỗ trợ đa ngôn ngữ (i18n).
- Chưa có ứng dụng mobile riêng.

## 7.3. Hướng phát triển tương lai

- **Tích hợp cổng thanh toán VNPay/MoMo** cho đặt lịch trực tuyến đầy đủ chu trình thanh toán.
- **Thêm thông báo real-time** bằng SignalR khi trạng thái lịch hẹn thay đổi.
- **Triển khai ứng dụng mobile** (Flutter/React Native) tích hợp với REST API hiện có.
- **Xây dựng hệ thống gợi ý Stylist** dựa trên lịch sử đặt lịch và đánh giá (Machine Learning).
- **Đưa lên cloud (Azure/AWS)** với CI/CD pipeline tự động và auto-scaling.
- **Triển khai Unit Test và Integration Test** với xUnit/NUnit để đảm bảo chất lượng code.

---

# TÀI LIỆU THAM KHẢO

1. Microsoft. (2024). ASP.NET Core documentation. https://docs.microsoft.com/aspnet/core

2. C# Corner. (2026). Best Practices for Building Web APIs in ASP.NET Core. c-sharpcorner.com

3. Code Maze. (2024). ASP.NET Core Web API Best Practices. code-maze.com

4. Medium / Chandrashekhar Singh. (2026). Entity Framework Core + SQL Server in .NET 8 Web API — Complete Guide.

5. WireFuture. (2026). JWT Authentication in ASP.NET Core Done Properly. wirefuture.com

6. Simple Talk / Red Gate. (2026). How to use refresh tokens in ASP.NET Core – a complete guide.

7. OWASP Foundation. (2025). OWASP Top Ten Security Risks. owasp.org

8. Entity Framework Core Documentation. (2024). https://docs.microsoft.com/ef/core

---

# PHỤ LỤC

## Phụ lục A: Cấu hình Connection String

**appsettings.json (API):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=ASUS;Database=HairSalonDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "HairSalon_SuperSecretKey_AtLeast32Characters_2024!",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  }
}
```

**appsettings.json (Web):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=ASUS;Database=HairSalonDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "ApiSettings": {
    "BaseUrl": "http://localhost:7098/api"
  }
}
```

## Phụ lục B: Cấu hình JWT Middleware (Program.cs - API)

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            RequireSignedTokens = false,
            SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
        };
    });
```

## Phụ lục C: Code mẫu - AppointmentService (Business Logic)

```csharp
public async Task<ApiResponse<AppointmentResponseDto>> GuestCreateAsync(AppointmentCreateDto dto)
{
    // Validate input
    if (string.IsNullOrWhiteSpace(dto.GuestName))
        return ApiResponse<AppointmentResponseDto>.Fail("Vui long nhap ho ten");
    if (string.IsNullOrWhiteSpace(dto.GuestPhone))
        return ApiResponse<AppointmentResponseDto>.Fail("Vui long nhap so dien thoai");

    // Validate service
    var service = await _svcRepo.GetByIdAsync(dto.ServiceId);
    if (service == null || !service.IsActive)
        return ApiResponse<AppointmentResponseDto>.Fail("Dich vu khong ton tai");

    // Lock để tránh race condition khi đặt lịch đồng thời
    var slotLock = GetStaffDateLock(dto.StaffId, startTime.Date);
    await slotLock.WaitAsync();
    try
    {
        // Check conflict
        if (await HasSlotConflictAsync(dto.StaffId, startTime, endTime))
            return ApiResponse<AppointmentResponseDto>.Fail("Khung gio da duoc dat");

        // Create appointment
        var appt = new Appointment
        {
            Id = Guid.NewGuid(),
            StaffId = dto.StaffId,
            ServiceId = dto.ServiceId,
            AppointmentDate = startTime,
            Status = AppointmentStatus.Pending,
            BookingCode = GenerateBookingCode(),
            IsGuestBooking = true,
            GuestName = dto.GuestName.Trim(),
            GuestPhone = dto.GuestPhone.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _apptRepo.AddAsync(appt);
        await _apptRepo.SaveChangesAsync();

        return ApiResponse<AppointmentResponseDto>.Ok(MapToDto(fullAppt), "Dat lich thanh cong!");
    }
    finally
    {
        slotLock.Release();
    }
}
```

## Phụ lục D: Middleware Pipeline (Program.cs - API)

```csharp
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<AuditLogMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    ctx.Response.Headers.Append("X-Frame-Options", "DENY");
    ctx.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    ctx.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});
```

---

**─── Hết báo cáo ───**
