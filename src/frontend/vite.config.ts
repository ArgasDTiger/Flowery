import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';
import { tanstackRouter } from '@tanstack/router-plugin/vite';
import tsconfigPaths from "vite-tsconfig-paths";

const srcPath = path.resolve(__dirname, 'src');

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tanstackRouter({
      target: 'react',
      autoCodeSplitting: true,
    }),
    react(),
    tsconfigPaths(),
  ],
  resolve: {
    alias: {
      '@styles': path.resolve(srcPath, 'styles'),
    },
  },
});
