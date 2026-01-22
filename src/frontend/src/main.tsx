import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import '@styles/main.scss';
import { routeTree } from './routeTree.gen';
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { AppProvider } from "./provider.tsx";

const router = createRouter({ routeTree });

const root = document.getElementById('root');
if (!root) throw new Error('No root element found');

createRoot(root).render(
  <StrictMode>
    <AppProvider>
      <RouterProvider router={router} />
    </AppProvider>
  </StrictMode>
);
