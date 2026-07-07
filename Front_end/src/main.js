import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Antd from 'ant-design-vue'
import App from './App.vue'
import router from './router'
import './assets/main.css'

console.log('[main.js] start')

const app = createApp(App)
app.config.errorHandler = (err) => {
  console.error('[Vue Error]', err)
}
console.log('[main.js] app created')

app.use(createPinia())
console.log('[main.js] pinia installed')

try {
  app.use(router)
  console.log('[main.js] router installed')
} catch (e) {
  console.error('[main.js] router error', e)
}

try {
  app.use(Antd)
  console.log('[main.js] antd installed')
} catch (e) {
  console.error('[main.js] antd error', e)
}

app.mount('#app')
console.log('[main.js] mounted')
