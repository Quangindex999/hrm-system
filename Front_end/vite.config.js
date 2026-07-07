import { fileURLToPath, URL } from 'node:url'

import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    plugins: [vue(), vueDevTools()],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    server: {
      port: 3000,
      proxy: {
        '/api-hr': {
          target: env.VITE_HR_URL || 'https://100.88.26.35:7084',
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api-hr/, ''),
        },
        '/api-attend': {
          target: env.VITE_ATTEND_URL || 'https://100.127.134.32:7108',
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api-attend/, ''),
        },
        '/api-payroll': {
          target: env.VITE_PAYROLL_URL || 'https://100.77.67.34:7300',
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api-payroll/, ''),
        },
      },
    },
  }
})
