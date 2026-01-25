import { api } from "@lib/api-client";
import type { GetFlowersRequest, PaginatedFlowersResponse } from "@features/flowers/get-flowers";
import { queryOptions, useQuery } from "@tanstack/react-query";
import type { QueryConfig } from "@lib/react-query";

export const getFlowers = async (request: GetFlowersRequest): Promise<PaginatedFlowersResponse> => {
  const response = api.get("/flowers", {
    params: {
      ...request
    },
  });

  const res = await response;
  return res.data;
};

export const getFlowersQueryOptions = (request: GetFlowersRequest) => {
  return queryOptions({
    queryKey: ["getFlowers", request],
    queryFn: () => getFlowers(request),
  });
};

type UseFlowersQueryOptions = {
  request: GetFlowersRequest;
  queryConfig?: QueryConfig<typeof getFlowersQueryOptions>;
}

export const useFlowers = ({ request, queryConfig }: UseFlowersQueryOptions) => {
  return useQuery({
    ...getFlowersQueryOptions(request),
    ...queryConfig
  });
};
