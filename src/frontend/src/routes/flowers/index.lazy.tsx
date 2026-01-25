import { createLazyFileRoute } from '@tanstack/react-router'
import { useFlowers } from "@features/flowers/get-flowers";
import { keepPreviousData } from "@tanstack/react-query";
import { useEffect } from "react";
import { toast } from "sonner";
import { Route as FlowersRoute } from "./index";
import type { GetFlowersSchemaType } from "./index";


export const Route = createLazyFileRoute('/flowers/')({
  component: RouteComponent
});

function RouteComponent() {
  const navigate = FlowersRoute.useNavigate();
  const searchParams: GetFlowersSchemaType = FlowersRoute.useSearch();

  const { data, isPending, isError, error } = useFlowers({
    request: {...searchParams},
    queryConfig: {
      placeholderData: keepPreviousData
    },
  });

  useEffect(() => {
    if (isError) {
      toast.error(error?.message || "Failed to load the flowers.");
    }
  }, [isError, error]);

  const handlePageChange = (newPageNumber: number) => {
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        pageNumber: newPageNumber
      }),
    })
  };

  const flowers = data?.items ?? [];
  return (
    <>
      {flowers.map(flower => (
        <div key={flower.id}>{flower.name}</div>
      ))}
    </>
  );
}
