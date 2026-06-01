import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

const backend = process.env.VITE_API_URL ?? 'http://localhost:5000';

export default defineConfig({
  plugins: [
    react()
  ],
  build: {
    outDir: '../pruvodce.server/wwwroot/app',
    emptyOutDir: true,
  },
  server: {
    proxy: {
      '/api': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/data': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/floors': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/interior': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/prumLogo.png': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/icons.svg': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/admin': {
        target: backend,
        changeOrigin: true,
        secure: false,
      },
      '/login': {
        target: backend,
        changeOrigin: true,
        secure: false,
      },
      '/Students': {
      target: backend,
      changeOrigin: true,
      secure: false
      },
      '/css': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/js': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Dashboard': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Events': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Points': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Subjects': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Teachers': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
      '/Specializations': {
        target: backend,
        changeOrigin: true,
        secure: false
      },
    }
  }
});