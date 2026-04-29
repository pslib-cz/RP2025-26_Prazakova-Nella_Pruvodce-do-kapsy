import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

const backend = process.env.VITE_API_URL ?? 'http://localhost:5000';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: backend,
        changeOrigin: true
      },
      '/data': {
        target: backend,
        changeOrigin: true
      },
      '/floors': {
        target: backend,
        changeOrigin: true
      },
      '/interior': {
        target: backend,
        changeOrigin: true
      },
      '/prumLogo.png': {
        target: backend,
        changeOrigin: true
      },
      '/icons.svg': {
        target: backend,
        changeOrigin: true
      }
    }
  }
});