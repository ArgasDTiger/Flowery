import type { OrderedPaginationParams, PaginatedResponse } from "@features/shared/pagination";

export interface GetFlowersResponse {
  id: string;
  name: string;
  slug: string;
  // description: string;
  price: number;
  // Category: string;
}

export type PaginatedFlowersResponse = PaginatedResponse<GetFlowersResponse>;

export interface GetFlowersRequest extends OrderedPaginationParams {
  sortBy?: string | null;
}
