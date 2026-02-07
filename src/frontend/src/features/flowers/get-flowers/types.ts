import type { OrderedPaginationParams, PaginatedResponse } from "@features/shared/pagination";

export interface GetFlowersResponse {
  name: string;
  slug: string;
  price: number;
  categories: string[];
}

export type PaginatedFlowersResponse = PaginatedResponse<GetFlowersResponse>;

export interface GetFlowersRequest extends OrderedPaginationParams {
  sortBy?: string | null;
  search?: string | null;
  category?: string | null;
}

export enum GetFlowersSortField {
  Date = 'date',
  Price = 'price',
}
