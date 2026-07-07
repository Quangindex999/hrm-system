# Hệ thống Quản lý Nhân sự (HRM System) - Microservices

Đây là kho lưu trữ tổng hợp (Monorepo) dành cho việc triển khai toàn bộ hệ thống HRM. Hệ thống được xây dựng theo kiến trúc Microservices, tách biệt rõ ràng các nghiệp vụ và giao tiếp với nhau qua API.

## 🏗 Kiến trúc Hệ thống

Hệ thống bao gồm 4 thành phần chính (dịch vụ):

1. **Frontend (`/Front_end`)**
   - **Tech stack:** Vue 3, Vite, Pinia, Ant Design Vue, Nginx.
   - **Chức năng:** Giao diện người dùng tập trung kết nối tới tất cả các dịch vụ backend.

2. **HR Core Service (`/HRCoreService`)**
   - **Tech stack:** .NET 8, SQL Server.
   - **Chức năng:** Quản lý nhân viên, phòng ban, hợp đồng và đảm nhiệm việc xác thực/phân quyền (Authentication & Authorization).
   - **Port:** `8081`

3. **Attendance Service (`/attendance-service`)**
   - **Tech stack:** .NET 8, SQL Server.
   - **Chức năng:** Quản lý điểm danh (Check-in/Check-out), lịch sử chấm công và tổng hợp ngày công. Đồng bộ dữ liệu nhân viên từ HR Service thông qua HTTP Polling.
   - **Port:** `8082`

4. **Payroll Service (`/payroll-service`)**
   - **Tech stack:** ASP.NET Core 8, MySQL 8.0.
   - **Chức năng:** Xử lý và tính toán bảng lương, quản lý phiếu lương, xuất báo cáo và gửi email tự động.
   - **Port:** `5000`

---

## 🚀 Hướng dẫn Cài đặt & Chạy dự án (Bằng Docker)

Toàn bộ hệ thống đã được cấu hình sẵn để chạy chung trong một file `docker-compose.yml`. Bạn không cần phải cài đặt .NET SDK, Node.js hay bất kỳ Database nào trên máy thật.

### Yêu cầu

- Đã cài đặt **Docker** và **Docker Compose** (Khuyến nghị dùng Docker Desktop trên Windows/Mac hoặc Docker Engine trên Linux).

### Các bước khởi chạy

1. **Clone mã nguồn:**

   ```bash
   git clone <đường_link_repo_của_bạn>
   cd <tên_thư_mục_repo_vừa_clone>
   ```

2. **Build và Khởi động tất cả dịch vụ:**
   Mở Terminal tại thư mục gốc của dự án và chạy:

   ```bash
   docker-compose up -d --build
   ```

   _(Lần chạy đầu tiên có thể mất vài phút để tải các base image và build code)_

3. **Truy cập ứng dụng:**
   - **Giao diện Web (Frontend):** [http://localhost](http://localhost)
   - **Swagger HR Service:** [http://localhost:8081/swagger](http://localhost:8081/swagger)
   - **Swagger Attendance Service:** [http://localhost:8082/swagger](http://localhost:8082/swagger)
   - **Swagger Payroll Service:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

### Dừng hệ thống

Để dừng ứng dụng:

```bash
docker-compose down
```

_(Nếu muốn xóa toàn bộ dữ liệu trong database, hãy thêm cờ `-v`: `docker-compose down -v`)_

---

## 🔒 Thông tin Mật khẩu Database (Mặc định)

Các container Database trong `docker-compose.yml` đang sử dụng mật khẩu mặc định cho môi trường phát triển:

- **SQL Server (dùng chung cho HR & Attendance):** `YourPassword123!`
- **MySQL 8.0 (dùng cho Payroll):** `123456789Mquan@`

> **Lưu ý triển khai (VPS):** Khi đẩy dự án này lên Production (VPS), hãy đảm bảo thay đổi các mật khẩu này và cấu hình lại chuỗi `Jwt__Secret` thông qua file `.env` để đảm bảo bảo mật.

## 📧 Cấu hình Gửi Email (Payroll Service)

Nếu bạn muốn test tính năng gửi email phiếu lương ở Payroll Service, bạn cần chỉnh sửa thông tin SMTP trong `docker-compose.yml` (hoặc file `.env` sau này):

- `Email__Username`: Địa chỉ Gmail của bạn
- `Email__Password`: Mật khẩu ứng dụng (App Password) của Gmail đó.

---

_Dự án thuộc Bài tập lớn môn Thiết kế & Kiến trúc Phần mềm._
