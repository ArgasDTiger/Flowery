import { type ReactNode, useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { queryConfig } from "@lib/react-query.ts";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { Toaster } from "@components/ui/toasts";
import { createTheme, MantineProvider } from "@mantine/core";

type AppProviderProps = {
  children: ReactNode;
};

export const AppProvider = ({ children }: AppProviderProps) => {
  const [queryClient] = useState<QueryClient>(() => new QueryClient({ defaultOptions: queryConfig }));

  const theme = createTheme({

  });

  return (
    <MantineProvider theme={theme}>
      <QueryClientProvider client={queryClient}>
        {import.meta.env.DEV && <ReactQueryDevtools />}
        <Toaster/>
        {children}
      </QueryClientProvider>
    </MantineProvider>

  );
};
