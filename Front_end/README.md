# Ứng dụng Quản lý Nhân sự (HRM) - Frontend

Đây là ứng dụng frontend cho hệ thống Quản lý Nhân sự (HRM) theo kiến trúc Microservices. Dự án được xây dựng bằng **Vue 3**, **Vite**, và **Ant Design Vue**, mang đến giao diện người dùng hiện đại, tốc độ cao và đáp ứng tốt cho việc quản lý nhân sự, chấm công và tiền lương.

## 🚀 Tính năng nổi bật

Ứng dụng đóng vai trò là một bảng điều khiển tập trung, kết nối với nhiều backend microservices khác nhau:

- **🔐 Xác thực & Phân quyền (Authentication & Authorization)**
  - Xác thực người dùng bằng JWT.
  - Phân quyền theo vai trò (Quản trị viên - Admin, Nhân sự - HR, Quản lý - Manager, Nhân viên - Employee).

- **👥 Quản lý Nhân sự cốt lõi (HR Core)**
  - **Nhân viên:** Quản lý hồ sơ nhân viên, hỗ trợ tạo/cập nhật/xóa hàng loạt.
  - **Phòng ban:** Xem và quản lý cơ cấu tổ chức (dưới dạng cây - tree view).
  - **Hợp đồng:** Quản lý hợp đồng lao động, các loại hợp đồng và hỗ trợ ký duyệt.

- **🕒 Quản lý Chấm công (Attendance)**
  - **Check In/Out:** Theo dõi thời gian chấm công theo thời gian thực.
  - **Lịch sử & Tổng hợp:** Xem nhật ký điểm danh hàng ngày và tổng hợp chấm công hàng tháng.
  - **Nghỉ phép:** Đăng ký nghỉ phép, luồng xử lý duyệt/từ chối đơn xin nghỉ.

- **💰 Quản lý Tiền lương (Payroll)**
  - **Tính lương:** Xử lý và tính toán bảng lương hàng tháng cho nhân viên.
  - **Phiếu lương & Báo cáo:** Xuất báo cáo và xem chi tiết phiếu lương của từng cá nhân.

## 🛠️ Công nghệ sử dụng (Tech Stack)

- **Framework:** [Vue 3](https://vuejs.org/) (Composition API)
- **Công cụ Build:** [Vite](https://vitejs.dev/)
- **Quản lý State:** [Pinia](https://pinia.vuejs.org/)
- **Thư viện UI:** [Ant Design Vue](https://antdv.com/)
- **HTTP Client:** [Axios](https://axios-http.com/)
- **Xử lý ngày tháng:** [Day.js](https://day.js.org/)
- **Biểu đồ:** [Chart.js](https://www.chartjs.org/) & [vue-chartjs](https://vue-chartjs.org/)

## ⚙️ Hướng dẫn Cài đặt & Chạy dự án

### 1. Cài đặt thư viện (Dependencies)

```sh
npm install
```

### 2. Cấu hình biến môi trường (Environment Variables)

Thiết lập các URL của backend API trong file `.env.development` hoặc `.env.local`. Theo mặc định, ứng dụng sử dụng tính năng proxy của Vite cho môi trường local để tránh lỗi CORS.

Ví dụ `.env.development`:
```env
VITE_HR_URL=https://localhost:7084
VITE_ATTEND_URL=https://localhost:7108
VITE_PAYROLL_URL=https://localhost:5000
```

### 3. Chạy môi trường phát triển (Development)

Khởi động server phát triển với tính năng Hot-Module-Replacement (HMR):

```sh
npm run dev
```

### 4. Build cho môi trường Production

Biên dịch và tối ưu hóa ứng dụng để chuẩn bị deploy:

```sh
npm run build
```

Kết quả build sẽ được lưu trong thư mục `dist`.

### 5. Format Code

Định dạng lại mã nguồn bằng Prettier:

```sh
npm run format
```

## 📂 Cấu trúc thư mục (Folder Structure)

- `src/api`: Chứa cấu hình Axios cho các dịch vụ HR, Attendance, và Payroll. Đã bao gồm các interceptor để gắn token JWT và xử lý lỗi 401.
- `src/router`: Cấu hình Vue Router với các route guards để kiểm tra trạng thái đăng nhập và quyền (role) của người dùng.
- `src/stores`: Sử dụng Pinia để lưu trữ state (ví dụ: Auth state).
- `src/views`: Chứa các component giao diện trang, được phân chia theo từng tính năng (`auth`, `hr`, `attendance`, `payroll`).
- `src/layouts`: Chứa các component cấu trúc bố cục chính của ứng dụng (ví dụ: Dashboard layout).
