// Script to generate DOCX report for LUXE Hair Salon
const { Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  Header, Footer, AlignmentType, HeadingLevel, BorderStyle, WidthType,
  ShadingType, VerticalAlign, PageNumber, PageBreak, LevelFormat,
  TableOfContents, ImageRun } = require('docx');
const fs = require('fs');
const path = require('path');

// Color palette
const BLUE = "1F4E79";
const LBLUE = "2E75B6";
const ACCENT = "D6E4F0";
const GRAY = "595959";
const LGRAY = "F2F2F2";
const WHITE = "FFFFFF";
const GREEN = "375623";
const LGREEN = "E2EFDA";

// Helper: cell border
const cellBorder = (color = "CCCCCC") => {
  const b = { style: BorderStyle.SINGLE, size: 1, color };
  return { top: b, bottom: b, left: b, right: b };
};

// Helper: styled paragraph
function para(text, opts = {}) {
  return new Paragraph({
    alignment: opts.center ? AlignmentType.CENTER : AlignmentType.LEFT,
    spacing: { before: opts.before ?? 80, after: opts.after ?? 80 },
    children: [new TextRun({
      text,
      bold: opts.bold ?? false,
      italic: opts.italic ?? false,
      size: opts.size ?? 22,
      color: opts.color ?? "000000",
      font: "Times New Roman",
    })]
  });
}

function heading1(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_1,
    spacing: { before: 360, after: 200 },
    children: [new TextRun({ text, bold: true, size: 32, color: BLUE, font: "Arial" })]
  });
}

function heading2(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_2,
    spacing: { before: 280, after: 140 },
    children: [new TextRun({ text, bold: true, size: 26, color: LBLUE, font: "Arial" })]
  });
}

function heading3(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_3,
    spacing: { before: 200, after: 100 },
    children: [new TextRun({ text, bold: true, size: 24, color: "1A1A1A", font: "Arial" })]
  });
}

function bullet(text) {
  return new Paragraph({
    numbering: { reference: "bullets", level: 0 },
    spacing: { before: 60, after: 60 },
    children: [new TextRun({ text, size: 22, font: "Times New Roman" })]
  });
}

function bodyText(text) {
  return para(text, { before: 100, after: 100 });
}

// TABLE BUILDER
function buildTable(headers, rows, colWidths) {
  const totalWidth = colWidths.reduce((a, b) => a + b, 0);
  const headerBorder = cellBorder("2E75B6");
  const rowBorder = cellBorder("CCCCCC");

  const makeCell = (text, width, isHeader) => new TableCell({
    width: { size: width, type: WidthType.DXA },
    borders: isHeader ? headerBorder : rowBorder,
    shading: { fill: isHeader ? LBLUE : WHITE, type: ShadingType.CLEAR },
    margins: { top: 80, bottom: 80, left: 120, right: 120 },
    verticalAlign: VerticalAlign.CENTER,
    children: [new Paragraph({
      alignment: AlignmentType.LEFT,
      children: [new TextRun({ text: text || "", bold: isHeader, size: isHeader ? 20 : 20,
        color: isHeader ? WHITE : "000000", font: "Arial" })]
    })]
  });

  return new Table({
    width: { size: totalWidth, type: WidthType.DXA },
    columnWidths: colWidths,
    rows: [
      new TableRow({ children: headers.map((h, i) => makeCell(h, colWidths[i], true)) }),
      ...rows.map((row, ri) => new TableRow({
        children: row.map((cell, i) => makeCell(cell, colWidths[i], false))
      }))
    ]
  });
}

// CODE BLOCK
function codeBlock(text) {
  return new Paragraph({
    spacing: { before: 100, after: 100 },
    shading: { fill: 'F0F0F0', type: ShadingType.CLEAR },
    children: [new TextRun({ text, font: 'Courier New', size: 18, color: '1F4E79' })]
  });
}

// BUILD DOCUMENT
async function buildDoc() {
  const doc = new Document({
    numbering: {
      config: [
        {
          reference: 'bullets',
          levels: [{ level: 0, format: LevelFormat.BULLET, text: '•',
            alignment: AlignmentType.LEFT,
            style: { paragraph: { indent: { left: 720, hanging: 360 } } } }]
        },
        {
          reference: 'numbered',
          levels: [{ level: 0, format: LevelFormat.DECIMAL, text: '%1.',
            alignment: AlignmentType.LEFT,
            style: { paragraph: { indent: { left: 720, hanging: 360 } } } }]
        },
      ]
    },
    styles: {
      default: {
        document: { run: { font: 'Times New Roman', size: 22 } }
      },
      paragraphStyles: [
        { id: 'Heading1', name: 'Heading 1', basedOn: 'Normal', next: 'Normal', quickFormat: true,
          run: { size: 32, bold: true, font: 'Arial', color: '1F4E79' },
          paragraph: { spacing: { before: 360, after: 200 }, outlineLevel: 0 } },
        { id: 'Heading2', name: 'Heading 2', basedOn: 'Normal', next: 'Normal', quickFormat: true,
          run: { size: 26, bold: true, font: 'Arial', color: '2E75B6' },
          paragraph: { spacing: { before: 280, after: 140 }, outlineLevel: 1 } },
        { id: 'Heading3', name: 'Heading 3', basedOn: 'Normal', next: 'Normal', quickFormat: true,
          run: { size: 24, bold: true, font: 'Arial', color: '1A1A1A' },
          paragraph: { spacing: { before: 200, after: 100 }, outlineLevel: 2 } },
      ]
    },
    sections: [{
      properties: {
        page: {
          size: { width: 11906, height: 16838 },
          margin: { top: 1440, right: 1134, bottom: 1440, left: 1701 }
        }
      },
      headers: {
        default: new Header({
          children: [new Paragraph({
            alignment: AlignmentType.RIGHT,
            border: { bottom: { style: BorderStyle.SINGLE, size: 6, color: '2E75B6', space: 4 } },
            spacing: { after: 120 },
            children: [new TextRun({ text: 'Báo cáo Đồ án – LUXE Hair Salon | Hệ thống Đặt Lịch Trực Tuyến',
              font: 'Arial', size: 18, color: '2E75B6', italic: true })]
          })]
        })
      },
      footers: {
        default: new Footer({
          children: [new Paragraph({
            alignment: AlignmentType.CENTER,
            border: { top: { style: BorderStyle.SINGLE, size: 4, color: 'CCCCCC', space: 4 } },
            children: [
              new TextRun({ text: 'Trang ', font: 'Arial', size: 18, color: '595959' }),
              new TextRun({ children: [PageNumber.CURRENT], font: 'Arial', size: 18, color: '595959' }),
              new TextRun({ text: ' / ', font: 'Arial', size: 18, color: '595959' }),
              new TextRun({ children: [PageNumber.TOTAL_PAGES], font: 'Arial', size: 18, color: '595959' }),
            ]
          })]
        })
      },
      children: [

        // ==================== TRANG BÌA ====================
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 1800, after: 400 },
          children: [new TextRun({ text: 'TRƯỜNG ĐẠI HỌC', bold: true, size: 24, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 1200 },
          children: [new TextRun({ text: 'Khoa Công nghệ Thông tin', bold: false, size: 24, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 400 },
          children: [new TextRun({ text: 'BÁO CÁO ĐỒ ÁN MÔN HỌC', bold: true, size: 40, font: 'Arial', color: '1F4E79' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 200 },
          children: [new TextRun({ text: 'Lập trình ADO.NET / ASP.NET Core', bold: false, size: 26, font: 'Arial', color: '595959', italics: true })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 600, after: 200 },
          children: [new TextRun({ text: 'LUXE HAIR SALON', bold: true, size: 52, font: 'Arial', color: '1F4E79' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 800 },
          children: [new TextRun({ text: 'Hệ thống Đặt Lịch Làm Tóc Trực Tuyến', bold: true, size: 30, font: 'Arial', color: '2E75B6' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 400, after: 120 },
          children: [new TextRun({ text: 'Công nghệ sử dụng:', bold: true, size: 22, font: 'Arial', color: '333333' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 600 },
          children: [new TextRun({ text: 'ASP.NET Core 8.0 • SQL Server • Entity Framework Core 8 • JWT Authentication', size: 22, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 600, after: 100 },
          children: [new TextRun({ text: 'Năm học: 2025–2026', size: 22, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({ children: [new PageBreak()] }),

        // ==================== MỤC LỤC ====================
        new TableOfContents('MỤC LỤC', {
          hyperlink: true,
          headingStyleRange: '1-3',
        }),
        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 1 ====================
        heading1('CHƯƠNG 1: GIỚI THIỆU ĐỀ TÀI'),

        heading2('1.1. Đặt vấn đề'),
        bodyText('Trong bối cảnh kinh tế dịch vụ phát triển mạnh mẽ tại Việt Nam, ngành chăm sóc tóc và làm đẹp đang chứng kiến sự tăng trưởng vượt bậc. Tuy nhiên, phần lớn các salon tóc vẫn đang vận hành theo phương thức truyền thống: khách hàng phải trực tiếp đến cửa hàng hoặc gọi điện để đặt lịch, dẫn đến nhiều bất tiện như xếp hàng chờ đợi, khó quản lý lịch hẹn và thiếu thông tin minh bạch về dịch vụ.'),
        bodyText('Trước thực trạng đó, nhóm thực hiện đề tài "LUXE Hair Salon – Hệ thống Đặt Lịch Làm Tóc Trực Tuyến" nhằm xây dựng một nền tảng số hóa toàn bộ quy trình đặt lịch, quản lý nhân viên và theo dõi doanh thu cho một salon tóc hiện đại.'),

        heading2('1.2. Mục tiêu đề tài'),
        heading3('1.2.1. Mục tiêu tổng quát'),
        bodyText('Xây dựng hệ thống đặt lịch làm tóc trực tuyến đa nền tảng, phục vụ ba nhóm người dùng chính: Khách hàng, Stylist và Quản trị viên, với giao diện thân thiện và quy trình nghiệp vụ chặt chẽ.'),
        heading3('1.2.2. Mục tiêu cụ thể'),
        bullet('Xây dựng hệ thống đặt lịch trực tuyến đa vai trò: Khách hàng, Stylist và Quản trị viên.'),
        bullet('Áp dụng kiến trúc API + MVC tách biệt rõ ràng giữa backend và frontend.'),
        bullet('Triển khai xác thực bảo mật với JWT Bearer Token và Refresh Token.'),
        bullet('Tích hợp các biện pháp bảo mật hiện đại: Rate Limiting, Audit Logging, Security Headers.'),
        bullet('Đảm bảo trải nghiệm người dùng mượt mà trên cả desktop và thiết bị di động (Responsive).'),

        heading2('1.3. Phạm vi và giới hạn'),
        heading3('1.3.1. Phạm vi nghiên cứu'),
        bodyText('Hệ thống tập trung vào nghiệp vụ cốt lõi của một salon tóc bao gồm: Quản lý dịch vụ, Đặt lịch hẹn (cả khách đã đăng ký và khách vãng lai), Quản lý lịch làm việc của stylist, Đánh giá dịch vụ và Báo cáo doanh thu.'),
        heading3('1.3.2. Giới hạn đề tài'),
        bullet('Chưa tích hợp thanh toán trực tuyến (VNPay, MoMo)'),
        bullet('Chưa có thông báo real-time (SignalR/WebSocket)'),
        bullet('Chưa triển khai ứng dụng mobile riêng'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 2 ====================
        heading1('CHƯƠNG 2: CÔNG NGHỆ SỬ DỤNG'),

        heading2('2.1. Tổng quan công nghệ'),
        buildTable(
          ['Thành phần', 'Công nghệ'],
          [
            ['Backend API', 'ASP.NET Core 8.0 Web API'],
            ['Frontend', 'ASP.NET Core 8.0 MVC (Razor)'],
            ['Cơ sở dữ liệu', 'SQL Server + Entity Framework Core 8'],
            ['Xác thực', 'JWT Bearer + Refresh Token'],
            ['Giao diện', 'CSS tùy chỉnh, Responsive'],
            ['Bảo mật', 'Rate Limiting, Audit Logging, Security Headers'],
          ],
          [3500, 5860]
        ),

        heading2('2.2. ASP.NET Core 8.0 Web API'),
        bodyText('ASP.NET Core 8.0 là nền tảng phát triển ứng dụng web đa nền tảng, mã nguồn mở của Microsoft. Dự án LUXE Hair Salon áp dụng kiến trúc phân lớp rõ ràng: Controllers (xử lý HTTP), Services (business logic), DTOs (tách biệt dữ liệu), và Middleware (xử lý xuyên suốt).'),

        heading2('2.3. Entity Framework Core 8 & SQL Server'),
        bodyText('Entity Framework Core (EF Core) là ORM chính thức của Microsoft cho .NET, cho phép ánh xạ các lớp C# thành bảng trong SQL Server. Dự án sử dụng Code-First approach với EF Core Migrations và Data Seeding.'),

        heading2('2.4. Xác thực JWT Bearer + Refresh Token'),
        bodyText('JSON Web Token (JWT) là tiêu chuẩn mở (RFC 7519) để truyền thông tin xác thực. Hệ thống sử dụng: Access Token (15 phút) cho truy cập tài nguyên và Refresh Token (7 ngày) để làm mới không cần đăng nhập lại.'),

        heading2('2.5. Bảo mật ứng dụng web'),
        buildTable(
          ['Biện pháp bảo mật', 'Mô tả', 'Cơ chế thực hiện'],
          [
            ['Rate Limiting', 'Giới hạn 100 requests/phút/client', 'RateLimitingMiddleware'],
            ['Audit Logging', 'Ghi log POST/PUT/DELETE', 'AuditLogMiddleware'],
            ['Security Headers', 'Ngăn chặn XSS, Clickjacking', 'X-Content-Type, X-Frame-Options'],
            ['Password Hashing', 'Mã hóa mật khẩu một chiều', 'BCrypt với salt tự động'],
            ['SQL Injection Prevention', 'Truy vấn an toàn', 'EF Core Parameterized Queries'],
          ],
          [2500, 3000, 2860]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 3 ====================
        heading1('CHƯƠNG 3: PHÂN TÍCH YÊU CẦU HỆ THỐNG'),

        heading2('3.1. Yêu cầu chức năng'),
        heading3('3.1.1. Nhóm chức năng Khách hàng (Customer)'),
        buildTable(
          ['STT', 'Chức năng', 'Mô tả chi tiết'],
          [
            ['1', 'Xem trang chủ', 'Landing page với hero banner, danh sách dịch vụ nổi bật'],
            ['2', 'Đăng ký tài khoản', 'Tạo tài khoản mới với validation email, SĐT, mật khẩu'],
            ['3', 'Đăng nhập / Đăng xuất', 'Xác thực JWT, lưu session'],
            ['4', 'Đặt lịch 3 bước', 'Chọn dịch vụ → Stylist & Giờ → Xác nhận'],
            ['5', 'Đặt lịch khách vãng lai', 'Đặt lịch không cần đăng nhập'],
            ['6', 'Xem lịch của tôi', 'Danh sách lịch hẹn cá nhân, lọc theo trạng thái'],
            ['7', 'Hủy lịch hẹn', 'Hủy lịch chưa được xác nhận'],
            ['8', 'Gửi đánh giá', 'Đánh giá 1-5 sao sau khi hoàn thành'],
          ],
          [500, 2500, 6360]
        ),

        heading3('3.1.2. Nhóm chức năng Stylist (Staff)'),
        buildTable(
          ['STT', 'Chức năng', 'Mô tả chi tiết'],
          [
            ['1', 'Dashboard cá nhân', 'Hiển thị lịch hẹn trong ngày, thống kê'],
            ['2', 'Cập nhật trạng thái lịch', 'Xác nhận/Hoàn thành lịch được gán'],
          ],
          [500, 2500, 6360]
        ),

        heading3('3.1.3. Nhóm chức năng Quản trị viên (Admin)'),
        buildTable(
          ['STT', 'Chức năng', 'Mô tả chi tiết'],
          [
            ['1', 'Dashboard tổng quan', 'Thống kê lịch hẹn, doanh thu tháng'],
            ['2', 'Quản lý lịch hẹn', 'Xem, lọc, duyệt và đổi trạng thái'],
            ['3', 'Quản lý dịch vụ (CRUD)', 'Thêm, sửa, xóa dịch vụ'],
            ['4', 'Quản lý nhân viên', 'Xem danh sách stylist'],
            ['5', 'Báo cáo & Thống kê', 'Doanh thu, top dịch vụ, nhân viên'],
          ],
          [500, 2500, 6360]
        ),

        heading2('3.2. Yêu cầu phi chức năng'),
        bullet('Hiệu suất: Hệ thống phục vụ tối thiểu 100 requests/phút/client'),
        bullet('Bảo mật: Tuân thủ tiêu chuẩn OWASP Top 10'),
        bullet('Khả dụng: Giao diện responsive, tương thích mobile và desktop'),
        bullet('Bảo trì: Kiến trúc phân lớp rõ ràng, code theo nguyên tắc SOLID'),
        bullet('Tính nhất quán: Sử dụng Unique Constraints, Foreign Keys và transaction'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 4 ====================
        heading1('CHƯƠNG 4: THIẾT KẾ HỆ THỐNG'),

        heading2('4.1. Kiến trúc tổng thể'),
        bodyText('Hệ thống LUXE Hair Salon được thiết kế theo mô hình API + MVC (tách biệt Backend và Frontend), giao tiếp qua giao thức REST với JWT Bearer Token. Luồng dữ liệu đi qua ba lớp chính:'),
        bullet('Lớp 1 (Presentation): Trình duyệt gửi HTTP Request đến ASP.NET Core MVC Web'),
        bullet('Lớp 2 (Application): MVC Web gọi REST API thông qua HttpClient kèm JWT Token'),
        bullet('Lớp 3 (Data): API truy vấn SQL Server thông qua Entity Framework Core 8'),

        heading2('4.2. Cấu trúc dự án'),
        buildTable(
          ['Project', 'Vai trò', 'Công nghệ chính'],
          [
            ['HairSalonVN.API', 'REST API Backend – xử lý business logic', 'ASP.NET Core 8 Web API, EF Core 8'],
            ['HairSalonVN.Web', 'MVC Frontend – giao diện người dùng', 'ASP.NET Core 8 MVC, Razor'],
            ['HairSalonVN.Database', 'Data Layer – entity, migration, seeder', 'EF Core 8, SQL Server, Fluent API'],
          ],
          [2500, 4000, 2860]
        ),

        heading2('4.3. Sơ đồ Use Case'),
        bodyText('Hệ thống có 4 actor chính và 17 use case được phân chia theo vai trò. Khách vãng lai (Guest) không cần đăng nhập vẫn có thể đặt lịch – đây là tính năng quan trọng giúp tăng tỷ lệ chuyển đổi.'),
        buildTable(
          ['STT', 'Use Case', 'Actor', 'Mô tả'],
          [
            ['UC01', 'Xem trang chủ', 'Guest, Customer', 'Landing page với thông tin salon'],
            ['UC02', 'Đăng ký tài khoản', 'Guest, Customer', 'Tạo tài khoản mới'],
            ['UC03', 'Đăng nhập', 'Customer, Staff, Admin', 'Xác thực tài khoản, nhận JWT'],
            ['UC05', 'Đặt lịch (Guest)', 'Guest', 'Đặt lịch không cần đăng nhập'],
            ['UC06', 'Đặt lịch (KH)', 'Customer', 'Đặt lịch với tài khoản'],
            ['UC07', 'Xem lịch của tôi', 'Customer', 'Danh sách lịch hẹn cá nhân'],
            ['UC09', 'Gửi đánh giá', 'Customer', 'Đánh giá dịch vụ'],
            ['UC10', 'Xem ca làm việc', 'Staff', 'Xem lịch hẹn trong ngày'],
            ['UC12', 'Dashboard Admin', 'Admin', 'Xem tổng quan thống kê'],
            ['UC13', 'Quản lý lịch hẹn', 'Admin', 'Duyệt, lọc, đổi trạng thái'],
          ],
          [600, 2000, 2000, 4760]
        ),

        heading2('4.4. Sơ đồ Lớp (Class Diagram)'),
        bodyText('Domain model bao gồm 9 entity chính. Entity Appointment là trung tâm của hệ thống, liên kết với User (CustomerId), Staff (StaffId), Service (ServiceId), Review và Payment. Thiết kế hỗ trợ cả Guest Booking thông qua các trường nullable.'),
        buildTable(
          ['Entity', 'Thuộc tính chính', 'Mô tả'],
          [
            ['User', 'Id, FullName, Email, PasswordHash, Role', 'Người dùng hệ thống'],
            ['Staff', 'Id, UserId, Specialty, Bio, IsAvailable', 'Nhân viên (stylist)'],
            ['Service', 'Id, Name, Price, DurationMinutes, IsActive', 'Dịch vụ salon'],
            ['Appointment', 'Id, StaffId, ServiceId, AppointmentDate, Status', 'Lịch hẹn'],
            ['Review', 'Id, AppointmentId, CustomerId, Rating, Comment', 'Đánh giá'],
            ['Payment', 'Id, AppointmentId, Amount, Method, Status', 'Thanh toán'],
            ['WorkingHour', 'Id, StaffId, DayOfWeek, StartTime, EndTime', 'Giờ làm việc'],
          ],
          [1500, 3500, 4360]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        heading2('4.5. Sơ đồ ERD'),
        bodyText('Hệ thống có 9 bảng dữ liệu với các mối quan hệ được thiết kế chặt chẽ:'),
        buildTable(
          ['Bảng 1', 'Quan hệ', 'Bảng 2', 'Mô tả'],
          [
            ['USERS', '1 : 0..1', 'STAFFS', 'Mỗi User có thể là Staff'],
            ['USERS', '1 : N', 'APPOINTMENTS', 'User đặt nhiều lịch'],
            ['STAFFS', '1 : N', 'WORKING_HOURS', 'Staff có nhiều giờ làm'],
            ['STAFFS', 'N : N', 'SERVICES', 'Staff cung cấp nhiều dịch vụ'],
            ['APPOINTMENTS', '1 : 0..1', 'REVIEWS', 'Appointment có thể có review'],
          ],
          [1600, 1200, 1800, 4760]
        ),

        heading2('4.6. Kiến trúc Bảo mật'),
        bodyText('Authentication Flow: User gửi POST /api/auth/login với email và password. API xác minh bằng BCrypt, tạo JWT Access Token (15 phút) và Refresh Token (7 ngày). Refresh Token được lưu trong bảng RefreshTokens để hỗ trợ thu hồi.'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 5 ====================
        heading1('CHƯƠNG 5: THIẾT KẾ API ENDPOINTS'),

        heading2('5.1. Authentication API'),
        buildTable(
          ['Method', 'Endpoint', 'Mô tả', 'Xác thực'],
          [
            ['POST', '/api/auth/register', 'Đăng ký tài khoản mới', 'Không'],
            ['POST', '/api/auth/login', 'Đăng nhập, nhận Access + Refresh Token', 'Không'],
            ['POST', '/api/auth/refresh', 'Làm mới Access Token', 'Không'],
            ['POST', '/api/auth/logout', 'Đăng xuất, thu hồi Refresh Token', 'Bearer JWT'],
            ['GET', '/api/auth/me', 'Lấy thông tin user hiện tại', 'Bearer JWT'],
          ],
          [900, 2200, 3500, 1760]
        ),

        heading2('5.2. Services API'),
        buildTable(
          ['Method', 'Endpoint', 'Mô tả', 'Xác thực'],
          [
            ['GET', '/api/services', 'Danh sách tất cả dịch vụ', 'Không'],
            ['GET', '/api/services/{id}', 'Chi tiết một dịch vụ', 'Không'],
            ['POST', '/api/services', 'Tạo dịch vụ mới', 'Admin'],
            ['PUT', '/api/services/{id}', 'Cập nhật dịch vụ', 'Admin'],
            ['DELETE', '/api/services/{id}', 'Xóa dịch vụ', 'Admin'],
          ],
          [900, 2200, 3500, 1760]
        ),

        heading2('5.3. Appointments API'),
        buildTable(
          ['Method', 'Endpoint', 'Mô tả', 'Xác thực'],
          [
            ['GET', '/api/appointments/available-slots', 'Khung giờ trống', 'Không'],
            ['POST', '/api/appointments/guest', 'Tạo lịch khách vãng lai', 'Không'],
            ['GET', '/api/appointments/my', 'Lịch của tôi', 'Customer'],
            ['POST', '/api/appointments', 'Tạo lịch hẹn', 'Customer'],
            ['PUT', '/api/appointments/{id}/status', 'Cập nhật trạng thái', 'Staff/Admin'],
            ['GET', '/api/appointments', 'Tất cả lịch hẹn', 'Admin'],
          ],
          [900, 2800, 3200, 1460]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 6 ====================
        heading1('CHƯƠNG 6: TRIỂN KHAI VÀ THỬ NGHIỆM'),

        heading2('6.1. Môi trường phát triển'),
        buildTable(
          ['Thành phần', 'Phiên bản / Công cụ'],
          [
            ['Framework', '.NET 8 SDK'],
            ['IDE', 'Visual Studio 2022 / VS Code'],
            ['Database', 'SQL Server 2022 (LocalDB/Express)'],
            ['API Testing', 'Swagger UI, Postman'],
            ['Version Control', 'Git'],
          ],
          [3500, 5860]
        ),

        heading2('6.2. Hướng dẫn cài đặt và chạy'),
        heading3('Bước 1: Chuẩn bị cơ sở dữ liệu'),
        codeBlock('cd HairSalonVN.API && dotnet ef database update --project ../HairSalonVN.Database'),

        heading3('Bước 2: Khởi chạy API'),
        codeBlock('# Terminal 1: cd HairSalonVN.API && dotnet run → http://localhost:7098'),

        heading3('Bước 3: Khởi chạy Web'),
        codeBlock('# Terminal 2: cd HairSalonVN.Web && dotnet run → http://localhost:7126'),

        heading2('6.3. Tài khoản demo'),
        buildTable(
          ['Vai trò', 'Email', 'Mật khẩu', 'Chuyên môn'],
          [
            ['Admin', 'admin@luxehair.vn', 'Admin@123', 'Toàn quyền quản trị'],
            ['Stylist', 'tai.nguyen@luxehair.vn', 'Staff@123', 'Cắt & Nhuộm'],
            ['Stylist', 'nam.tran@luxehair.vn', 'Staff@123', 'Uốn & Duỗi'],
            ['Khách hàng', 'mai.pham@gmail.com', 'Customer@123', 'Tài khoản thử nghiệm'],
          ],
          [1600, 2800, 1600, 3360]
        ),

        heading2('6.4. Các luồng nghiệp vụ chính'),
        heading3('Luồng 1: Đặt lịch khách vãng lai'),
        bullet('Truy cập http://localhost:7126 → Click "Đặt lịch ngay"'),
        bullet('Bước 1: Chọn dịch vụ (VD: Cắt Tóc Nam – 150.000đ)'),
        bullet('Bước 2: Chọn ngày, hệ thống tự động tải Stylist, chọn khung giờ trống'),
        bullet('Bước 3: Điền Họ tên và SĐT → Submit → Nhận mã đặt lịch'),

        heading3('Luồng 2: Admin quản lý và duyệt lịch hẹn'),
        bullet('Đăng nhập admin@luxehair.vn / Admin@123'),
        bullet('Dashboard: Xem tổng quan số lịch hẹn Pending, doanh thu tháng'),
        bullet('Lịch hẹn: Duyệt chuyển trạng thái Pending → Confirmed'),

        heading2('6.5. Kết quả thử nghiệm'),
        buildTable(
          ['Project', 'Kết quả', 'Warnings', 'Errors'],
          [
            ['HairSalonVN.API', 'Thành công', '8', '0'],
            ['HairSalonVN.Web', 'Thành công', '0', '0'],
          ],
          [3500, 2000, 1500, 1360]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 7 ====================
        heading1('CHƯƠNG 7: KẾT LUẬN VÀ HƯỚNG PHÁT TRIỂN'),

        heading2('7.1. Kết quả đạt được'),
        bullet('Xây dựng hoàn chỉnh hệ thống đặt lịch trực tuyến với 3 vai trò người dùng và 17 use case'),
        bullet('Triển khai kiến trúc API + MVC tách biệt rõ ràng, dễ bảo trì và mở rộng'),
        bullet('Áp dụng xác thực JWT (Access Token 15 phút + Refresh Token 7 ngày)'),
        bullet('Tích hợp đầy đủ các lớp bảo mật: Rate Limiting, Audit Logging, Security Headers, BCrypt'),
        bullet('Hỗ trợ Guest Booking – cho phép khách chưa đăng ký đặt lịch ngay lập tức'),
        bullet('Toàn bộ project build thành công với 0 Error trên cả API và Web'),

        heading2('7.2. Hạn chế'),
        bullet('Chưa tích hợp cổng thanh toán trực tuyến (VNPay, MoMo)'),
        bullet('Chưa có thông báo real-time (SignalR/WebSocket) khi lịch hẹn được duyệt'),
        bullet('Chưa triển khai unit test và integration test đầy đủ'),
        bullet('Chưa hỗ trợ đa ngôn ngữ (i18n)'),

        heading2('7.3. Hướng phát triển tương lai'),
        bullet('Tích hợp cổng thanh toán VNPay/MoMo cho đặt lịch trực tuyến'),
        bullet('Thêm thông báo real-time bằng SignalR khi trạng thái lịch hẹn thay đổi'),
        bullet('Triển khai ứng dụng mobile (Flutter/React Native) tích hợp với REST API'),
        bullet('Xây dựng hệ thống gợi ý Stylist dựa trên lịch sử đặt lịch (Machine Learning)'),
        bullet('Đưa lên cloud (Azure/AWS) với CI/CD pipeline tự động'),

        heading2('7.4. Tài liệu tham khảo'),
        bodyText('1. Microsoft. (2024). ASP.NET Core documentation. https://docs.microsoft.com/aspnet/core'),
        bodyText('2. C# Corner. (2026). Best Practices for Building Web APIs in ASP.NET Core. c-sharpcorner.com'),
        bodyText('3. Code Maze. (2024). ASP.NET Core Web API Best Practices. code-maze.com'),
        bodyText('4. Entity Framework Core Documentation. (2024). https://docs.microsoft.com/ef/core'),
        bodyText('5. OWASP Foundation. (2025). OWASP Top Ten Security Risks. owasp.org'),

        new Paragraph({ spacing: { before: 600, after: 200 }, alignment: AlignmentType.CENTER, children: [
          new TextRun({ text: '─── Hết ───', size: 22, font: 'Arial', color: '595959', italics: true })
        ]}),
      ]
    }]
  });

  return doc;
}

// RUN
buildDoc().then(async (doc) => {
  const buffer = await Packer.toBuffer(doc);
  const outputPath = path.join(__dirname, 'BaoCao_LUXE_HairSalon.docx');
  fs.writeFileSync(outputPath, buffer);
  console.log('✅ Created:', outputPath);
}).catch(err => {
  console.error('❌ Error:', err);
  process.exit(1);
});
