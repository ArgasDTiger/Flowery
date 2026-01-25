export interface PaginationParams {
  pageNumber?: number | null;
  pageSize?: number | null;
}

export interface OrderedPaginationParams extends PaginationParams {
  sortDirection?: 'asc' | 'desc' | null;
}
