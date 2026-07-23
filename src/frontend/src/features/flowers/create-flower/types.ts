import { LanguageCode } from "@features/shared/enums";

export interface FlowerNameRequest {
  languageCode: LanguageCode;
  name: string;
}

export interface CreateFlowerRequest {
  price: number;
  flowerNames: FlowerNameRequest[];
  description: string;
  primaryImage: File;
  galleryImages?: File[];
}

export type CreateFlowerResponse = string;
