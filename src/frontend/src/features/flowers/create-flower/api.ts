import { api } from "@lib/api-client";
import type { CreateFlowerRequest, CreateFlowerResponse } from "./types";
import { useMutation } from "@tanstack/react-query";
import type { MutationConfig } from "@lib/react-query";

export const createFlower = (request: CreateFlowerRequest): Promise<CreateFlowerResponse> => {
  const formData = new FormData();

  formData.append("price", String(request.price));
  formData.append("description", request.description);
  formData.append("primaryImage", request.primaryImage);

  request.flowerNames.forEach((name, index) => {
    formData.append(`flowerNames[${index}].languageCode`, String(name.languageCode));
    formData.append(`flowerNames[${index}].name`, name.name);
  });

  request.galleryImages?.forEach((file) => {
    formData.append("galleryImages", file);
  });

  return api.post("/flowers", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
};

type UseCreateFlowerOptions = {
  mutationConfig?: MutationConfig<typeof createFlower>;
};

export const useCreateFlower = ({ mutationConfig }: UseCreateFlowerOptions = {}) => {
  return useMutation({
    mutationFn: createFlower,
    ...mutationConfig,
  });
};
