import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';
import { tanstackRouter } from '@tanstack/router-plugin/vite';

const srcPath = path.resolve(__dirname, 'src');

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    tanstackRouter({
      target: 'react',
      autoCodeSplitting: true,
    }),
    react()
  ],
  resolve: {
    alias: {
      '@app': path.resolve(srcPath, 'app'),
      '@components': path.resolve(srcPath, 'components'),
      '@config': path.resolve(srcPath, 'config'),
      '@features': path.resolve(srcPath, 'features'),
      '@lib': path.resolve(srcPath, 'lib'),
      '@routes': path.resolve(srcPath, 'routes'),
      '@styles': path.resolve(srcPath, 'styles'),
    },
  },
});
