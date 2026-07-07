# Payroll & Report Service

Microservice quản lý lương và báo cáo nhân sự - Nhóm 3 HRM System.

## 📋 Yêu cầu

- Docker & Docker Compose (khuyến nghị)
- Hoặc .NET 8.0 SDK + MySQL 8.0+ (nếu chạy local)

## 📁 Cấu trúc Project
payroll-report-service/
├── Controllers/        # API Controllers
│   ├── PayrollsController.cs
│   ├── AttendanceTestController.cs
│   ├── HrServiceTestController.cs
│   └── TestController.cs
├── Models/             # Entity Models
│   └── Payroll.cs
├── DTOs/               # Data Transfer Objects
│   ├── PayrollDTO.cs
│   ├── AttendanceDTO.cs
│   └── HrContractDTOs.cs
├── Data/               # DbContext
│   └── PayrollDbContext.cs
├── Services/           # Business Logic
│   ├── PayrollService.cs
│   ├── HrServiceClient.cs
│   ├── AttendanceServiceClient.cs
│   ├── EmailService.cs
│   └── PayrollCalculatorService.cs
├── Repositories/       # Data Access Layer
│   └── PayrollRepository.cs
├── Migrations/         # EF Core Migrations
├── appsettings.json
├── Program.cs
├── Dockerfile
├── docker-compose.yml
└── payroll-service.csproj

## 🚀 Hướng dẫn chạy

### Cách 1: Chạy bằng Docker (Khuyến nghị)

> Yêu cầu: Cài **Docker Desktop** tại https://www.docker.com/products/docker-desktop và đảm bảo Docker đang chạy.

#### Bước 1: Clone repo về

```bash
git clone https://github.com/namisu-26/fullstack.git
```

#### Bước 2: Vào đúng thư mục chứa docker-compose

```bash
cd fullstack\payroll-report-service\payroll-report-service
```

#### Bước 3: Build và chạy

```bash
docker-compose up --build
```

Lần đầu chạy sẽ mất 2-3 phút để pull image và build. Khi thấy log `Hosting started` là thành công.

#### Bước 4: Truy cập Swagger
http://localhost:5000/swagger

#### Dừng service

```bash
docker-compose down
```

#### Dừng và xóa toàn bộ data DB

```bash
docker-compose down -v
```

---

### Cách 2: Chạy Local (Development)

#### Bước 1: Yêu cầu

- .NET 8.0 SDK
- MySQL 8.0 đang chạy local
- Connection string trong `appsettings.json` trỏ đúng MySQL local

#### Bước 2: Clone và vào thư mục

```bash
git clone https://github.com/namisu-26/fullstack.git
cd fullstack\payroll-report-service\payroll-report-service
```

#### Bước 3: Restore và migrate

```bash
dotnet restore
dotnet ef database update
```

#### Bước 4: Chạy

```bash
dotnet run
```

#### Bước 5: Truy cập Swagger
http://localhost:5000/swagger

---

## 🧪 Test API

### Lấy token trước khi test

Tất cả các endpoint đều yêu cầu JWT token. Lấy token từ HR Core Service (Nhóm 1):
GET https://100.88.26.35:7084/api/v1/hr/TestAuth/generate-token

Copy token, vào Swagger click nút **Authorize 🔒** ở góc trên phải → nhập `Bearer <token>` → **Authorize**.

### Các endpoint chính

#### 1. Tính lương tất cả nhân viên theo tháng (POST)
POST /api/Payrolls/calculate-all

```json
{
  "month": "2026-06"
}
```

#### 2. Lấy danh sách bảng lương (GET)
GET /api/Payrolls
GET /api/Payrolls?payPeriod=2026-06

#### 3. Lấy phiếu lương chi tiết (GET)
GET /api/Payrolls/{id}/payslip

#### 4. Duyệt bảng lương và gửi email (POST)
POST /api/Payrolls/approve

```json
{
  "payPeriod": "2026-06",
  "sendEmail": true
}
```

#### 5. Lương của nhân viên đang đăng nhập (GET)
GET /api/Payrolls/my-payroll
GET /api/Payrolls/my-payroll?payPeriod=2026-06

#### 6. Báo cáo tổng lương (GET)
GET /api/Payrolls/report/summary

---

## 📝 Toàn bộ API Endpoints

| Method | Endpoint | Quyền | Mô tả |
|--------|----------|-------|-------|
| GET | `/api/Payrolls` | Admin/Employee | Danh sách bảng lương |
| GET | `/api/Payrolls/{id}` | Admin/Employee* | Chi tiết bảng lương |
| GET | `/api/Payrolls/{id}/payslip` | Admin/Employee* | Phiếu lương chi tiết |
| GET | `/api/Payrolls/my-payroll` | Employee | Lương của mình |
| GET | `/api/Payrolls/report/summary` | Admin | Báo cáo tổng hợp |
| POST | `/api/Payrolls` | Admin | Tạo bảng lương thủ công |
| POST | `/api/Payrolls/calculate-all` | Admin | Tính lương tự động theo tháng |
| POST | `/api/Payrolls/approve` | Admin | Duyệt và gửi phiếu lương qua email |
| PUT | `/api/Payrolls/{id}` | Admin | Cập nhật bảng lương |
| DELETE | `/api/Payrolls/{id}` | Admin | Xóa bảng lương |

*Employee chỉ xem được bảng lương của chính mình.

---

## 💾 Công thức tính lương
Gross = (BasicSalary / StandardDays) × (WorkingDays + PaidLeaveDays) × SalaryRatio
+ OvertimePay + Bonus
BHXH_NV = 8%  |  BHYT_NV = 1.5%  |  BHTN_NV = 1%
TaxableIncome = Gross - TổngBH_NV - 11,000,000 - (DependentsCount × 4,400,000)
Thuế TNCN: Lũy tiến 7 bậc (Progressive) hoặc khấu trừ 10% vãng lai (Flat10)
Net = Gross - TổngBH_NV - ThuếTNCN - KhấuTrừKhác

---

## 🔧 Cấu hình

### Kết nối với các Service khác

Cập nhật IP thực tế của các nhóm trong `docker-compose.yml` hoặc `appsettings.json`:

```json
"HrService": {
  "BaseUrl": "https://<IP_NHOM_1>:7084"
},
"AttendanceService": {
  "BaseUrl": "https://<IP_NHOM_2>:7108"
}
```

### Cấu hình Email (gửi phiếu lương)

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": "587",
  "Username": "your-email@gmail.com",
  "Password": "your-app-password"
}
```

> Dùng Gmail cần bật **App Password**: Google Account → Security → 2-Step Verification → App Passwords.

---

## 🐛 Troubleshooting

### Lỗi 401 Unauthorized trên Swagger
Chưa gắn token. Click **Authorize 🔒** → nhập `Bearer <token>` → Authorize.

### Lỗi 500 "Unknown column"
Database chưa có cột mới. Chạy:
```bash
dotnet ef database update
```
Hoặc nếu dùng Docker, restart lại container — migration tự chạy khi khởi động.

### Lỗi container name already in use
```bash
docker rm -f payroll-service
docker-compose up --build
```

### Lỗi không kết nối được HR/Attendance Service
Kiểm tra IP trong cấu hình. Các nhóm cần cùng mạng hoặc dùng ngrok để expose service ra ngoài.

---

## 📦 Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8 + Pomelo MySQL
- MySQL 8.0
- JWT Authentication
- Docker & Docker Compose
- Swagger / OpenAPI

---

## 👨‍💻 Nhóm 3 — Payroll & Report Service

HRM Microservices System  
Học phần: Thiết kế & Kiến trúc Phần mềm  
Giảng viên: ThS. Đỗ Quang Th