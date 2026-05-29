# Diagram Generator for LUXE Hair Salon Report
# Run: pip install pillow && python draw_diagrams.py

from PIL import Image, ImageDraw, ImageFont
import os

# Colors
BLUE = "#1F4E79"
LBLUE = "#2E75B6"
ACCENT = "#D6E4F0"
GRAY = "#595959"
LGRAY = "#F2F2F2"
WHITE = "#FFFFFF"
GREEN = "#375623"
LGREEN = "#E2EFDA"
BROWN = "#7F5539"
RED = "#C00000"

# Output folder
OUTPUT_DIR = "d:/HairSalonVN/diagrams"
os.makedirs(OUTPUT_DIR, exist_ok=True)

def hex_to_rgb(hex_color):
    hex_color = hex_color.lstrip('#')
    return tuple(int(hex_color[i:i+2], 16) for i in (0, 2, 4))

def draw_rounded_rect(draw, coords, radius, fill, outline=None, width=1):
    x1, y1, x2, y2 = coords
    draw.rectangle([x1, y1, x2, y2], fill=fill, outline=outline, width=width)

def draw_centered_text(draw, text, center_x, y, font, color):
    bbox = draw.textbbox((0, 0), text, font=font)
    text_width = bbox[2] - bbox[0]
    x = center_x - text_width // 2
    draw.text((x, y), text, fill=color, font=font)

# ==================== 1. ARCHITECTURE DIAGRAM ====================
def draw_architecture():
    w, h = 900, 500
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    # Border
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    
    # Title
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 14, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 11, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    
    draw_centered_text(draw, "Kiến trúc Hệ thống – API + MVC", w//2, 15, title_font, BLUE)
    
    # Browser box
    draw_rounded_rect(draw, [50, 100, 200, 200], 10, BLUE)
    draw_centered_text(draw, "Trình duyệt", 125, 130, bold_font, WHITE)
    draw_centered_text(draw, "(Browser)", 125, 150, small_font, ACCENT)
    draw_centered_text(draw, "HTTP Request", 125, 170, tiny_font, ACCENT)
    
    # Arrow 1
    draw.line([200, 150, 260, 150], fill=LBLUE, width=2)
    draw.polygon([(260, 145), (270, 150), (260, 155)], fill=LBLUE)
    draw_centered_text(draw, "HTTP Request", 230, 125, tiny_font, GRAY)
    
    # MVC Web box
    draw_rounded_rect(draw, [270, 80, 480, 220], 10, LBLUE)
    draw_centered_text(draw, "HairSalonVN.Web", 375, 95, bold_font, WHITE)
    draw_centered_text(draw, "ASP.NET Core MVC", 375, 115, small_font, ACCENT)
    draw_centered_text(draw, "(Razor Views)", 375, 130, small_font, ACCENT)
    draw_centered_text(draw, "• Controllers", 280, 155, tiny_font, WHITE)
    draw_centered_text(draw, "• Views (Razor)", 280, 168, tiny_font, WHITE)
    draw_centered_text(draw, "• Services", 280, 181, tiny_font, WHITE)
    draw_centered_text(draw, "• Session (JWT)", 280, 194, tiny_font, WHITE)
    
    # Arrow 2
    draw.line([480, 150, 540, 150], fill=LBLUE, width=2)
    draw.polygon([(540, 145), (550, 150), (540, 155)], fill=LBLUE)
    draw_centered_text(draw, "REST API", 510, 125, tiny_font, GRAY)
    draw_centered_text(draw, "(JWT Bearer)", 510, 138, tiny_font, GRAY)
    
    # API box
    draw_rounded_rect(draw, [550, 80, 760, 220], 10, GREEN)
    draw_centered_text(draw, "HairSalonVN.API", 655, 95, bold_font, WHITE)
    draw_centered_text(draw, "ASP.NET Core Web API", 655, 115, small_font, LGREEN)
    draw_centered_text(draw, "• Controllers", 560, 155, tiny_font, WHITE)
    draw_centered_text(draw, "• Services", 560, 168, tiny_font, WHITE)
    draw_centered_text(draw, "• DTOs", 560, 181, tiny_font, WHITE)
    draw_centered_text(draw, "• Middleware", 560, 194, tiny_font, WHITE)
    
    # Arrow 3
    draw.line([655, 220, 655, 280], fill=LBLUE, width=2)
    draw.polygon([(650, 275), (655, 285), (660, 275)], fill=LBLUE)
    draw_centered_text(draw, "EF Core Query", 680, 245, tiny_font, GRAY)
    
    # Database box
    draw_rounded_rect(draw, [560, 290, 750, 380], 10, BROWN)
    draw_centered_text(draw, "SQL Server", 655, 305, bold_font, WHITE)
    draw_centered_text(draw, "Entity Framework Core 8", 655, 325, small_font, LGREEN)
    draw_centered_text(draw, "HairSalonDB", 655, 345, small_font, LGREEN)
    
    # Legend
    draw_rounded_rect(draw, [50, 300, 200, 380], 8, LGRAY, LBLUE)
    draw_centered_text(draw, "Chú thích:", 125, 310, small_font, BLUE)
    draw_rounded_rect(draw, [60, 328, 72, 340], 2, BLUE)
    draw.text((78, 325), "Lớp giao diện", fill=GRAY, font=tiny_font)
    draw_rounded_rect(draw, [60, 348, 72, 360], 2, LBLUE)
    draw.text((78, 345), "Lớp xử lý", fill=GRAY, font=tiny_font)
    draw_rounded_rect(draw, [60, 368, 72, 380], 2, GREEN)
    draw.text((78, 365), "Lớp dữ liệu", fill=GRAY, font=tiny_font)
    
    # Save
    path = f"{OUTPUT_DIR}/1_architecture_diagram.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

def draw_usecase():
    w, h = 1000, 650
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 12, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 10, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    
    # Border
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Sơ đồ Use Case – Hệ thống LUXE Hair Salon", w//2, 12, title_font, BLUE)
    
    # Actors (left side)
    actors = [
        (60, "Khách vãng lai"),
        (60, "Khách hàng"),
        (60, "Stylist"),
        (60, "Admin")
    ]
    actor_y = [80, 180, 300, 430]
    
    def draw_actor(draw, x, y, label):
        # Head
        draw.ellipse([x-12, y-30, x+12, y-6], outline=BLUE, width=2)
        # Body
        draw.line([x, y-6, x, y+20], fill=BLUE, width=2)
        # Arms
        draw.line([x-18, y+5, x+18, y+5], fill=BLUE, width=2)
        # Legs
        draw.line([x, y+20, x-14, y+42], fill=BLUE, width=2)
        draw.line([x, y+20, x+14, y+42], fill=BLUE, width=2)
        # Label
        draw_centered_text(draw, label, x, y+50, tiny_font, BLUE)
    
    for i, (x, label) in enumerate(actors):
        draw_actor(draw, x, actor_y[i], label)
    
    # System boundary
    draw_rounded_rect(draw, [165, 55, 970, 610], 10, LGRAY, BLUE, width=2)
    draw.text((170, 60), "Hệ thống LUXE Hair Salon", fill=BLUE, font=small_font)
    
    # Use Cases (ellipses) - arranged in grid
    usecases = [
        # Row 1 - Guest
        (300, 90, "Xem trang chủ"),
        (500, 90, "Đăng ký"),
        (700, 90, "Đặt lịch\n(Guest)"),
        
        # Row 2 - Customer
        (300, 180, "Đặt lịch\n(Đã ĐK)"),
        (500, 180, "Xem lịch\ncủa tôi"),
        (700, 180, "Gửi đánh\ngiá"),
        (870, 180, "Hủy lịch"),
        
        # Row 3 - Staff
        (300, 300, "Xem ca\nlàm việc"),
        (500, 300, "Cập nhật\ntrạng thái"),
        
        # Row 4 - Admin
        (300, 420, "Dashboard\nAdmin"),
        (500, 420, "Quản lý\nlịch hẹn"),
        (700, 420, "CRUD\ndịch vụ"),
        (870, 420, "Xem nhân\nviên"),
        (870, 300, "Đăng nhập/\nĐăng xuất"),
    ]
    
    def draw_usecase_ellipse(draw, cx, cy, label):
        lines = label.split('\n')
        width = max(len(l) for l in lines) * 8 + 30
        height = len(lines) * 14 + 20
        draw.ellipse([cx-width//2, cy-height//2, cx+width//2, cy+height//2], fill=ACCENT, outline=BLUE, width=2)
        y_offset = cy - (len(lines)-1) * 7
        for line in lines:
            draw_centered_text(draw, line, cx, y_offset, tiny_font, BLUE)
            y_offset += 14
    
    for cx, cy, label in usecases:
        draw_usecase_ellipse(draw, cx, cy, label)
    
    # Connections (dashed lines)
    # Guest actors to use cases
    draw.line([72, 90, 240, 90], fill=GRAY, width=1)
    draw.line([240, 90, 240, 100], fill=GRAY, width=1)
    draw.line([240, 100, 255, 100], fill=GRAY, width=1)
    
    # Save
    path = f"{OUTPUT_DIR}/2_usecase_diagram.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== 3. ERD DIAGRAM ====================
def draw_erd():
    w, h = 1000, 700
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 11, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 8, encoding="utf-8")
    
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Entity Relationship Diagram (ERD) – LUXE Hair Salon", w//2, 12, title_font, BLUE)
    
    def draw_entity(draw, x, y, w, h, name, fields, header_color):
        # Header
        draw_rounded_rect(draw, [x, y, x+w, y+25], 5, header_color)
        draw_centered_text(draw, name, x+w//2, y+5, bold_font, WHITE)
        # Body
        draw.rectangle([x, y+25, x+w, y+h], fill=WHITE, outline=header_color, width=1)
        # Fields
        for i, field in enumerate(fields):
            fy = y + 30 + i * 14
            is_pk = field.startswith("PK")
            is_fk = field.startswith("FK")
            color = BLUE if is_pk else BROWN if is_fk else GRAY
            font = bold_font if (is_pk or is_fk) else tiny_font
            draw.text((x+5, fy), field, fill=color, font=font)
    
    # Draw entities
    # USERS (top left)
    draw_entity(draw, 20, 60, 160, 130, "USERS", [
        "PK  Id (Guid)",
        "FullName",
        "Email (UK)",
        "PasswordHash",
        "Phone",
        "Role",
        "IsActive"
    ], BLUE)
    
    # STAFFS (top middle)
    draw_entity(draw, 200, 60, 160, 120, "STAFFS", [
        "PK  Id (Guid)",
        "FK  UserId",
        "Specialty",
        "Bio",
        "IsAvailable"
    ], LBLUE)
    
    # SERVICES (top right)
    draw_entity(draw, 380, 60, 160, 120, "SERVICES", [
        "PK  Id (Guid)",
        "Name",
        "Description",
        "Price",
        "DurationMinutes",
        "IsActive"
    ], GREEN)
    
    # STAFF_SERVICES
    draw_entity(draw, 560, 60, 140, 60, "STAFF_SERVICES", [
        "FK  StaffId",
        "FK  ServiceId"
    ], BROWN)
    
    # APPOINTMENTS (center)
    draw_entity(draw, 250, 230, 200, 200, "APPOINTMENTS", [
        "PK  Id (Guid)",
        "FK  CustomerId (nullable)",
        "FK  StaffId",
        "FK  ServiceId",
        "AppointmentDate",
        "Status",
        "BookingCode",
        "IsGuestBooking",
        "GuestName",
        "GuestPhone",
        "GuestEmail"
    ], BLUE)
    
    # REVIEWS
    draw_entity(draw, 480, 260, 170, 100, "REVIEWS", [
        "PK  Id (Guid)",
        "FK  AppointmentId (UK)",
        "FK  CustomerId",
        "Rating",
        "Comment"
    ], LBLUE)
    
    # PAYMENTS
    draw_entity(draw, 480, 390, 170, 90, "PAYMENTS", [
        "PK  Id (Guid)",
        "FK  AppointmentId (UK)",
        "Amount",
        "Method",
        "Status"
    ], GREEN)
    
    # WORKING_HOURS
    draw_entity(draw, 20, 230, 160, 90, "WORKING_HOURS", [
        "PK  Id (Guid)",
        "FK  StaffId",
        "DayOfWeek",
        "StartTime",
        "EndTime"
    ], BROWN)
    
    # REFRESH_TOKENS
    draw_entity(draw, 20, 380, 160, 90, "REFRESH_TOKENS", [
        "PK  Id (Guid)",
        "FK  UserId",
        "Token",
        "ExpiresAt"
    ], GRAY)
    
    # Relationship lines (dashed)
    draw.line([100, 190, 200, 130], fill=GRAY, width=1)
    draw_centered_text(draw, "1:0..1", 140, 155, tiny_font, GRAY)
    
    # Legend
    draw_rounded_rect(draw, [700, 520, 980, 680], 8, LGRAY, LBLUE)
    draw_centered_text(draw, "Chú thích:", 840, 530, small_font, BLUE)
    draw.rectangle([715, 555, 725, 565], fill=BLUE)
    draw.text((732, 553), "PK – Primary Key", fill=GRAY, font=tiny_font)
    draw.rectangle([715, 575, 725, 585], fill=BROWN)
    draw.text((732, 573), "FK – Foreign Key", fill=GRAY, font=tiny_font)
    draw.rectangle([715, 595, 725, 605], fill=GREEN)
    draw.text((732, 593), "UK – Unique", fill=GRAY, font=tiny_font)
    
    # Save
    path = f"{OUTPUT_DIR}/3_erd_diagram.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== 4. SEQUENCE DIAGRAM ====================
def draw_sequence():
    w, h = 950, 550
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 11, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 10, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Sơ đồ Sequence – Luồng Đặt Lịch Hẹn", w//2, 12, title_font, BLUE)
    
    # Participants
    participants = [
        (80, "Khách hàng"),
        (260, "BookingController\n(Web MVC)"),
        (450, "AppointmentsController\n(API)"),
        (650, "AppointmentService\n(Business)"),
        (850, "SQL Server\n(Database)")
    ]
    
    # Draw participant boxes and lifelines
    for x, name in participants:
        # Box
        lines = name.split('\n')
        draw_rounded_rect(draw, [x-50, 50, x+50, 85], 5, LBLUE)
        y_offset = 55
        for line in lines:
            draw_centered_text(draw, line, x, y_offset, tiny_font, WHITE)
            y_offset += 12
        # Lifeline
        draw.line([x, 85, x, 530], fill=GRAY, width=1)
    
    # Step 1: Get services
    y = 120
    draw.line([80, y, 260, y], fill=BLUE, width=2)
    draw.polygon([(260, y-5), (270, y), (260, y+5)], fill=BLUE)
    draw_centered_text(draw, "GET /Booking", 170, y-18, tiny_font, GRAY)
    
    y = 145
    draw.line([260, y, 450, y], fill=BLUE, width=2)
    draw.polygon([(450, y-5), (460, y), (450, y+5)], fill=BLUE)
    draw_centered_text(draw, "GET /api/services", 355, y-18, tiny_font, GRAY)
    
    y = 170
    draw.line([450, y, 850, y], fill=BLUE, width=2)
    draw.polygon([(850, y-5), (860, y), (850, y+5)], fill=BLUE)
    draw_centered_text(draw, "SELECT Services", 650, y-18, tiny_font, GRAY)
    
    y = 195
    draw.line([850, y, 450, y], fill=GRAY, width=1, joint="curve")
    draw.polygon([(450, y-5), (440, y), (450, y+5)], fill=GRAY)
    
    y = 220
    draw.line([450, y, 260, y], fill=GRAY, width=1)
    draw.polygon([(260, y-5), (250, y), (260, y+5)], fill=GRAY)
    
    y = 245
    draw.line([260, y, 80, y], fill=GRAY, width=1)
    draw.polygon([(80, y-5), (70, y), (80, y+5)], fill=GRAY)
    
    # Step 2: Select slot
    y = 290
    draw_rounded_rect(draw, [25, y-15, 150, y+5], 5, LGRAY, GRAY)
    draw_centered_text(draw, "[Chọn dịch vụ]", 87, y-8, tiny_font, GRAY)
    
    y = 320
    draw.line([80, y, 260, y], fill=BLUE, width=2)
    draw.polygon([(260, y-5), (270, y), (260, y+5)], fill=BLUE)
    draw_centered_text(draw, "GET /SelectSlot", 170, y-18, tiny_font, GRAY)
    
    y = 345
    draw.line([260, y, 450, y], fill=BLUE, width=2)
    draw.polygon([(450, y-5), (460, y), (450, y+5)], fill=BLUE)
    draw_centered_text(draw, "GET /api/staff", 355, y-18, tiny_font, GRAY)
    
    # Step 3: Confirm booking
    y = 400
    draw_rounded_rect(draw, [10, y-15, 165, y+5], 5, LGREEN, GREEN)
    draw_centered_text(draw, "[Xác nhận đặt lịch]", 87, y-8, tiny_font, GREEN)
    
    y = 430
    draw.line([80, y, 260, y], fill=BLUE, width=2)
    draw.polygon([(260, y-5), (270, y), (260, y+5)], fill=BLUE)
    draw_centered_text(draw, "POST /Confirm", 170, y-18, tiny_font, GRAY)
    
    y = 455
    draw.line([260, y, 450, y], fill=BLUE, width=2)
    draw.polygon([(450, y-5), (460, y), (450, y+5)], fill=BLUE)
    draw_centered_text(draw, "POST /api/appointments", 355, y-18, tiny_font, GRAY)
    
    y = 480
    draw.line([450, y, 650, y], fill=BLUE, width=2)
    draw.polygon([(650, y-5), (660, y), (650, y+5)], fill=BLUE)
    draw_centered_text(draw, "CreateAppointment()", 550, y-18, tiny_font, GRAY)
    
    y = 505
    draw.line([650, y, 850, y], fill=BLUE, width=2)
    draw.polygon([(850, y-5), (860, y), (850, y+5)], fill=BLUE)
    draw_centered_text(draw, "INSERT", 750, y-18, tiny_font, GRAY)
    
    # Save
    path = f"{OUTPUT_DIR}/4_sequence_diagram.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== 5. CLASS DIAGRAM ====================
def draw_class():
    w, h = 960, 580
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 11, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 8, encoding="utf-8")
    
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Sơ đồ Class – Domain Entity Model", w//2, 12, title_font, BLUE)
    
    def draw_class_box(draw, x, y, w, h, name, attrs, color):
        # Header
        draw_rounded_rect(draw, [x, y, x+w, y+22], 5, color)
        draw_centered_text(draw, "«Entity» " + name, x+w//2, y+4, bold_font, WHITE)
        # Body
        draw.rectangle([x, y+22, x+w, y+h], fill=WHITE, outline=color, width=1)
        # Divider
        draw.line([x, y+22, x+w, y+22], fill=GRAY, width=1)
        # Attributes
        for i, attr in enumerate(attrs):
            fy = y + 28 + i * 13
            draw.text((x+5, fy), attr, fill=GRAY, font=tiny_font)
    
    # Row 1: Top entities
    draw_class_box(draw, 10, 50, 160, 140, "User", [
        "+Id: Guid [PK]",
        "+FullName: string",
        "+Email: string [UK]",
        "+PasswordHash: string",
        "+Phone: string",
        "+Role: string",
        "+IsActive: bool",
        "+CreatedAt: DateTime"
    ], BLUE)
    
    draw_class_box(draw, 190, 50, 155, 115, "Staff", [
        "+Id: Guid [PK]",
        "+UserId: Guid [FK]",
        "+Specialty: string",
        "+Bio: string",
        "+AvatarUrl: string",
        "+IsAvailable: bool"
    ], LBLUE)
    
    draw_class_box(draw, 365, 50, 160, 115, "Service", [
        "+Id: Guid [PK]",
        "+Name: string",
        "+Description: string",
        "+Price: decimal",
        "+DurationMinutes: int",
        "+IsActive: bool"
    ], GREEN)
    
    draw_class_box(draw, 545, 50, 145, 60, "StaffService", [
        "+StaffId: Guid [FK]",
        "+ServiceId: Guid [FK]"
    ], BROWN)
    
    draw_class_box(draw, 710, 50, 155, 90, "WorkingHour", [
        "+Id: Guid [PK]",
        "+StaffId: Guid [FK]",
        "+DayOfWeek: int",
        "+StartTime: TimeSpan",
        "+EndTime: TimeSpan"
    ], GRAY)
    
    # Row 2: Core entities
    draw_class_box(draw, 10, 310, 175, 185, "Appointment", [
        "+Id: Guid [PK]",
        "+CustomerId: Guid? [FK]",
        "+StaffId: Guid [FK]",
        "+ServiceId: Guid [FK]",
        "+AppointmentDate",
        "+Status: string",
        "+BookingCode: string",
        "+IsGuestBooking: bool",
        "+GuestName: string?",
        "+GuestPhone: string?",
        "+GuestEmail: string?"
    ], BLUE)
    
    draw_class_box(draw, 210, 310, 160, 100, "Review", [
        "+Id: Guid [PK]",
        "+AppointmentId [FK]",
        "+CustomerId: Guid [FK]",
        "+Rating: int",
        "+Comment: string?",
        "+CreatedAt: DateTime"
    ], LBLUE)
    
    draw_class_box(draw, 395, 310, 155, 90, "Payment", [
        "+Id: Guid [PK]",
        "+AppointmentId [FK]",
        "+Amount: decimal",
        "+Method: string",
        "+Status: string",
        "+PaidAt: DateTime?"
    ], GREEN)
    
    draw_class_box(draw, 575, 310, 155, 80, "RefreshToken", [
        "+Id: Guid [PK]",
        "+UserId: Guid [FK]",
        "+Token: string",
        "+ExpiresAt: DateTime",
        "+IsRevoked: bool"
    ], BROWN)
    
    # Save
    path = f"{OUTPUT_DIR}/5_class_diagram.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== 6. UI FLOW DIAGRAM ====================
def draw_ui_flow():
    w, h = 900, 500
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 12, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 10, encoding="utf-8")
    
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Luồng giao diện người dùng – 3 bước đặt lịch", w//2, 15, title_font, BLUE)
    
    # Step 1
    draw_rounded_rect(draw, [50, 80, 250, 200], 10, LBLUE)
    draw_centered_text(draw, "Bước 1", 150, 90, bold_font, WHITE)
    draw_centered_text(draw, "Chọn Dịch vụ", 150, 110, small_font, ACCENT)
    draw.text((60, 135), "• Trang chủ → Đặt lịch", fill=WHITE, font=small_font)
    draw.text((60, 155), "• Chọn dịch vụ", fill=WHITE, font=small_font)
    draw.text((60, 175), "• Xem giá, thời gian", fill=WHITE, font=small_font)
    
    # Arrow 1
    draw.line([250, 140, 300, 140], fill=LBLUE, width=3)
    draw.polygon([(295, 135), (310, 140), (295, 145)], fill=LBLUE)
    
    # Step 2
    draw_rounded_rect(draw, [310, 80, 510, 200], 10, LBLUE)
    draw_centered_text(draw, "Bước 2", 410, 90, bold_font, WHITE)
    draw_centered_text(draw, "Chọn Stylist & Giờ", 410, 110, small_font, ACCENT)
    draw.text((320, 135), "• Chọn ngày", fill=WHITE, font=small_font)
    draw.text((320, 155), "• Chọn stylist", fill=WHITE, font=small_font)
    draw.text((320, 175), "• Chọn khung giờ", fill=WHITE, font=small_font)
    
    # Arrow 2
    draw.line([510, 140, 560, 140], fill=LBLUE, width=3)
    draw.polygon([(555, 135), (570, 140), (555, 145)], fill=LBLUE)
    
    # Step 3
    draw_rounded_rect(draw, [570, 80, 770, 200], 10, LBLUE)
    draw_centered_text(draw, "Bước 3", 670, 90, bold_font, WHITE)
    draw_centered_text(draw, "Xác nhận đặt lịch", 670, 110, small_font, ACCENT)
    draw.text((580, 135), "• Nhập thông tin", fill=WHITE, font=small_font)
    draw.text((580, 155), "• Xác nhận", fill=WHITE, font=small_font)
    draw.text((580, 175), "• Nhận mã đặt lịch", fill=WHITE, font=small_font)
    
    # Arrow down
    draw.line([670, 200, 670, 250], fill=GREEN, width=3)
    draw.polygon([(665, 245), (670, 260), (675, 245)], fill=GREEN)
    
    # Success
    draw_rounded_rect(draw, [520, 270, 820, 400], 10, GREEN)
    draw_centered_text(draw, "✓ Thành công", 670, 285, bold_font, WHITE)
    draw_centered_text(draw, "Trang xác nhận", 670, 310, small_font, LGREEN)
    draw.text((535, 335), "• Mã đặt lịch: HS260529XXXXXX", fill=WHITE, font=small_font)
    draw.text((535, 355), "• Ngày giờ đã đặt", fill=WHITE, font=small_font)
    draw.text((535, 375), "• Thông tin stylist", fill=WHITE, font=small_font)
    
    # Alternative path
    draw.line([150, 200, 150, 240], fill=RED, width=2, joint="curve")
    draw.polygon([(145, 235), (150, 250), (155, 235)], fill=RED)
    draw_rounded_rect(draw, [50, 250, 250, 340], 8, "#FFF0F0", RED)
    draw_centered_text(draw, "Nếu chưa đăng nhập:", 150, 260, small_font, RED)
    draw.text((60, 280), "• Đăng ký tài khoản mới", fill=RED, font=small_font)
    draw.text((60, 300), "• Hoặc điền thông tin khách", fill=RED, font=small_font)
    draw.text((60, 320), "  (Guest Booking)", fill=RED, font=small_font)
    
    # Legend
    draw_rounded_rect(draw, [50, 420, 300, 480], 8, LGRAY, LBLUE)
    draw.text((60, 430), "→ Luồng chính", fill=BLUE, font=small_font)
    draw.text((60, 450), "→ Luồng khách vãng lai", fill=RED, font=small_font)
    
    # Save
    path = f"{OUTPUT_DIR}/6_ui_flow.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== 7. DATABASE SCHEMA ====================
def draw_database():
    w, h = 800, 600
    img = Image.new('RGB', (w, h), WHITE)
    draw = ImageDraw.Draw(img)
    
    title_font = ImageFont.truetype("arial.ttf", 18, encoding="utf-8")
    bold_font = ImageFont.truetype("arial.ttf", 11, encoding="utf-8")
    small_font = ImageFont.truetype("arial.ttf", 9, encoding="utf-8")
    tiny_font = ImageFont.truetype("arial.ttf", 8, encoding="utf-8")
    
    draw.rectangle([2, 2, w-3, h-3], outline=LBLUE, width=2)
    draw_centered_text(draw, "Sơ đồ Database Schema – HairSalonDB", w//2, 12, title_font, BLUE)
    
    def draw_table(draw, x, y, w, h, name, cols, color):
        # Header
        draw_rounded_rect(draw, [x, y, x+w, y+20], 3, color)
        draw_centered_text(draw, name, x+w//2, y+3, bold_font, WHITE)
        # Columns
        draw.rectangle([x, y+20, x+w, y+h], fill=WHITE, outline=color, width=1)
        for i, col in enumerate(cols):
            fy = y + 24 + i * 14
            draw.text((x+5, fy), col, fill=GRAY, font=tiny_font)
    
    # Tables
    draw_table(draw, 20, 50, 180, 110, "USERS", [
        "PK Id (uniqueidentifier)",
        "FullName (nvarchar)",
        "Email (nvarchar) UNIQUE",
        "PasswordHash (nvarchar)",
        "Phone (nvarchar)",
        "Role (nvarchar)"
    ], BLUE)
    
    draw_table(draw, 220, 50, 160, 90, "STAFFS", [
        "PK Id (uniqueidentifier)",
        "FK UserId (uniqueidentifier)",
        "Specialty (nvarchar)",
        "Bio (nvarchar)",
        "IsAvailable (bit)"
    ], LBLUE)
    
    draw_table(draw, 400, 50, 160, 100, "SERVICES", [
        "PK Id (uniqueidentifier)",
        "Name (nvarchar)",
        "Description (nvarchar)",
        "Price (decimal)",
        "Duration (int)",
        "IsActive (bit)"
    ], GREEN)
    
    draw_table(draw, 580, 50, 200, 150, "APPOINTMENTS", [
        "PK Id (uniqueidentifier)",
        "FK CustomerId (null)",
        "FK StaffId (uniqueidentifier)",
        "FK ServiceId (uniqueidentifier)",
        "AppointmentDate (datetime)",
        "Status (nvarchar)",
        "BookingCode (nvarchar)",
        "IsGuestBooking (bit)",
        "GuestName (nvarchar)",
        "GuestPhone (nvarchar)"
    ], BLUE)
    
    draw_table(draw, 20, 200, 160, 70, "REVIEWS", [
        "PK Id (uniqueidentifier)",
        "FK AppointmentId (unique)",
        "FK CustomerId (uniqueidentifier)",
        "Rating (int)"
    ], LBLUE)
    
    draw_table(draw, 220, 200, 160, 70, "PAYMENTS", [
        "PK Id (uniqueidentifier)",
        "FK AppointmentId (unique)",
        "Amount (decimal)",
        "Method (nvarchar)",
        "Status (nvarchar)"
    ], GREEN)
    
    draw_table(draw, 400, 200, 180, 80, "WORKING_HOURS", [
        "PK Id (uniqueidentifier)",
        "FK StaffId (uniqueidentifier)",
        "DayOfWeek (int)",
        "StartTime (time)",
        "EndTime (time)"
    ], BROWN)
    
    draw_table(draw, 580, 200, 180, 70, "STAFF_SERVICES", [
        "FK StaffId (uniqueidentifier)",
        "FK ServiceId (uniqueidentifier)"
    ], GRAY)
    
    # Legend
    draw_rounded_rect(draw, [20, 520, 780, 580], 8, LGRAY, LBLUE)
    draw.text((30, 530), "PK = Primary Key    FK = Foreign Key    UNIQUE = Unique Constraint    NULL = Nullable", fill=GRAY, font=small_font)
    draw.text((30, 550), "SQL Server 2022 + Entity Framework Core 8 | Code-First Migration", fill=GRAY, font=small_font)
    
    # Save
    path = f"{OUTPUT_DIR}/7_database_schema.png"
    img.save(path, "PNG")
    print(f"Created: {path}")
    return path

# ==================== RUN ALL ====================
if __name__ == "__main__":
    print("Drawing diagrams for LUXE Hair Salon Report...")
    print("-" * 50)
    
    paths = []
    paths.append(draw_architecture())
    paths.append(draw_usecase())
    paths.append(draw_erd())
    paths.append(draw_sequence())
    paths.append(draw_class())
    paths.append(draw_ui_flow())
    paths.append(draw_database())
    
    print("-" * 50)
    print(f"All diagrams created in: {OUTPUT_DIR}")
    for p in paths:
        print(f"   - {p}")
