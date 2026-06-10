import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { VitePWA } from 'vite-plugin-pwa'
import https from 'https'

export default defineConfig({
  plugins: [
    vue(),
    VitePWA({
      registerType: 'autoUpdate',
      injectRegister: 'auto',
      manifest: {
        name: 'TasteBuds',
        short_name: 'TasteBuds',
        description: 'My Fullstack PWA app for sharing interesting content!',
        theme_color: '#4DBA87',
        background_color: '#ffffff',
        display: 'standalone',
        icons: [
          {
            src: 'TasteBuds-favicon.png',
            sizes: '192x192',
            type: 'image/png'
          },
          {
            src: 'Tastebuds-logo.png',
            sizes: '512x512',
            type: 'image/png',
            purpose: 'any maskable'
          }
        ]
      }
    })
  ],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'https://localhost:7082',
        changeOrigin: true,
        secure: false,
        agent: new https.Agent({ keepAlive: true, rejectUnauthorized: false }),
        ws: true
      }
    }
  }
})
