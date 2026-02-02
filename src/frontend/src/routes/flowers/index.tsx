import { createFileRoute } from '@tanstack/react-router';
import { z } from "zod";
import { SortDirection } from "@features/shared/pagination";
import { GetFlowersSortField } from "@features/flowers/get-flowers";

export const getFlowersSchema = z.object({
  search: z.string().optional(),
  category: z.string().optional(),
  pageNumber: z.number().optional().default(1),
  pageSize: z.number().optional().default(12),
  sortBy: z.enum(GetFlowersSortField).optional(),
  sortDirection: z.enum(SortDirection).optional(),
});

export type GetFlowersSchemaType = z.infer<typeof getFlowersSchema>;

export const Route = createFileRoute('/flowers/')({
  validateSearch: (search) => getFlowersSchema.parse(search),
});
