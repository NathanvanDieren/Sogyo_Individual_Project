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
      workbox: {
        // 1. Cache alle statische frontend bestanden
        globPatterns: ['**/*.{js,css,html,ico,png,svg,woff2}'],
        navigateFallback: '/index.html',

        // 2. Sla API requests en groepen offline op
        runtimeCaching: [
          {
            // Onderschept alle verzoeken die naar /api lopen
            urlPattern: ({ url }) => url.pathname.startsWith('/api'),
            handler: 'NetworkFirst', // Eerst live proberen, mislukt? Dan direct de laatste cache!
            options: {
              cacheName: 'tastebuds-api-cache',
              expiration: {
                maxEntries: 100,           // Maximaal 100 verschillende API-endpoints opslaan
                maxAgeSeconds: 60 * 60 * 24 * 7 // Data blijft 7 dagen geldig in de cache
              },
              cacheableResponse: {
                statuses: [0, 200]        // Sla alleen succesvolle (200) of ondoorzichtige (0) responses op
              }
            }
          }
        ]
      },
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
