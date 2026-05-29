// DOCX Report Generator with Images for LUXE Hair Salon
const { Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  Header, Footer, AlignmentType, HeadingLevel, BorderStyle, WidthType,
  ShadingType, VerticalAlign, PageNumber, PageBreak, LevelFormat,
  TableOfContents, ImageRun } = require('docx');
const fs = require('fs');
const path = require('path');

// Load images
const DIAGRAM_DIR = "d:/HairSalonVN/diagrams";

function loadImage(filename) {
  const filepath = path.join(DIAGRAM_DIR, filename);
  if (fs.existsSync(filepath)) {
    return fs.readFileSync(filepath);
  }
  console.log(`Warning: Image not found: ${filepath}`);
  return null;
}

const images = {
  architecture: loadImage("1_architecture_diagram.png"),
  usecase: loadImage("2_usecase_diagram.png"),
  erd: loadImage("3_erd_diagram.png"),
  sequence: loadImage("4_sequence_diagram.png"),
  class: loadImage("5_class_diagram.png"),
  ui_flow: loadImage("6_ui_flow.png"),
  database: loadImage("7_database_schema.png"),
};

// Color palette
const BLUE = "1F4E79";
const LBLUE = "2E75B6";
const WHITE = "FFFFFF";
const GRAY = "595959";

// Helper functions
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

function buildTable(headers, rows, colWidths) {
  const totalWidth = colWidths.reduce((a, b) => a + b, 0);
  const makeCell = (text, width, isHeader) => new TableCell({
    width: { size: width, type: WidthType.DXA },
    borders: {
      top: { style: BorderStyle.SINGLE, size: 1, color: "CCCCCC" },
      bottom: { style: BorderStyle.SINGLE, size: 1, color: "CCCCCC" },
      left: { style: BorderStyle.SINGLE, size: 1, color: "CCCCCC" },
      right: { style: BorderStyle.SINGLE, size: 1, color: "CCCCCC" },
    },
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
      ...rows.map((row) => new TableRow({
        children: row.map((cell, i) => makeCell(cell, colWidths[i], false))
      }))
    ]
  });
}

function codeBlock(text) {
  return new Paragraph({
    spacing: { before: 100, after: 100 },
    shading: { fill: 'F0F0F0', type: ShadingType.CLEAR },
    children: [new TextRun({ text, font: 'Courier New', size: 18, color: '1F4E79' })]
  });
}

function imagePara(buffer, w, h, caption) {
  const elements = [];
  
  if (buffer) {
    elements.push(new Paragraph({
      alignment: AlignmentType.CENTER,
      spacing: { before: 160, after: 60 },
      children: [new ImageRun({
        type: 'png',
        data: buffer,
        transformation: { width: w, height: h },
      })]
    }));
  }
  
  if (caption) {
    elements.push(new Paragraph({
      alignment: AlignmentType.CENTER,
      spacing: { before: 60, after: 160 },
      children: [new TextRun({ text: caption, italics: true, size: 20, color: GRAY, font: "Times New Roman" })]
    }));
  }
  
  return elements;
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
            children: [new TextRun({ text: 'Bao cao Do an – LUXE Hair Salon',
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
          children: [new TextRun({ text: 'Khoa Cong nghe Thong tin', bold: false, size: 24, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 400 },
          children: [new TextRun({ text: 'BAO CAO DO AN MON HOC', bold: true, size: 40, font: 'Arial', color: '1F4E79' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 200 },
          children: [new TextRun({ text: 'Lap trinh ADO.NET / ASP.NET Core', bold: false, size: 26, font: 'Arial', color: '595959', italics: true })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 600, after: 200 },
          children: [new TextRun({ text: 'LUXE HAIR SALON', bold: true, size: 52, font: 'Arial', color: '1F4E79' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 800 },
          children: [new TextRun({ text: 'He thong Dat Lich Lam Toc Truc Tuyen', bold: true, size: 30, font: 'Arial', color: '2E75B6' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 400, after: 120 },
          children: [new TextRun({ text: 'Cong nghe su dung:', bold: true, size: 22, font: 'Arial', color: '333333' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 0, after: 600 },
          children: [new TextRun({ text: 'ASP.NET Core 8.0 | SQL Server | Entity Framework Core 8 | JWT Authentication', size: 22, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 600, after: 100 },
          children: [new TextRun({ text: 'Nam hoc: 2025-2026', size: 22, font: 'Arial', color: '595959' })]
        }),
        new Paragraph({ children: [new PageBreak()] }),

        // ==================== MỤC LỤC ====================
        new TableOfContents('MU LUC', {
          hyperlink: true,
          headingStyleRange: '1-3',
        }),
        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 1 ====================
        heading1('CHƯƠNG 1: GIOI THIEU DE TAI'),

        heading2('1.1. Đat van de'),
        bodyText('Trong boi canh kinh te dich vu phat trien manh me tai Viet Nam, nganh cham soc toc va lam dep dang chung kien su tang truong vuot bac. Tuy nhien, phan lon cac salon toc van dang van hanh theo phuong thuc truyen thong: khach hang phai truc tiep den cua hang hoac goi dien de dat lich, dan den nhieu bat tien nhu xep hang cho doi, kho quan ly lich hen va thieu thong tin minh bach ve dich vu.'),
        bodyText('Truoc thuc trang do, nhom thuc hien de tai "LUXE Hair Salon – He thong Dat Lich Lam Toc Truc Tuyen" nham xay dung mot nen tang so hoa toan bo quy trinh dat lich, quan ly nhan vien va theo doi doanh thu cho mot salon toc hien dai.'),

        heading2('1.2. Muc tieu de tai'),
        heading3('1.2.1. Muc tieu tong quat'),
        bodyText('Xay dung he thong dat lich lam toc truc tuyen da nen tang, phuc vu ba nhom nguoi dung chinh: Khach hang, Stylist va Quan tri vien, voi giao dien than thien va quy trinh nghiep vu chat che.'),
        heading3('1.2.2. Muc tieu cu the'),
        bullet('Xay dung he thong dat lich truc tuyen da vai tro: Khach hang, Stylist va Quan tri vien.'),
        bullet('Ap dung kien truc API + MVC tach biet ro rang giua backend va frontend.'),
        bullet('Trien khai xac thuc bao mat voi JWT Bearer Token va Refresh Token.'),
        bullet('Tich hop cac bien phap bao mat hien dai: Rate Limiting, Audit Logging, Security Headers.'),
        bullet('Dam bao trai nghiem nguoi dung muot mat tren ca desktop va thiet bi di dong.'),

        heading2('1.3. Pham vi va gioi han'),
        heading3('1.3.1. Pham vi nghien cuu'),
        bodyText('He thong tap trung vao nghiep vu cot loi cua mot salon toc bao gom: Quan ly dich vu, Dat lich hen (ca khach da dang ky va khach vang lai), Quan ly lich lam viec cua stylist, Danh gia dich vu va Bao cao doanh thu.'),
        heading3('1.3.2. Gioi han de tai'),
        bullet('Chua tich hop thanh toan truc tuyen (VNPay, MoMo)'),
        bullet('Chua co thong bao real-time (SignalR/WebSocket)'),
        bullet('Chua trien khai ung dung mobile rieng'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 2 ====================
        heading1('CHƯƠNG 2: CONG NGHE SU DUNG'),

        heading2('2.1. Tong quan cong nghe'),
        buildTable(
          ['Thanh phan', 'Cong nghe'],
          [
            ['Backend API', 'ASP.NET Core 8.0 Web API'],
            ['Frontend', 'ASP.NET Core 8.0 MVC (Razor)'],
            ['Co so du lieu', 'SQL Server + Entity Framework Core 8'],
            ['Xac thuc', 'JWT Bearer + Refresh Token'],
            ['Giao dien', 'CSS tuy chinh, Responsive'],
            ['Bao mat', 'Rate Limiting, Audit Logging, Security Headers'],
          ],
          [3500, 5860]
        ),

        heading2('2.2. ASP.NET Core 8.0 Web API'),
        bodyText('ASP.NET Core 8.0 la nen tang phat trien ung dung web da nen tang, ma nguon mo cua Microsoft. Du an LUXE Hair Salon ap dung kien truc phan lop ro rang: Controllers (xu ly HTTP), Services (business logic), DTOs (tach biet du lieu), va Middleware (xu ly xuyen suot).'),

        heading2('2.3. Entity Framework Core 8 & SQL Server'),
        bodyText('Entity Framework Core (EF Core) la ORM chinh thuc cua Microsoft cho .NET, cho phep anh xa cac lop C# thanh bang trong SQL Server. Du an su dung Code-First approach voi EF Core Migrations va Data Seeding.'),

        heading2('2.4. Xac thuc JWT Bearer + Refresh Token'),
        bodyText('JSON Web Token (JWT) la tieu chuan mo (RFC 7519) de truyen thong tin xac thuc. He thong su dung: Access Token (15 phut) cho truy cap tai nguyen va Refresh Token (7 ngay) de lam moi khong can dang nhap lai.'),

        heading2('2.5. Bao mat ung dung web'),
        buildTable(
          ['Bien phap bao mat', 'Mo ta', 'Co che thuc hien'],
          [
            ['Rate Limiting', 'Gioi han 100 requests/phut/client', 'RateLimitingMiddleware'],
            ['Audit Logging', 'Ghi log POST/PUT/DELETE', 'AuditLogMiddleware'],
            ['Security Headers', 'Ngan chan XSS, Clickjacking', 'X-Content-Type, X-Frame-Options'],
            ['Password Hashing', 'Ma hoa mat khau mot chieu', 'BCrypt voi salt tu dong'],
            ['SQL Injection Prevention', 'Truy van an toan', 'EF Core Parameterized Queries'],
          ],
          [2500, 3000, 2860]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 3 ====================
        heading1('CHƯƠNG 3: PHAN TICH YEU CAU HE THONG'),

        heading2('3.1. Yeu cau chuc nang'),
        heading3('3.1.1. Nhom chuc nang Khach hang (Customer)'),
        buildTable(
          ['STT', 'Chuc nang', 'Mo ta chi tiet'],
          [
            ['1', 'Xem trang chu', 'Landing page voi hero banner, danh sach dich vu noi bat'],
            ['2', 'Dang ky tai khoan', 'Tao tai khoan moi voi validation email, DT, mat khau'],
            ['3', 'Dang nhap / Dang xuat', 'Xac thuc JWT, luu session'],
            ['4', 'Dat lich 3 buoc', 'Chon dich vu | Stylist & Gio | Xac nhan'],
            ['5', 'Dat lich khach vang lai', 'Dat lich khong can dang nhap'],
            ['6', 'Xem lich cua toi', 'Danh sach lich hen ca nhan, loc theo trang thai'],
            ['7', 'Huy lich hen', 'Huy lich chua duoc xac nhan'],
            ['8', 'Gui danh gia', 'Danh gia 1-5 sao sau khi hoan thanh'],
          ],
          [500, 2500, 6360]
        ),

        heading3('3.1.2. Nhom chuc nang Stylist (Staff)'),
        buildTable(
          ['STT', 'Chuc nang', 'Mo ta chi tiet'],
          [
            ['1', 'Dashboard ca nhan', 'Hien thi lich hen trong ngay, thong ke'],
            ['2', 'Cap nhat trang thai lich', 'Xac nhan/Hoan thanh lich duoc gan'],
          ],
          [500, 2500, 6360]
        ),

        heading3('3.1.3. Nhom chuc nang Quan tri vien (Admin)'),
        buildTable(
          ['STT', 'Chuc nang', 'Mo ta chi tiet'],
          [
            ['1', 'Dashboard tong quan', 'Thong ke lich hen, doanh thu thang'],
            ['2', 'Quan ly lich hen', 'Xem, loc, duyet va doi trang thai'],
            ['3', 'Quan ly dich vu (CRUD)', 'Them, sua, xoa dich vu'],
            ['4', 'Quan ly nhan vien', 'Xem danh sach stylist'],
            ['5', 'Bao cao & Thong ke', 'Doanh thu, top dich vu, nhan vien'],
          ],
          [500, 2500, 6360]
        ),

        heading2('3.2. Yeu cau phi chuc nang'),
        bullet('Hieu suat: He thong phuc vu toi thieu 100 requests/phut/client'),
        bullet('Bao mat: Tuan thu tieu chuan OWASP Top 10'),
        bullet('Kha dung: Giao dien responsive, tuong thich mobile va desktop'),
        bullet('Bao tri: Kien truc phan lop ro rang, code theo nguyen tac SOLID'),
        bullet('Tinh nhat quan: Su dung Unique Constraints, Foreign Keys va transaction'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 4 ====================
        heading1('CHƯƠNG 4: THIET KE HE THONG'),

        heading2('4.1. Kien truc tong the'),
        bodyText('He thong LUXE Hair Salon duoc thiet ke theo mo hinh API + MVC (tach biet Backend va Frontend), giao tiep qua giao thuc REST voi JWT Bearer Token. Luong du lieu di qua ba lop chinh:'),
        bullet('Lop 1 (Presentation): Trinh duyet gui HTTP Request den ASP.NET Core MVC Web'),
        bullet('Lop 2 (Application): MVC Web goi REST API thong qua HttpClient kem JWT Token'),
        bullet('Lop 3 (Data): API truy van SQL Server thong qua Entity Framework Core 8'),

        heading2('4.2. Cau truc du an'),
        buildTable(
          ['Project', 'Vai tro', 'Cong nghe chinh'],
          [
            ['HairSalonVN.API', 'REST API Backend – xu ly business logic', 'ASP.NET Core 8 Web API, EF Core 8'],
            ['HairSalonVN.Web', 'MVC Frontend – giao dien nguoi dung', 'ASP.NET Core 8 MVC, Razor'],
            ['HairSalonVN.Database', 'Data Layer – entity, migration, seeder', 'EF Core 8, SQL Server, Fluent API'],
          ],
          [2500, 4000, 2860]
        ),

        ...imagePara(images.architecture, 580, 320, 'Hinh 4.1: Kien truc tong the he thong - API + MVC'),

        heading2('4.3. So do Use Case'),
        bodyText('He thong co 4 actor chinh va 17 use case duoc phan chia theo vai tro. Khach vang lai (Guest) khong can dang nhap van co the dat lich – day la tinh nang quan trong giup tang ti le chuyen doi.'),
        
        ...imagePara(images.usecase, 620, 400, 'Hinh 4.2: So do Use Case toan he thong (4 actors, 17 use cases)'),

        heading2('4.4. So do Lop (Class Diagram)'),
        bodyText('Domain model bao gom 9 entity chinh. Entity Appointment la trung tam cua he thong, lien ket voi User (CustomerId), Staff (StaffId), Service (ServiceId), Review va Payment.'),
        
        ...imagePara(images.class, 620, 370, 'Hinh 4.3: So do Class - Domain Entity Model'),

        new Paragraph({ children: [new PageBreak()] }),

        heading2('4.5. So do ERD'),
        bodyText('He thong co 9 bang du lieu voi cac moi quan he duoc thiet ke chat che:'),
        
        ...imagePara(images.erd, 620, 430, 'Hinh 4.4: Entity Relationship Diagram (ERD) - 9 bang du lieu'),

        heading2('4.6. So do Sequence - Luong Dat Lich'),
        
        ...imagePara(images.sequence, 600, 350, 'Hinh 4.5: So do Sequence - Luong dat lich hen (3 buoc)'),

        heading2('4.7. Kien truc Bao mat'),
        bodyText('Authentication Flow: User gui POST /api/auth/login voi email va password. API xac minh bang BCrypt, tao JWT Access Token (15 phut) va Refresh Token (7 ngay). Refresh Token duoc luu trong bang RefreshTokens de ho tro thu hoi.'),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 5 ====================
        heading1('CHƯƠNG 5: THIET KE API ENDPOINTS'),

        heading2('5.1. Authentication API'),
        buildTable(
          ['Method', 'Endpoint', 'Mo ta', 'Xac thuc'],
          [
            ['POST', '/api/auth/register', 'Dang ky tai khoan moi', 'Khong'],
            ['POST', '/api/auth/login', 'Dang nhap, nhan Access + Refresh Token', 'Khong'],
            ['POST', '/api/auth/refresh', 'Lam moi Access Token', 'Khong'],
            ['POST', '/api/auth/logout', 'Dang xuat, thu hoi Refresh Token', 'Bearer JWT'],
            ['GET', '/api/auth/me', 'Lay thong tin user hien tai', 'Bearer JWT'],
          ],
          [900, 2200, 3500, 1760]
        ),

        heading2('5.2. Services API'),
        buildTable(
          ['Method', 'Endpoint', 'Mo ta', 'Xac thuc'],
          [
            ['GET', '/api/services', 'Danh sach tat ca dich vu', 'Khong'],
            ['GET', '/api/services/{id}', 'Chi tiet mot dich vu', 'Khong'],
            ['POST', '/api/services', 'Tao dich vu moi', 'Admin'],
            ['PUT', '/api/services/{id}', 'Cap nhat dich vu', 'Admin'],
            ['DELETE', '/api/services/{id}', 'Xoa dich vu', 'Admin'],
          ],
          [900, 2200, 3500, 1760]
        ),

        heading2('5.3. Appointments API'),
        buildTable(
          ['Method', 'Endpoint', 'Mo ta', 'Xac thuc'],
          [
            ['GET', '/api/appointments/available-slots', 'Khung gio trong', 'Khong'],
            ['POST', '/api/appointments/guest', 'Tao lich khach vang lai', 'Khong'],
            ['GET', '/api/appointments/my', 'Lich cua toi', 'Customer'],
            ['POST', '/api/appointments', 'Tao lich hen', 'Customer'],
            ['PUT', '/api/appointments/{id}/status', 'Cap nhat trang thai', 'Staff/Admin'],
            ['GET', '/api/appointments', 'Tat ca lich hen', 'Admin'],
          ],
          [900, 2800, 3200, 1460]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 6 ====================
        heading1('CHƯƠNG 6: TRIEN KHAI VA THU NGHIEM'),

        heading2('6.1. Moi truong phat trien'),
        buildTable(
          ['Thanh phan', 'Phien ban / Cong cu'],
          [
            ['Framework', '.NET 8 SDK'],
            ['IDE', 'Visual Studio 2022 / VS Code'],
            ['Database', 'SQL Server 2022 (LocalDB/Express)'],
            ['API Testing', 'Swagger UI, Postman'],
            ['Version Control', 'Git'],
          ],
          [3500, 5860]
        ),

        heading2('6.2. Cau truc Database Schema'),
        ...imagePara(images.database, 580, 420, 'Hinh 6.1: So do Database Schema - HairSalonDB'),

        heading2('6.3. Luong giao dien nguoi dung'),
        ...imagePara(images.ui_flow, 580, 300, 'Hinh 6.2: Luong giao dien nguoi dung - 3 buoc dat lich'),

        heading2('6.4. Huong dan cai dat va chay'),
        heading3('Buoc 1: Chuan bi co so du lieu'),
        codeBlock('cd HairSalonVN.API && dotnet ef database update --project ../HairSalonVN.Database'),

        heading3('Buoc 2: Khoi chay API'),
        codeBlock('# Terminal 1: cd HairSalonVN.API && dotnet run | http://localhost:7098'),

        heading3('Buoc 3: Khoi chay Web'),
        codeBlock('# Terminal 2: cd HairSalonVN.Web && dotnet run | http://localhost:7126'),

        heading2('6.5. Tai khoan demo'),
        buildTable(
          ['Vai tro', 'Email', 'Mat khau', 'Chuyen mon'],
          [
            ['Admin', 'admin@luxehair.vn', 'Admin@123', 'Toan quyen quan tri'],
            ['Stylist', 'tai.nguyen@luxehair.vn', 'Staff@123', 'Cat & Nhuom'],
            ['Stylist', 'nam.tran@luxehair.vn', 'Staff@123', 'Uon & Duei'],
            ['Khach hang', 'mai.pham@gmail.com', 'Customer@123', 'Tai khoan thu nghiem'],
          ],
          [1600, 2800, 1600, 3360]
        ),

        heading2('6.6. Cac luong nghiep vu chinh'),
        heading3('Luong 1: Dat lich khach vang lai'),
        bullet('Truy cap http://localhost:7126 | Click "Dat lich ngay"'),
        bullet('Buoc 1: Chon dich vu (VD: Cat Toc Nam - 150.000d)'),
        bullet('Buoc 2: Chon ngay, he thong tu dong tai Stylist, chon khung gio trong'),
        bullet('Buoc 3: Dien Ho ten va DT | Submit | Nhan ma dat lich'),

        heading3('Luong 2: Admin quan ly va duyet lich hen'),
        bullet('Dang nhap admin@luxehair.vn / Admin@123'),
        bullet('Dashboard: Xem tong quan so lich hen Pending, doanh thu thang'),
        bullet('Lich hen: Duyet chuyen trang thai Pending | Confirmed'),

        heading2('6.7. Ket qua thu nghiem'),
        buildTable(
          ['Project', 'Ket qua', 'Warnings', 'Errors'],
          [
            ['HairSalonVN.API', 'Thanh cong', '8', '0'],
            ['HairSalonVN.Web', 'Thanh cong', '0', '0'],
          ],
          [3500, 2000, 1500, 1360]
        ),

        new Paragraph({ children: [new PageBreak()] }),

        // ==================== CHƯƠNG 7 ====================
        heading1('CHƯƠNG 7: KET LUAN VA HUONG PHAT TRIEN'),

        heading2('7.1. Ket qua dat duoc'),
        bullet('Xay dung hoan chinh he thong dat lich truc tuyen voi 3 vai tro nguoi dung va 17 use case'),
        bullet('Trien khai kien truc API + MVC tach biet ro rang, de bao tri va mo rong'),
        bullet('Ap dung xac thuc JWT (Access Token 15 phut + Refresh Token 7 ngay)'),
        bullet('Tich hop day du cac lop bao mat: Rate Limiting, Audit Logging, Security Headers, BCrypt'),
        bullet('Ho tro Guest Booking – cho phep khach chua dang ky dat lich ngay lap tuc'),
        bullet('Toan bo project build thanh cong voi 0 Error tren ca API va Web'),

        heading2('7.2. Han che'),
        bullet('Chua tich hop cong thanh toan truc tuyen (VNPay, MoMo)'),
        bullet('Chua co thong bao real-time (SignalR/WebSocket) khi lich hen duoc duyet'),
        bullet('Chua trien khai unit test va integration test day du'),
        bullet('Chua ho tro da ngon ngu (i18n)'),

        heading2('7.3. Huong phat trien tuong lai'),
        bullet('Tich hop cong thanh toan VNPay/MoMo cho dat lich truc tuyen'),
        bullet('Them thong bao real-time bang SignalR khi trang thai lich hen thay doi'),
        bullet('Trien khai ung dung mobile (Flutter/React Native) tich hop voi REST API'),
        bullet('Xay dung he thong goi y Stylist dua tren lich su dat lich (Machine Learning)'),
        bullet('Dua len cloud (Azure/AWS) voi CI/CD pipeline tu dong'),

        heading2('7.4. Tai lieu tham khao'),
        bodyText('1. Microsoft. (2024). ASP.NET Core documentation. https://docs.microsoft.com/aspnet/core'),
        bodyText('2. C# Corner. (2026). Best Practices for Building Web APIs in ASP.NET Core.'),
        bodyText('3. Code Maze. (2024). ASP.NET Core Web API Best Practices.'),
        bodyText('4. Entity Framework Core Documentation. (2024). https://docs.microsoft.com/ef/core'),
        bodyText('5. OWASP Foundation. (2025). OWASP Top Ten Security Risks. owasp.org'),

        new Paragraph({ spacing: { before: 600, after: 200 }, alignment: AlignmentType.CENTER, children: [
          new TextRun({ text: '--- Het ---', size: 22, font: 'Arial', color: '595959', italics: true })
        ]}),
      ]
    }]
  });

  return doc;
}

// RUN
buildDoc().then(async (doc) => {
  const buffer = await Packer.toBuffer(doc);
  const outputPath = path.join(__dirname, 'BaoCao_LUXE_HairSalon_Full.docx');
  fs.writeFileSync(outputPath, buffer);
  console.log('Created:', outputPath);
}).catch(err => {
  console.error('Error:', err);
  process.exit(1);
});
