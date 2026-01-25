import { createFileRoute } from '@tanstack/react-router'
import { z } from "zod";

export const getFlowersSchema = z.object({
  search: z.string().optional(),
  pageNumber: z.number().optional().default(1),
  pageSize: z.number().optional().default(10),
  sortBy: z.string().optional().default('name'),
  sortDirection: z.enum(['asc', 'desc']).optional().default('asc'),
});

export type GetFlowersSchemaType = z.infer<typeof getFlowersSchema>;

export const Route = createFileRoute('/flowers/')({
  validateSearch: (search) => getFlowersSchema.parse(search),
})
