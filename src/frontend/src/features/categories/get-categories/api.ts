import { api } from "@lib/api-client";
import type { GetCategoriesRequest, PaginatedCategoriesResponse } from "@features/categories/get-categories";
import { queryOptions, useQuery } from "@tanstack/react-query";
import type { QueryConfig } from "@lib/react-query";

export const getCategories = (request: GetCategoriesRequest): Promise<PaginatedCategoriesResponse> => {
  return api.get("/categories", {
    params: {
      ...request
    },
  });
};

export const getCategoriesQueryOptions = (request: GetCategoriesRequest) => {
  return queryOptions({
    queryKey: ["getCategories", request],
    queryFn: () => getCategories(request),
  });
};

type UseCategoriesQueryOptions = {
  request: GetCategoriesRequest;
  queryConfig?: QueryConfig<typeof getCategoriesQueryOptions>;
}

export const useCategories = ({ request, queryConfig }: UseCategoriesQueryOptions) => {
  return useQuery({
    ...getCategoriesQueryOptions(request),
    ...queryConfig
  });
};
