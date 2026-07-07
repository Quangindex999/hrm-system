# ---- Stage 1: Build ----
FROM node:22-alpine AS builder

WORKDIR /app

# Copy package files first để tận dụng Docker layer cache
COPY package.json package-lock.json ./
RUN npm install

# Copy toàn bộ source code
COPY . .

# Build production bundle (sẽ đọc .env.production)
RUN npm run build

# ---- Stage 2: Serve ----
FROM nginx:1.27-alpine

# Xóa config mặc định của nginx
RUN rm /etc/nginx/conf.d/default.conf

# Copy nginx config tuỳ chỉnh (có proxy tới các backend service)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy file build từ stage 1 vào thư mục serve của nginx
COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["/bin/sh", "-c", "envsubst '${VITE_HR_URL} ${VITE_ATTEND_URL} ${VITE_PAYROLL_URL}' < /etc/nginx/conf.d/default.conf > /tmp/nginx.conf && mv /tmp/nginx.conf /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
