import { createRootRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';
import { Header } from "@components/layouts";
import ContentContainer from '@components/layouts/ContentContainer';

const RootLayout = () => (
  <>
    <Header/>
    <ContentContainer>
      <Outlet />
      <TanStackRouterDevtools />
    </ContentContainer>
  </>
);

export const Route = createRootRoute({ component: RootLayout });