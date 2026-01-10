import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { createRouter, RouterProvider } from "@tanstack/react-router";

const router = createRouter({ routeTree });

const root = document.getElementById('root');
if (!root) throw new Error('No root element found');

createRoot(root).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);