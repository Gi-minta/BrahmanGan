import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        // Por defecto el puerto de `dotnet run` (ver launchSettings.json). Si levantas
        // la API con `docker compose`, exporta VITE_API_PROXY=http://localhost:8080.
        target: process.env.VITE_API_PROXY || 'http://localhost:5077',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
