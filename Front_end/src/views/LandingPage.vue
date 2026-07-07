<template>
  <div class="lp">
    <!-- NAVBAR -->
    <header class="lp-nav" :class="{ scrolled: navScrolled }">
      <div class="lp-container lp-nav-inner">
        <div class="lp-logo">
          <div class="lp-logo-icon">
            <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
              <rect x="2" y="2" width="7" height="7" rx="2" fill="white" />
              <rect x="11" y="2" width="7" height="7" rx="2" fill="white" opacity=".7" />
              <rect x="2" y="11" width="7" height="7" rx="2" fill="white" opacity=".7" />
              <rect x="11" y="11" width="7" height="7" rx="2" fill="white" opacity=".4" />
            </svg>
          </div>
          <span class="lp-logo-text">HRM<span class="lp-logo-dot">.</span></span>
        </div>
        <nav class="lp-nav-links">
          <a href="#features">Tính năng</a>
          <a href="#services">Kiến trúc</a>
          <a href="#stats">Hiệu quả</a>
          <a href="#team">Nhóm</a>
        </nav>
        <router-link to="/login" class="lp-btn-nav">Đăng nhập →</router-link>
        <button class="lp-hamburger" @click="mobileOpen = !mobileOpen">
          <span></span><span></span><span></span>
        </button>
      </div>
      <div class="lp-mobile-menu" :class="{ open: mobileOpen }">
        <a href="#features" @click="mobileOpen = false">Tính năng</a>
        <a href="#services" @click="mobileOpen = false">Kiến trúc</a>
        <a href="#stats" @click="mobileOpen = false">Hiệu quả</a>
        <router-link to="/login" class="lp-btn-mobile">Đăng nhập</router-link>
      </div>
    </header>

    <!-- HERO -->
    <section class="lp-hero">
      <div class="lp-hero-bg">
        <div class="lp-blob lp-blob-1"></div>
        <div class="lp-blob lp-blob-2"></div>
        <div class="lp-grid-lines"></div>
      </div>
      <div class="lp-container lp-hero-inner">
        <div class="lp-hero-badge">
          <span class="lp-badge-dot"></span>
          Microservices Architecture · Đề tài 03
        </div>
        <h1 class="lp-hero-title">
          Quản lý nhân sự<br />
          <span class="lp-hero-accent">thông minh hơn</span>
        </h1>
        <p class="lp-hero-sub">
          Hệ thống HRM hiện đại cho doanh nghiệp vừa và nhỏ — tự động hoá chấm công, tính lương minh
          bạch và quản lý nhân viên từ một nơi duy nhất.
        </p>
        <div class="lp-hero-cta">
          <router-link to="/login" class="lp-btn-primary">
            Vào hệ thống
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
              <path
                d="M3 8h10M9 4l4 4-4 4"
                stroke="currentColor"
                stroke-width="1.8"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
          </router-link>
          <a href="#features" class="lp-btn-ghost">Xem tính năng</a>
        </div>

        <!-- Mock dashboard preview -->
        <div class="lp-hero-preview">
          <div class="lp-preview-bar">
            <span class="lp-dot red"></span>
            <span class="lp-dot yellow"></span>
            <span class="lp-dot green"></span>
            <span class="lp-preview-url">hrm.system.local/dashboard</span>
          </div>
          <div class="lp-preview-body">
            <div class="lp-preview-stats">
              <div class="lp-preview-stat" v-for="s in previewStats" :key="s.label">
                <div class="lp-preview-stat-icon" :style="{ background: s.grad }"></div>
                <div>
                  <div class="lp-preview-stat-val">{{ s.val }}</div>
                  <div class="lp-preview-stat-label">{{ s.label }}</div>
                </div>
              </div>
            </div>
            <div class="lp-preview-table">
              <div class="lp-preview-thead">
                <span>Nhân viên</span><span>Phòng ban</span><span>Trạng thái</span
                ><span>Lương</span>
              </div>
              <div class="lp-preview-row" v-for="r in previewRows" :key="r.name">
                <span class="lp-preview-emp">
                  <span class="lp-preview-avatar" :style="{ background: r.color }">{{
                    r.name[0]
                  }}</span>
                  {{ r.name }}
                </span>
                <span class="lp-preview-dept">{{ r.dept }}</span>
                <span class="lp-preview-badge" :class="r.active ? 'ok' : 'off'">{{
                  r.active ? 'Đang làm' : 'Nghỉ phép'
                }}</span>
                <span class="lp-preview-salary">{{ r.salary }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- TRUST -->
    <section class="lp-trust">
      <div class="lp-container">
        <p class="lp-trust-label">Xây dựng trên nền tảng công nghệ hiện đại</p>
        <div class="lp-trust-marquee-wrap">
          <div class="lp-trust-marquee">
            <div class="lp-trust-track" :class="{ paused: marqueePaused }">
              <div
                class="lp-trust-logo"
                v-for="logo in [...techLogos, ...techLogos]"
                :key="logo.name + Math.random()"
                @mouseenter="marqueePaused = true"
                @mouseleave="marqueePaused = false"
                :style="{ '--brand-color': logo.color, '--brand-bg': logo.bg }"
              >
                <component :is="logo.component" class="lp-logo-icon" />
                <span class="lp-logo-text">{{ logo.icon }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- FEATURES -->
    <section class="lp-section" id="features">
      <div class="lp-container">
        <div class="lp-section-header">
          <span class="lp-eyebrow">Tính năng</span>
          <h2 class="lp-section-title">Mọi thứ bạn cần<br />trong một hệ thống</h2>
          <p class="lp-section-sub">
            Ba module độc lập, phối hợp nhịp nhàng — nhân sự, chấm công và tính lương vận hành song
            song không gián đoạn.
          </p>
        </div>
        <div class="lp-features-grid">
          <div class="lp-feature-card" v-for="f in features" :key="f.title">
            <div class="lp-feature-icon" :style="{ background: f.grad }">
              <component :is="f.icon" style="font-size: 22px; color: #fff" />
            </div>
            <h3 class="lp-feature-title">{{ f.title }}</h3>
            <p class="lp-feature-desc">{{ f.desc }}</p>
            <ul class="lp-feature-list">
              <li v-for="item in f.items" :key="item">
                <svg width="14" height="14" viewBox="0 0 14 14" fill="none">
                  <circle cx="7" cy="7" r="7" fill="#00b14f" opacity=".15" />
                  <path
                    d="M4 7l2 2 4-4"
                    stroke="#00b14f"
                    stroke-width="1.5"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                </svg>
                {{ item }}
              </li>
            </ul>
          </div>
        </div>
      </div>
    </section>

    <!-- SERVICES -->
    <section class="lp-section lp-section-dark" id="services">
      <div class="lp-container">
        <div class="lp-section-header">
          <span class="lp-eyebrow light">Kiến trúc</span>
          <h2 class="lp-section-title light">Ba service, một hệ thống</h2>
          <p class="lp-section-sub light">
            Mỗi service hoạt động độc lập trong Docker container riêng, giao tiếp qua API Gateway.
          </p>
        </div>

        <!-- Architecture diagram -->
        <div class="lp-arch">
          <div class="lp-arch-box client">
            <div class="lp-arch-box-icon">
              <MonitorOutlined style="font-size: 18px; color: #fff" />
            </div>
            <span>Vue.js Frontend</span>
            <small>:3000</small>
          </div>
          <div class="lp-arch-arrow">
            <div class="lp-arch-line-h"></div>
            <span>HTTPS + JWT</span>
          </div>
          <div class="lp-arch-box gateway">
            <div class="lp-arch-box-icon"><ApiOutlined style="font-size: 18px; color: #fff" /></div>
            <span>API Gateway</span>
            <small>Auth · Routing</small>
          </div>
          <div class="lp-arch-arrow">
            <div class="lp-arch-line-h"></div>
            <span>Forward</span>
          </div>
          <div class="lp-arch-svc-row">
            <div class="lp-arch-box svc" v-for="svc in services" :key="svc.name">
              <div class="lp-arch-box-icon">
                <component :is="svc.icon" style="font-size: 16px; color: #fff" />
              </div>
              <span>{{ svc.shortName }}</span>
              <small>{{ svc.port }}</small>
            </div>
          </div>
        </div>

        <!-- Service detail cards -->
        <div class="lp-svc-cards">
          <div class="lp-svc-card" v-for="(svc, i) in services" :key="svc.name">
            <div class="lp-svc-num">{{ String(i + 1).padStart(2, '0') }}</div>
            <div class="lp-svc-icon" :style="{ background: svc.grad }">
              <component :is="svc.icon" style="font-size: 20px; color: #fff" />
            </div>
            <h3>{{ svc.name }}</h3>
            <p>{{ svc.desc }}</p>
            <div class="lp-svc-port">
              <code>{{ svc.port }}</code>
              <span class="lp-online-dot"></span>
              <span>Online</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- STATS -->
    <section class="lp-section" id="stats">
      <div class="lp-container">
        <div class="lp-section-header">
          <span class="lp-eyebrow">Hiệu quả</span>
          <h2 class="lp-section-title">Con số nói lên tất cả</h2>
        </div>
        <div class="lp-stats-grid">
          <div class="lp-stat-item" v-for="s in stats" :key="s.label">
            <div class="lp-stat-val">
              <span class="lp-stat-num">{{ s.display }}</span>
              <span class="lp-stat-unit">{{ s.unit }}</span>
            </div>
            <div class="lp-stat-label">{{ s.label }}</div>
            <div class="lp-stat-bar">
              <div
                class="lp-stat-bar-fill"
                :style="{ width: s.pct + '%', background: s.color }"
              ></div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- HOW IT WORKS -->
    <section class="lp-section lp-section-surface">
      <div class="lp-container">
        <div class="lp-section-header">
          <span class="lp-eyebrow">Quy trình</span>
          <h2 class="lp-section-title">Bắt đầu trong 3 bước</h2>
        </div>
        <div class="lp-steps">
          <div class="lp-step" v-for="(step, i) in steps" :key="step.title">
            <div class="lp-step-num">{{ i + 1 }}</div>
            <div class="lp-step-content">
              <h3>{{ step.title }}</h3>
              <p>{{ step.desc }}</p>
            </div>
          </div>
          <div class="lp-step-connector connector-1"></div>
          <div class="lp-step-connector connector-2"></div>
        </div>
      </div>
    </section>

    <!-- TEAM -->
    <section class="lp-section" id="team">
      <div class="lp-container">
        <div class="lp-section-header">
          <span class="lp-eyebrow">Nhóm phát triển</span>
          <h2 class="lp-section-title">Đội ngũ xây dựng hệ thống</h2>
          <p class="lp-section-sub">Nhóm A4 · FIT4110 · Hệ thống quản lý nhân sự & chấm công</p>
        </div>
        <div class="lp-team-grid">
          <div class="lp-team-card" v-for="member in team" :key="member.name">
            <div class="lp-team-avatar" :style="{ background: member.color }">
              {{ member.initial }}
            </div>
            <div class="lp-team-name">{{ member.name }}</div>
            <div class="lp-team-role">{{ member.role }}</div>
            <span class="lp-team-badge">{{ member.service }}</span>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="lp-cta-section">
      <div class="lp-cta-bg">
        <div class="lp-blob lp-blob-cta1"></div>
        <div class="lp-blob lp-blob-cta2"></div>
      </div>
      <div class="lp-container lp-cta-inner">
        <h2 class="lp-cta-title">Sẵn sàng dùng thử?</h2>
        <p class="lp-cta-sub">Đăng nhập ngay để khám phá toàn bộ hệ thống HRM</p>
        <router-link to="/login" class="lp-btn-primary large">
          Vào hệ thống ngay
          <svg width="18" height="18" viewBox="0 0 16 16" fill="none">
            <path
              d="M3 8h10M9 4l4 4-4 4"
              stroke="currentColor"
              stroke-width="1.8"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </router-link>
        <div class="lp-cta-meta">
          <span>✓ Không cần cài đặt</span>
          <span>✓ Dữ liệu demo có sẵn</span>
          <span>✓ Miễn phí sử dụng</span>
        </div>
      </div>
    </section>

    <!-- FOOTER -->
    <footer class="lp-footer">
      <div class="lp-container lp-footer-inner">
        <div class="lp-footer-brand">
          <div style="display: flex; align-items: center; gap: 8px; margin-bottom: 12px">
            <div class="lp-logo-icon small">
              <svg width="14" height="14" viewBox="0 0 20 20" fill="none">
                <rect x="2" y="2" width="7" height="7" rx="2" fill="white" />
                <rect x="11" y="2" width="7" height="7" rx="2" fill="white" opacity=".7" />
                <rect x="2" y="11" width="7" height="7" rx="2" fill="white" opacity=".7" />
                <rect x="11" y="11" width="7" height="7" rx="2" fill="white" opacity=".4" />
              </svg>
            </div>
            <span
              style="
                font-family: var(--font-h);
                font-size: 16px;
                font-weight: 700;
                color: rgba(255, 255, 255, 0.9);
              "
              >HRM<span style="color: var(--green)">.</span></span
            >
          </div>
          <p>Hệ thống quản lý nhân sự microservices<br />dành cho doanh nghiệp vừa và nhỏ.</p>
        </div>
        <div class="lp-footer-links">
          <div class="lp-footer-col">
            <div class="lp-footer-col-title">Hệ thống</div>
            <router-link to="/login">Đăng nhập</router-link>
            <router-link to="/dashboard">Dashboard</router-link>
            <router-link to="/hr/employees">Nhân viên</router-link>
          </div>
          <div class="lp-footer-col">
            <div class="lp-footer-col-title">Module</div>
            <a href="#features">HR Core</a>
            <a href="#features">Chấm công</a>
            <a href="#features">Tính lương</a>
          </div>
          <div class="lp-footer-col">
            <div class="lp-footer-col-title">Công nghệ</div>
            <span>ASP.NET Core 8</span>
            <span>Vue.js 3</span>
            <span>Docker</span>
          </div>
        </div>
      </div>
      <div class="lp-footer-bottom">
        <div class="lp-container">© 2024 HRM System · Đề tài 03 · Nhóm A4 · FIT4110</div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import {
  UserOutlined,
  ClockCircleOutlined,
  DollarOutlined,
  FileTextOutlined,
  ApiOutlined,
  MonitorOutlined,
  SafetyOutlined,
  ThunderboltOutlined,
  CodeOutlined,
  BranchesOutlined,
  ContainerOutlined,
  DatabaseOutlined,
  GlobalOutlined,
  LockOutlined,
} from '@ant-design/icons-vue'

const navScrolled = ref(false)
const mobileOpen = ref(false)
const marqueePaused = ref(false)

function onScroll() {
  navScrolled.value = window.scrollY > 40
}
onMounted(() => window.addEventListener('scroll', onScroll))
onUnmounted(() => window.removeEventListener('scroll', onScroll))

const previewStats = [
  { label: 'Nhân viên', val: '128', grad: 'linear-gradient(135deg,#00b14f,#00d46a)' },
  { label: 'Chấm công hôm nay', val: '96', grad: 'linear-gradient(135deg,#00b4d8,#7c5cfc)' },
  { label: 'Quỹ lương tháng', val: '2.4T', grad: 'linear-gradient(135deg,#ffb020,#ff8c00)' },
  { label: 'Đơn nghỉ chờ duyệt', val: '7', grad: 'linear-gradient(135deg,#ff4757,#ff6b81)' },
]
const previewRows = [
  { name: 'Nguyễn Văn A', dept: 'Kỹ thuật', active: true, salary: '18.000.000₫', color: '#00b14f' },
  { name: 'Trần Thị B', dept: 'Kế toán', active: true, salary: '15.500.000₫', color: '#00b4d8' },
  { name: 'Lê Văn C', dept: 'Kinh doanh', active: false, salary: '16.200.000₫', color: '#7c5cfc' },
]

const techLogos = [
  {
    name: 'Vue.js',
    color: '#42b883',
    bg: 'rgba(66,184,131,0.08)',
    icon: 'Vue.js',
    component: CodeOutlined,
  },
  {
    name: 'ASP.NET',
    color: '#512bd4',
    bg: 'rgba(81,43,212,0.08)',
    icon: '.NET 8',
    component: BranchesOutlined,
  },
  {
    name: 'Docker',
    color: '#2496ed',
    bg: 'rgba(36,150,237,0.08)',
    icon: 'Docker',
    component: ContainerOutlined,
  },
  {
    name: 'SQL Server',
    color: '#cc2927',
    bg: 'rgba(204,41,39,0.08)',
    icon: 'SQL Server',
    component: DatabaseOutlined,
  },
  {
    name: 'Ant Design',
    color: '#1890ff',
    bg: 'rgba(24,144,255,0.08)',
    icon: 'Ant Design',
    component: GlobalOutlined,
  },
  {
    name: 'Swagger',
    color: '#85ea2d',
    bg: 'rgba(133,234,45,0.08)',
    icon: 'Swagger',
    component: ApiOutlined,
  },
  {
    name: 'Pinia',
    color: '#eab308',
    bg: 'rgba(234,179,8,0.08)',
    icon: 'Pinia',
    component: LockOutlined,
  },
  {
    name: 'JWT',
    color: '#e94f37',
    bg: 'rgba(233,79,55,0.08)',
    icon: 'JWT',
    component: SafetyOutlined,
  },
]

const features = [
  {
    title: 'Quản lý Nhân viên & Phòng ban',
    desc: 'Hồ sơ nhân viên đầy đủ, cơ cấu tổ chức dạng cây, phân quyền theo vai trò.',
    icon: UserOutlined,
    grad: 'linear-gradient(135deg,#00b14f,#00d46a)',
    items: [
      'Hồ sơ nhân viên chi tiết',
      'Sơ đồ tổ chức dạng cây',
      'Phân quyền Admin / Manager / NV',
      'CRUD đầy đủ với validation',
    ],
  },
  {
    title: 'Chấm công & Nghỉ phép',
    desc: 'Check-in/out realtime, lịch sử chấm công, đăng ký và duyệt nghỉ phép online.',
    icon: ClockCircleOutlined,
    grad: 'linear-gradient(135deg,#00b4d8,#7c5cfc)',
    items: [
      'Check-in/out tức thì',
      'Lịch sử và tổng hợp tháng',
      'Đăng ký nghỉ phép online',
      'Duyệt/từ chối theo cấp quản lý',
    ],
  },
  {
    title: 'Tính lương & Báo cáo',
    desc: 'Tự động tính lương cuối tháng, xuất báo cáo chi tiết, biểu đồ phân tích.',
    icon: DollarOutlined,
    grad: 'linear-gradient(135deg,#ffb020,#ff8c00)',
    items: [
      'Tính lương tự động theo batch',
      'Báo cáo chi tiết từng nhân viên',
      'Biểu đồ phân bổ theo phòng ban',
      'Xuất CSV trực tiếp',
    ],
  },
  {
    title: 'Bảo mật JWT tập trung',
    desc: 'Một service duy nhất cấp token, các service còn lại validate bằng shared secret.',
    icon: SafetyOutlined,
    grad: 'linear-gradient(135deg,#1a2332,#2d3f55)',
    items: ['JWT Bearer Token', 'Shared secret key', 'Role-based access control'],
  },
  {
    title: 'API Gateway',
    desc: 'Điểm vào duy nhất cho toàn hệ thống, rate limiting và smart routing.',
    icon: ApiOutlined,
    grad: 'linear-gradient(135deg,#7c5cfc,#00b4d8)',
    items: ['Single entry point', 'Rate limiting', 'JWT validation trước forward'],
  },
  {
    title: 'Docker Ready',
    desc: 'Mỗi service chạy trong container độc lập, dễ dàng deploy và scale.',
    icon: ThunderboltOutlined,
    grad: 'linear-gradient(135deg,#ff4757,#ff6b81)',
    items: ['Container độc lập', 'docker-compose toàn hệ thống', 'Swagger UI riêng mỗi service'],
  },
]

const services = [
  {
    name: 'HR Core Service',
    shortName: 'HR Core',
    port: 'localhost:7084',
    db: 'HRCoreDB',
    desc: 'Quản lý hồ sơ nhân viên, phòng ban, chức vụ và cơ cấu tổ chức. Đây cũng là service phát hành JWT token cho toàn hệ thống.',
    icon: UserOutlined,
    grad: 'linear-gradient(135deg,#00b14f,#00d46a)',
  },
  {
    name: 'Attendance Service',
    shortName: 'Attendance',
    port: 'localhost:7108',
    db: 'AttendanceDB',
    desc: 'Xử lý chấm công hàng ngày, ca làm việc, đăng ký nghỉ phép và luồng duyệt theo cấp quản lý.',
    icon: ClockCircleOutlined,
    grad: 'linear-gradient(135deg,#00b4d8,#7c5cfc)',
  },
  {
    name: 'Payroll Service',
    shortName: 'Payroll',
    port: 'localhost:5000',
    db: 'PayrollDB',
    desc: 'Cấu hình lương, tự động tính lương cuối tháng theo batch và xuất báo cáo chi phí nhân sự.',
    icon: DollarOutlined,
    grad: 'linear-gradient(135deg,#ffb020,#ff8c00)',
  },
]

const stats = [
  { display: '98', unit: '%', label: 'Uptime hệ thống', pct: 98, color: '#00b14f' },
  { display: '3', unit: 'ms', label: 'Latency trung bình API', pct: 30, color: '#00b4d8' },
  { display: '500', unit: '+', label: 'Nhân viên có thể quản lý', pct: 75, color: '#7c5cfc' },
  { display: '100', unit: '%', label: 'API được document Swagger', pct: 100, color: '#ffb020' },
]

const steps = [
  {
    title: 'Đăng nhập',
    desc: 'Dùng tài khoản admin/123456. JWT token được cấp tự động bởi HR Core Service.',
  },
  {
    title: 'Thiết lập nhân sự',
    desc: 'Tạo phòng ban, thêm nhân viên và phân quyền theo vai trò Admin / Manager / NV.',
  },
  {
    title: 'Vận hành hàng ngày',
    desc: 'Chấm công, duyệt nghỉ phép và tính lương cuối tháng hoàn toàn tự động.',
  },
]

const team = [
  {
    name: 'Nguyễn Văn A',
    initial: 'A',
    role: 'Team Lead · Backend',
    service: 'HR Core Service',
    color: '#00b14f',
  },
  {
    name: 'Trần Thị B',
    initial: 'B',
    role: 'Backend Developer',
    service: 'HR Core Service',
    color: '#009944',
  },
  {
    name: 'Lê Văn C',
    initial: 'C',
    role: 'Backend Developer',
    service: 'Attendance Service',
    color: '#00b4d8',
  },
  {
    name: 'Phạm Thị D',
    initial: 'D',
    role: 'Backend Developer',
    service: 'Attendance Service',
    color: '#0099b8',
  },
  {
    name: 'Hoàng Văn E',
    initial: 'E',
    role: 'Backend Developer',
    service: 'Payroll Service',
    color: '#7c5cfc',
  },
  {
    name: 'Vũ Thị F',
    initial: 'F',
    role: 'Backend · Frontend',
    service: 'Payroll Service',
    color: '#6344e0',
  },
]
</script>

<style scoped>
.lp {
  --green: #00b14f;
  --green-d: #009944;
  --blue: #00b4d8;
  --purple: #7c5cfc;
  --orange: #ffb020;
  --red: #ff4757;
  --dark: #0d1b2a;
  --dark2: #1a2332;
  --surface: #f7f9fc;
  --border: #e8ecf2;
  --text: #0d1b2a;
  --muted: #5a6578;
  --faint: #8892a0;
  --font-h: 'Plus Jakarta Sans', sans-serif;
  --font-b: 'DM Sans', sans-serif;
  --ease: cubic-bezier(0.4, 0, 0.2, 1);
  font-family: var(--font-b);
  color: var(--text);
  overflow-x: hidden;
}
.lp-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 32px;
}

/* NAV */
.lp-nav {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 200;
  transition:
    background 0.3s var(--ease),
    box-shadow 0.3s var(--ease);
}
.lp-nav.scrolled {
  background: rgba(255, 255, 255, 0.93);
  backdrop-filter: blur(12px);
  box-shadow:
    0 1px 0 var(--border),
    0 4px 16px rgba(13, 27, 42, 0.06);
}
.lp-nav-inner {
  display: flex;
  align-items: center;
  height: 68px;
  gap: 32px;
}
.lp-logo {
  display: flex;
  align-items: center;
  gap: 8px;
  text-decoration: none;
}
.lp-logo-icon {
  width: 34px;
  height: 34px;
  border-radius: 10px;
  background: linear-gradient(135deg, var(--green), #00d46a);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  box-shadow: 0 4px 12px rgba(0, 177, 79, 0.3);
}
.lp-logo-icon.small {
  width: 28px;
  height: 28px;
  border-radius: 8px;
}
.lp-logo-text {
  font-family: var(--font-h);
  font-size: 18px;
  font-weight: 700;
  color: var(--dark);
}
.lp-logo-dot {
  color: var(--green);
}
.lp-nav-links {
  display: flex;
  align-items: center;
  gap: 28px;
  margin-left: 16px;
  flex: 1;
}
.lp-nav-links a {
  font-size: 14px;
  font-weight: 500;
  color: var(--muted);
  text-decoration: none;
  transition: color 0.2s;
}
.lp-nav-links a:hover {
  color: var(--green);
}
.lp-btn-nav {
  padding: 8px 20px;
  background: var(--green);
  color: #fff;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  text-decoration: none;
  transition:
    background 0.2s,
    box-shadow 0.2s,
    transform 0.15s;
}
.lp-btn-nav:hover {
  background: var(--green-d);
  box-shadow: 0 6px 20px rgba(0, 177, 79, 0.35);
  transform: translateY(-1px);
}
.lp-hamburger {
  display: none;
  flex-direction: column;
  gap: 5px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
  margin-left: auto;
}
.lp-hamburger span {
  display: block;
  width: 22px;
  height: 2px;
  background: var(--dark);
  border-radius: 2px;
}
.lp-mobile-menu {
  display: none;
  flex-direction: column;
  padding: 0 20px;
  background: #fff;
  border-top: 1px solid var(--border);
  max-height: 0;
  overflow: hidden;
  transition:
    max-height 0.3s,
    padding 0.3s;
}
.lp-mobile-menu.open {
  max-height: 300px;
  padding: 12px 20px 16px;
}
.lp-mobile-menu a {
  padding: 10px 0;
  font-size: 15px;
  color: var(--dark);
  text-decoration: none;
  border-bottom: 1px solid var(--border);
  display: block;
}
.lp-btn-mobile {
  margin-top: 8px;
  padding: 12px;
  background: var(--green);
  color: #fff;
  text-align: center;
  border-radius: 8px;
  font-weight: 600;
  text-decoration: none;
  display: block;
  border: none;
}

/* HERO */
.lp-hero {
  position: relative;
  min-height: 100vh;
  display: flex;
  align-items: center;
  padding: 120px 0 80px;
  overflow: hidden;
  background: linear-gradient(160deg, #ffffff 0%, #f0f7f3 60%, #e8f4ec 100%);
}
.lp-hero-bg {
  position: absolute;
  inset: 0;
  pointer-events: none;
}
.lp-blob {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.35;
  animation: blobFloat 8s ease-in-out infinite;
}
.lp-blob-1 {
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(0, 177, 79, 0.3), transparent);
  top: -100px;
  right: -100px;
}
.lp-blob-2 {
  width: 350px;
  height: 350px;
  background: radial-gradient(circle, rgba(0, 180, 216, 0.2), transparent);
  bottom: 50px;
  left: -50px;
  animation-delay: 3s;
}
.lp-blob-cta1 {
  width: 400px;
  height: 400px;
  background: radial-gradient(circle, rgba(0, 177, 79, 0.25), transparent);
  top: -80px;
  left: 0;
}
.lp-blob-cta2 {
  width: 300px;
  height: 300px;
  background: radial-gradient(circle, rgba(0, 180, 216, 0.2), transparent);
  bottom: -50px;
  right: 0;
  animation-delay: 2s;
}
@keyframes blobFloat {
  0%,
  100% {
    transform: translate(0, 0) scale(1);
  }
  33% {
    transform: translate(20px, -15px) scale(1.05);
  }
  66% {
    transform: translate(-10px, 10px) scale(0.97);
  }
}
.lp-grid-lines {
  position: absolute;
  inset: 0;
  background-image:
    linear-gradient(rgba(0, 177, 79, 0.05) 1px, transparent 1px),
    linear-gradient(90deg, rgba(0, 177, 79, 0.05) 1px, transparent 1px);
  background-size: 60px 60px;
}
.lp-hero-inner {
  position: relative;
  z-index: 1;
  text-align: center;
  width: 100%;
}
.lp-hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 16px;
  border-radius: 9999px;
  background: rgba(0, 177, 79, 0.08);
  border: 1px solid rgba(0, 177, 79, 0.2);
  font-size: 12px;
  font-weight: 600;
  color: var(--green);
  margin-bottom: 24px;
  letter-spacing: 0.02em;
  animation: fadeDown 0.6s var(--ease) both;
}
.lp-badge-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: var(--green);
  animation: pulse 2s infinite;
}
@keyframes pulse {
  0%,
  100% {
    box-shadow: 0 0 0 0 rgba(0, 177, 79, 0.4);
  }
  50% {
    box-shadow: 0 0 0 6px rgba(0, 177, 79, 0);
  }
}
@keyframes fadeDown {
  from {
    opacity: 0;
    transform: translateY(-12px);
  }
  to {
    opacity: 1;
    transform: none;
  }
}
.lp-hero-title {
  font-family: var(--font-h);
  font-size: clamp(38px, 5.5vw, 68px);
  font-weight: 700;
  line-height: 1.1;
  letter-spacing: -1px;
  color: var(--dark);
  margin: 0 0 20px;
  animation: fadeDown 0.6s 0.1s var(--ease) both;
}
.lp-hero-accent {
  background: linear-gradient(135deg, var(--green), #00d46a);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}
.lp-hero-sub {
  font-size: clamp(16px, 2vw, 18px);
  color: var(--muted);
  line-height: 1.65;
  max-width: 560px;
  margin: 0 auto 36px;
  animation: fadeDown 0.6s 0.2s var(--ease) both;
}
.lp-hero-cta {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  margin-bottom: 56px;
  animation: fadeDown 0.6s 0.3s var(--ease) both;
}
.lp-btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 14px 28px;
  background: var(--green);
  color: #fff;
  border-radius: 10px;
  font-size: 15px;
  font-weight: 600;
  text-decoration: none;
  transition:
    background 0.2s,
    box-shadow 0.2s,
    transform 0.15s;
}
.lp-btn-primary:hover {
  background: var(--green-d);
  box-shadow: 0 8px 28px rgba(0, 177, 79, 0.4);
  transform: translateY(-2px);
}
.lp-btn-primary.large {
  padding: 16px 36px;
  font-size: 17px;
  border-radius: 12px;
}
.lp-btn-ghost {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 14px 24px;
  color: var(--dark);
  border: 1px solid var(--border);
  border-radius: 10px;
  font-size: 15px;
  font-weight: 500;
  text-decoration: none;
  background: rgba(255, 255, 255, 0.8);
  transition:
    border-color 0.2s,
    background 0.2s;
}
.lp-btn-ghost:hover {
  border-color: var(--green);
  background: #fff;
}

/* preview */
.lp-hero-preview {
  max-width: 820px;
  margin: 0 auto;
  border-radius: 14px;
  overflow: hidden;
  box-shadow:
    0 24px 80px rgba(13, 27, 42, 0.18),
    0 4px 16px rgba(0, 0, 0, 0.08);
  border: 1px solid var(--border);
  animation: fadeDown 0.7s 0.4s var(--ease) both;
}
.lp-preview-bar {
  background: #f0f0f0;
  padding: 10px 16px;
  display: flex;
  align-items: center;
  gap: 6px;
}
.lp-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
}
.lp-dot.red {
  background: #ff5f57;
}
.lp-dot.yellow {
  background: #febc2e;
}
.lp-dot.green {
  background: #28c840;
}
.lp-preview-url {
  margin-left: 8px;
  font-size: 12px;
  color: #888;
  background: #e0e0e0;
  border-radius: 4px;
  padding: 2px 10px;
  font-family: monospace;
}
.lp-preview-body {
  background: var(--surface);
  padding: 16px;
}
.lp-preview-stats {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 10px;
  margin-bottom: 14px;
}
.lp-preview-stat {
  background: #fff;
  border-radius: 8px;
  padding: 10px 12px;
  border: 1px solid var(--border);
  display: flex;
  align-items: center;
  gap: 8px;
}
.lp-preview-stat-icon {
  width: 28px;
  height: 28px;
  border-radius: 7px;
  flex-shrink: 0;
}
.lp-preview-stat-val {
  font-family: var(--font-h);
  font-size: 14px;
  font-weight: 700;
  color: var(--dark);
}
.lp-preview-stat-label {
  font-size: 10px;
  color: var(--faint);
}
.lp-preview-table {
  background: #fff;
  border-radius: 8px;
  border: 1px solid var(--border);
  overflow: hidden;
}
.lp-preview-thead {
  display: grid;
  grid-template-columns: 2fr 1.5fr 1fr 1.5fr;
  padding: 8px 12px;
  background: var(--surface);
  font-size: 10px;
  font-weight: 700;
  color: var(--faint);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}
.lp-preview-row {
  display: grid;
  grid-template-columns: 2fr 1.5fr 1fr 1.5fr;
  padding: 9px 12px;
  align-items: center;
  border-top: 1px solid var(--border);
  font-size: 12px;
}
.lp-preview-emp {
  display: flex;
  align-items: center;
  gap: 7px;
  font-weight: 500;
}
.lp-preview-avatar {
  width: 20px;
  height: 20px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 9px;
  font-weight: 700;
  color: #fff;
  flex-shrink: 0;
}
.lp-preview-dept {
  color: var(--muted);
  font-size: 11px;
}
.lp-preview-badge {
  font-size: 10px;
  font-weight: 600;
  padding: 2px 7px;
  border-radius: 4px;
  width: fit-content;
}
.lp-preview-badge.ok {
  background: rgba(0, 177, 79, 0.1);
  color: var(--green);
}
.lp-preview-badge.off {
  background: rgba(255, 183, 32, 0.12);
  color: #b07800;
}
.lp-preview-salary {
  font-weight: 600;
  font-size: 11px;
  color: var(--green);
}

/* TRUST */
.lp-trust {
  padding: 56px 0 64px;
  border-top: 1px solid var(--border);
  border-bottom: 1px solid var(--border);
  background: #fff;
  overflow: hidden;
}
.lp-trust-label {
  text-align: center;
  font-size: 11px;
  color: var(--faint);
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  margin-bottom: 36px;
}
.lp-trust-marquee-wrap {
  position: relative;
}
.lp-trust-marquee {
  overflow: hidden;
  mask-image: linear-gradient(to right, transparent 0%, black 10%, black 90%, transparent 100%);
  -webkit-mask-image: linear-gradient(
    to right,
    transparent 0%,
    black 10%,
    black 90%,
    transparent 100%
  );
}
.lp-trust-track {
  display: flex;
  gap: 14px;
  width: max-content;
  animation: marqueeScroll 28s linear infinite;
}
.lp-trust-track.paused {
  animation-play-state: paused;
}
.lp-trust-logo {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 14px 24px;
  border-radius: 12px;
  background: var(--brand-bg);
  border: 1px solid transparent;
  cursor: default;
  user-select: none;
  flex-shrink: 0;
  transition:
    border-color 0.2s,
    box-shadow 0.2s,
    transform 0.2s;
}
.lp-trust-logo:hover {
  border-color: var(--brand-color);
  box-shadow: 0 0 0 3px color-mix(in srgb, var(--brand-color) 12%, transparent);
  transform: translateY(-1px);
}
.lp-trust-logo .lp-logo-icon,
.lp-trust-logo .lp-logo-icon.anticon,
.lp-trust-logo .anticon {
  background: transparent !important;
}
.lp-logo-icon {
  font-size: 22px;
  color: var(--brand-color);
  flex-shrink: 0;
  line-height: 1;
}
.lp-logo-text {
  font-size: 16px;
  font-weight: 700;
  color: var(--brand-color);
  letter-spacing: 0.02em;
  white-space: nowrap;
}
@keyframes marqueeScroll {
  from {
    transform: translateX(0);
  }
  to {
    transform: translateX(-50%);
  }
}

/* SECTIONS */
.lp-section {
  padding: 96px 0;
  background: #fff;
}
.lp-section-surface {
  background: var(--surface);
}
.lp-section-dark {
  background: var(--dark2);
}
.lp-section-header {
  text-align: center;
  margin-bottom: 60px;
}
.lp-eyebrow {
  display: inline-block;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--green);
  margin-bottom: 14px;
}
.lp-eyebrow.light {
  color: rgba(0, 177, 79, 0.8);
}
.lp-section-title {
  font-family: var(--font-h);
  font-size: clamp(28px, 4vw, 44px);
  font-weight: 700;
  line-height: 1.15;
  letter-spacing: -0.5px;
  color: var(--dark);
  margin: 0 0 16px;
}
.lp-section-title.light {
  color: #fff;
}
.lp-section-sub {
  font-size: 17px;
  color: var(--muted);
  max-width: 560px;
  margin: 0 auto;
  line-height: 1.6;
}
.lp-section-sub.light {
  color: rgba(255, 255, 255, 0.5);
}

/* FEATURES */
.lp-features-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}
.lp-feature-card {
  background: #fff;
  border: 1px solid var(--border);
  border-radius: 16px;
  padding: 28px;
  box-shadow: 0 2px 12px rgba(13, 27, 42, 0.05);
  transition:
    box-shadow 0.25s,
    transform 0.25s;
}
.lp-feature-card:hover {
  box-shadow: 0 10px 36px rgba(13, 27, 42, 0.1);
  transform: translateY(-3px);
}
.lp-feature-icon {
  width: 48px;
  height: 48px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 16px;
}
.lp-feature-title {
  font-family: var(--font-h);
  font-size: 16px;
  font-weight: 700;
  color: var(--dark);
  margin: 0 0 8px;
}
.lp-feature-desc {
  font-size: 14px;
  color: var(--muted);
  line-height: 1.55;
  margin: 0 0 16px;
}
.lp-feature-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 7px;
}
.lp-feature-list li {
  display: flex;
  align-items: flex-start;
  gap: 8px;
  font-size: 13px;
  color: var(--muted);
}

/* ARCHITECTURE */
.lp-arch {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  margin-bottom: 48px;
  flex-wrap: wrap;
}
.lp-arch-box {
  background: rgba(255, 255, 255, 0.06);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 12px;
  padding: 14px 18px;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
  color: #fff;
  text-align: center;
  min-width: 120px;
}
.lp-arch-box.gateway {
  border-color: rgba(0, 177, 79, 0.5);
  background: rgba(0, 177, 79, 0.1);
}
.lp-arch-box span {
  font-size: 12px;
  font-weight: 600;
}
.lp-arch-box small {
  font-size: 10px;
  color: rgba(255, 255, 255, 0.4);
  font-family: monospace;
}
.lp-arch-box-icon {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 4px;
}
.lp-arch-arrow {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
}
.lp-arch-line-h {
  width: 32px;
  height: 2px;
  background: rgba(0, 177, 79, 0.5);
}
.lp-arch-arrow span {
  font-size: 9px;
  color: rgba(0, 177, 79, 0.7);
  font-weight: 600;
}
.lp-arch-svc-row {
  display: flex;
  gap: 8px;
}
.lp-online-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--green);
  box-shadow: 0 0 6px rgba(0, 177, 79, 0.6);
  animation: pulse 2s infinite;
  flex-shrink: 0;
}

/* SVC CARDS */
.lp-svc-cards {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}
.lp-svc-card {
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 16px;
  padding: 28px;
  position: relative;
  overflow: hidden;
  transition:
    border-color 0.25s,
    transform 0.25s;
}
.lp-svc-card:hover {
  border-color: rgba(0, 177, 79, 0.35);
  transform: translateY(-3px);
}
.lp-svc-num {
  position: absolute;
  top: 16px;
  right: 20px;
  font-family: var(--font-h);
  font-size: 40px;
  font-weight: 700;
  color: rgba(255, 255, 255, 0.04);
  line-height: 1;
}
.lp-svc-icon {
  width: 44px;
  height: 44px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 16px;
}
.lp-svc-card h3 {
  font-family: var(--font-h);
  font-size: 16px;
  font-weight: 700;
  color: #fff;
  margin: 0 0 8px;
}
.lp-svc-card p {
  font-size: 13px;
  color: rgba(255, 255, 255, 0.45);
  line-height: 1.6;
  margin: 0 0 16px;
}
.lp-svc-port {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 12px;
}
.lp-svc-port code {
  background: rgba(255, 255, 255, 0.08);
  color: rgba(255, 255, 255, 0.65);
  padding: 3px 8px;
  border-radius: 5px;
  font-size: 11px;
}
.lp-svc-port span {
  color: rgba(255, 255, 255, 0.35);
  font-size: 12px;
}

/* STATS */
.lp-stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}
.lp-stat-item {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 14px;
  padding: 28px 24px;
}
.lp-stat-val {
  display: flex;
  align-items: baseline;
  gap: 2px;
  margin-bottom: 6px;
}
.lp-stat-num {
  font-family: var(--font-h);
  font-size: 44px;
  font-weight: 700;
  color: var(--dark);
  line-height: 1;
}
.lp-stat-unit {
  font-family: var(--font-h);
  font-size: 22px;
  font-weight: 700;
  color: var(--green);
}
.lp-stat-label {
  font-size: 14px;
  color: var(--muted);
  margin-bottom: 16px;
  line-height: 1.4;
}
.lp-stat-bar {
  height: 4px;
  background: var(--border);
  border-radius: 2px;
  overflow: hidden;
}
.lp-stat-bar-fill {
  height: 100%;
  border-radius: 2px;
}

/* STEPS */
.lp-steps {
  display: flex;
  align-items: flex-start;
  gap: 0;
  position: relative;
}
.lp-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  flex: 1;
  padding: 0 24px;
}
.lp-step-num {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--green), #00d46a);
  color: #fff;
  font-family: var(--font-h);
  font-size: 20px;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 20px;
  box-shadow: 0 6px 20px rgba(0, 177, 79, 0.3);
  flex-shrink: 0;
}
.lp-step-content h3 {
  font-family: var(--font-h);
  font-size: 17px;
  font-weight: 700;
  color: var(--dark);
  margin: 0 0 8px;
}
.lp-step-content p {
  font-size: 14px;
  color: var(--muted);
  line-height: 1.55;
  margin: 0;
}
.lp-step-connector {
  position: absolute;
  top: 25px;
  height: 2px;
  background: var(--border);
}
.lp-step-connector.connector-1 {
  left: calc(16.67% + 24px);
  width: calc(33.33% - 48px);
}
.lp-step-connector.connector-2 {
  left: calc(50% + 24px);
  width: calc(33.33% - 48px);
}

/* TEAM */
.lp-team-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 20px;
}
.lp-team-card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 14px;
  padding: 28px 24px;
  text-align: center;
  transition:
    box-shadow 0.25s,
    transform 0.25s;
}
.lp-team-card:hover {
  box-shadow: 0 8px 28px rgba(13, 27, 42, 0.08);
  transform: translateY(-3px);
}
.lp-team-avatar {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: var(--font-h);
  font-size: 22px;
  font-weight: 700;
  color: #fff;
  margin: 0 auto 14px;
  box-shadow: 0 4px 14px rgba(0, 0, 0, 0.15);
}
.lp-team-name {
  font-family: var(--font-h);
  font-size: 15px;
  font-weight: 700;
  color: var(--dark);
  margin-bottom: 4px;
}
.lp-team-role {
  font-size: 13px;
  color: var(--muted);
  margin-bottom: 12px;
}
.lp-team-badge {
  display: inline-block;
  font-size: 11px;
  font-weight: 600;
  padding: 4px 12px;
  border-radius: 9999px;
  background: rgba(0, 177, 79, 0.1);
  color: var(--green);
  border: 1px solid rgba(0, 177, 79, 0.2);
}

/* CTA */
.lp-cta-section {
  position: relative;
  padding: 96px 0;
  background: linear-gradient(160deg, #0d1b2a, #1a2f1e);
  overflow: hidden;
  text-align: center;
}
.lp-cta-bg {
  position: absolute;
  inset: 0;
  pointer-events: none;
}
.lp-cta-inner {
  position: relative;
  z-index: 1;
}
.lp-cta-title {
  font-family: var(--font-h);
  font-size: clamp(28px, 4vw, 48px);
  font-weight: 700;
  color: #fff;
  margin: 0 0 16px;
  letter-spacing: -0.5px;
}
.lp-cta-sub {
  font-size: 18px;
  color: rgba(255, 255, 255, 0.5);
  margin: 0 0 36px;
}
.lp-cta-meta {
  display: flex;
  justify-content: center;
  gap: 28px;
  margin-top: 24px;
  flex-wrap: wrap;
}
.lp-cta-meta span {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.4);
  font-weight: 500;
}

/* FOOTER */
.lp-footer {
  background: var(--dark);
  padding: 56px 0 0;
}
.lp-footer-inner {
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 64px;
  padding-bottom: 48px;
}
.lp-footer-brand p {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.38);
  line-height: 1.7;
  margin: 0;
  max-width: 260px;
}
.lp-footer-links {
  display: flex;
  gap: 48px;
}
.lp-footer-col {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.lp-footer-col-title {
  font-size: 11px;
  font-weight: 700;
  color: rgba(255, 255, 255, 0.3);
  text-transform: uppercase;
  letter-spacing: 0.08em;
  margin-bottom: 4px;
}
.lp-footer-col a {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.45);
  text-decoration: none;
  transition: color 0.2s;
}
.lp-footer-col a:hover {
  color: #fff;
}
.lp-footer-col span {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.3);
}
.lp-footer-bottom {
  border-top: 1px solid rgba(255, 255, 255, 0.08);
  padding: 20px 0;
  font-size: 12px;
  color: rgba(255, 255, 255, 0.22);
}

/* RESPONSIVE */
@media (max-width: 900px) {
  .lp-features-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .lp-stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .lp-team-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .lp-svc-cards {
    grid-template-columns: 1fr;
  }
  .lp-arch {
    gap: 6px;
  }
  .lp-arch-svc-row {
    flex-wrap: wrap;
    justify-content: center;
  }
  .lp-footer-inner {
    grid-template-columns: 1fr;
    gap: 32px;
  }
  .lp-footer-links {
    flex-wrap: wrap;
    gap: 32px;
  }
  .lp-steps {
    flex-direction: column;
    align-items: center;
    gap: 32px;
  }
  .lp-step-connector {
    display: none;
  }
  .lp-step {
    max-width: 360px;
  }
}
@media (max-width: 640px) {
  .lp-container {
    padding: 0 16px;
  }
  .lp-nav-links {
    display: none;
  }
  .lp-btn-nav {
    display: none;
  }
  .lp-hamburger {
    display: flex;
  }
  .lp-mobile-menu {
    display: flex;
  }
  .lp-features-grid {
    grid-template-columns: 1fr;
  }
  .lp-stats-grid {
    grid-template-columns: 1fr 1fr;
  }
  .lp-team-grid {
    grid-template-columns: 1fr;
  }
  .lp-hero-cta {
    flex-direction: column;
    align-items: stretch;
  }
  .lp-btn-primary,
  .lp-btn-ghost {
    justify-content: center;
  }
  .lp-preview-stats {
    grid-template-columns: 1fr 1fr;
  }
  .lp-preview-thead {
    display: none;
  }
  .lp-preview-row {
    grid-template-columns: 1fr 1fr;
    font-size: 11px;
  }
  .lp-section {
    padding: 64px 0;
  }
  .lp-cta-meta {
    flex-direction: column;
    gap: 10px;
    align-items: center;
  }
  .lp-arch {
    flex-direction: column;
  }
  .lp-arch-line-h {
    width: 2px;
    height: 20px;
  }
  .lp-arch-svc-row {
    flex-direction: column;
    align-items: center;
  }
}
</style>
