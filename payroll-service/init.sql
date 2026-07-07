-- Khởi tạo database với charset UTF-8 cho tiếng Việt
CREATE DATABASE IF NOT EXISTS payroll_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE payroll_db;

-- Tạo bảng payrolls
CREATE TABLE IF NOT EXISTS payrolls (
  id INT AUTO_INCREMENT PRIMARY KEY COMMENT 'ID duy nhất',
  employee_id VARCHAR(50) NOT NULL COMMENT 'ID nhân viên',
  employee_name NVARCHAR(255) NOT NULL COMMENT 'Tên nhân viên',
  base_salary DECIMAL(12, 2) NOT NULL COMMENT 'Lương cơ bản',
  working_days INT NOT NULL DEFAULT 0 COMMENT 'Số ngày làm việc',
  leave_days INT NOT NULL DEFAULT 0 COMMENT 'Số ngày nghỉ',
  bonus DECIMAL(12, 2) NOT NULL DEFAULT 0 COMMENT 'Tiền thưởng',
  deduction DECIMAL(12, 2) NOT NULL DEFAULT 0 COMMENT 'Khoản khấu trừ',
  final_salary DECIMAL(12, 2) NOT NULL COMMENT 'Lương cuối cùng',
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Thời gian tạo',
  updated_at DATETIME NULL COMMENT 'Thời gian cập nhật',
  KEY idx_employee_id (employee_id),
  KEY idx_created_at (created_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='Bảng quản lý bảng lương';

-- Tạo index cho tìm kiếm nhanh
CREATE INDEX idx_employee_id_created ON payrolls(employee_id, created_at);
