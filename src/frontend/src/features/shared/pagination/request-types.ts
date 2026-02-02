export enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

export interface PaginationParams {
  pageNumber?: number | null;
  pageSize?: number | null;
}

export interface OrderedPaginationParams extends PaginationParams {
  sortDirection?: SortDirection.Asc | SortDirection.Desc | null;
}
