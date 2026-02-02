import { createLazyFileRoute } from '@tanstack/react-router';
import { GetFlowersComponent, GetFlowersSortField, useFlowers } from "@features/flowers/get-flowers";
import { useCategories } from "@features/categories/get-categories";
import { keepPreviousData } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { Route as FlowersRoute } from "./index";
import type { GetFlowersSchemaType } from "./index";
import { ComboboxData } from "@mantine/core";
import { SortDirection } from "@features/shared/pagination";

export const Route = createLazyFileRoute('/flowers/')({
  component: RouteComponent
});

function RouteComponent() {
  const navigate = FlowersRoute.useNavigate();
  const searchParams: GetFlowersSchemaType = FlowersRoute.useSearch();

  const [searchInput, setSearchInput] = useState(searchParams.search || '');

  const { data, isPending, isError, error } = useFlowers({
    request: {
      pageNumber: searchParams.pageNumber,
      pageSize: searchParams.pageSize,
      search: searchParams.search,
      category: searchParams.category,
      sortBy: searchParams.sortBy,
      sortDirection: searchParams.sortDirection,
    },
    queryConfig: {
      placeholderData: keepPreviousData
    },
  });

  const { data: categoriesData, isError: isCategoriesError, error: categoriesError } = useCategories({
    request: {
      pageNumber: 1,
      pageSize: 100,
    },
  });

  useEffect(() => {
    if (isError) {
      toast.error(error?.message || "Failed to load the flowers.");
    }
  }, [isError, error]);

  useEffect(() => {
    if (isCategoriesError) {
      toast.error(categoriesError?.message || "Failed to load categories.");
    }
  }, [isCategoriesError, categoriesError]);

  const commitSearch = () => {
    const value = searchInput.length >= 2 ? searchInput : undefined;
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        search: value,
        pageNumber: 1,
      }),
    });
  };

  const handlePageChange = (newPageNumber: number) => {
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        pageNumber: newPageNumber,
      }),
    });
  };

  const handleCategoryChange = (category: string | null) => {
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        category: category || undefined,
        pageNumber: 1,
      }),
    });
  };

  const handleSortByChange = (sortBy: string | null) => {
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        sortBy: (sortBy as GetFlowersSortField) || undefined,
        sortDirection: sortBy ? (old.sortDirection || SortDirection.Asc) : undefined,
      }),
    });
  };

  const handleSortDirectionChange = (sortDirection?: SortDirection | null) => {
    navigate({
      search: (old: GetFlowersSchemaType) => ({
        ...old,
        sortDirection: sortDirection,
      }),
    });
  };

  const flowerItems = data?.items ?? [];
  const totalCount = data?.totalCount ?? 0;

  const categoryOptions: ComboboxData = categoriesData?.items.map(cat => ({
    value: cat.slug,
    label: cat.name,
  })) ?? [];

  return (
    <GetFlowersComponent
      flowers={flowerItems}
      totalCount={totalCount}
      activePage={searchParams.pageNumber}
      pageSize={searchParams.pageSize}
      onPageChange={handlePageChange}
      searchValue={searchInput}
      onSearchChange={setSearchInput}
      onSearchCommit={commitSearch}
      onCategoryChange={handleCategoryChange}
      onSortByChange={handleSortByChange}
      onSortDirectionChange={handleSortDirectionChange}
      selectedCategory={searchParams.category || null}
      sortBy={searchParams.sortBy || null}
      sortDirection={searchParams.sortDirection || null}
      isPending={isPending}
      categories={categoryOptions}
    />
  );
}
