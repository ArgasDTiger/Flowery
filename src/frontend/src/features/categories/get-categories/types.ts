import type { PaginatedResponse, OrderedPaginationParams } from "@features/shared/pagination";

export type GetCategoriesResponse = {
  name: string;
  slug: string;
};

export type PaginatedCategoriesResponse = PaginatedResponse<GetCategoriesResponse>;

export interface GetCategoriesRequest extends OrderedPaginationParams {
  sortBy?: GetCategoriesSortField | null;
}

export enum GetCategoriesSortField {
  Name = 'name',
}
