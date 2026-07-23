import { Button, Textarea } from "@mantine/core";
import { Controller, useFieldArray, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Link } from "@tanstack/react-router";
import { ImagePlus, Plus, X } from "lucide-react";
import React, { useRef, useState } from "react";
import { LanguageCode } from "@features/shared/enums";
import type { CreateFlowerRequest } from "./types";
import styles from "./CreateFlowerComponent.module.scss";

const LANGUAGE_LABELS: Record<LanguageCode, string> = {
  [LanguageCode.UA]: "Ukrainian",
  [LanguageCode.RO]: "Romanian",
};

const OPTIONAL_LANGUAGES = [LanguageCode.RO] as const;

const schema = z.object({
  price: z
    .number({ error: "Price is required." })
    .gt(0, { error: "Price must be greater than 0." }),
  flowerNames: z
    .array(
      z.object({
        languageCode: z.enum(Object.values(LanguageCode) as [LanguageCode, ...LanguageCode[]]),
        name: z.string().min(1, { error: "Name is required." }),
      })
    )
    .min(1)
    .refine(
      (names) => names.some((n) => n.languageCode === LanguageCode.UA),
      { message: "Ukrainian name is required." }
    ),
  description: z.string().min(1, { error: "Description is required." }),
  primaryImage: z
    .instanceof(File, { message: "Primary image is required." })
    .refine((f) => f.size > 0, { message: "Primary image must not be empty." }),
  galleryImages: z.array(z.instanceof(File)).optional(),
});

type FormFields = z.infer<typeof schema>;

interface Props {
  onSubmit: (data: CreateFlowerRequest) => void;
  isPending: boolean;
}

export const CreateFlowerComponent = ({ onSubmit, isPending }: Props) => {
  const primaryImageInputRef = useRef<HTMLInputElement>(null);
  const galleryInputRef = useRef<HTMLInputElement>(null);

  const [primaryImagePreview, setPrimaryImagePreview] = useState<string | null>(null);
  const [galleryPreviews, setGalleryPreviews] = useState<{ url: string; file: File }[]>([]);

  const {
    register,
    handleSubmit,
    control,
    setValue,
    formState: { errors, isValid },
  } = useForm<FormFields>({
    resolver: zodResolver(schema),
    mode: "onChange",
    defaultValues: {
      flowerNames: [{ languageCode: LanguageCode.UA, name: "" }],
      galleryImages: [],
    },
  });

  const { fields, append, remove } = useFieldArray({ control, name: "flowerNames" });

  const addedLanguages = fields.map((f) => f.languageCode);
  const availableToAdd = OPTIONAL_LANGUAGES.filter((lang) => !addedLanguages.includes(lang));

  const handleAddLanguage = (lang: LanguageCode) => {
    append({ languageCode: lang, name: "" });
  };

  const handlePrimaryImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setValue("primaryImage", file, { shouldValidate: true });
    setPrimaryImagePreview(URL.createObjectURL(file));
  };

  const handleRemovePrimaryImage = () => {
    setValue("primaryImage", undefined as unknown as File, { shouldValidate: true });
    setPrimaryImagePreview(null);
    if (primaryImageInputRef.current) primaryImageInputRef.current.value = "";
  };

  const handleGalleryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(e.target.files ?? []);
    const newPreviews = files.map((file) => ({ url: URL.createObjectURL(file), file }));
    const updated = [...galleryPreviews, ...newPreviews];
    setGalleryPreviews(updated);
    setValue("galleryImages", updated.map((p) => p.file), { shouldValidate: true });
    if (galleryInputRef.current) galleryInputRef.current.value = "";
  };

  const handleRemoveGalleryImage = (index: number) => {
    const updated = galleryPreviews.filter((_, i) => i !== index);
    setGalleryPreviews(updated);
    setValue("galleryImages", updated.map((p) => p.file), { shouldValidate: true });
  };

  const submitHandler: SubmitHandler<FormFields> = (data) => {
    onSubmit({ ...data, galleryImages: data.galleryImages ?? [] });
  };

  return (
    <div className={styles.page}>
      <header className={styles.header}>
        <h1>Create New Flower</h1>
        {/*<Link to="/flowers" className={styles.backLink}>*/}
        {/*  ← Back to Catalog*/}
        {/*</Link>*/}
      </header>

      <form className={styles.form} onSubmit={handleSubmit(submitHandler)}>

        <section className={styles.section}>
          <h2 className={styles.sectionTitle}>Flower Names</h2>

          {fields.map((field, index) => {
            const isRequired = field.languageCode === LanguageCode.UA;
            return (
              <div key={field.id} className={styles.languageCard}>
                <div className={styles.languageCardHeader}>
                  <span>{LANGUAGE_LABELS[field.languageCode]}</span>
                  <div style={{ display: "flex", alignItems: "center", gap: "0.5rem" }}>
                    {isRequired
                      ? <span className={styles.requiredBadge}>Required</span>
                      : <span className={styles.optionalBadge}>Optional</span>
                    }
                    {!isRequired && (
                      <button
                        type="button"
                        className={styles.removeBtn}
                        style={{ position: "static" }}
                        onClick={() => remove(index)}
                        aria-label="Remove language"
                      >
                        <X size={12} />
                      </button>
                    )}
                  </div>
                </div>
                <input type="hidden" {...register(`flowerNames.${index}.languageCode`)} />
                <div>
                  <input
                    className={styles.fieldInput}
                    placeholder={`Flower name in ${LANGUAGE_LABELS[field.languageCode]}...`}
                    {...register(`flowerNames.${index}.name`)}
                  />
                  {errors.flowerNames?.[index]?.name && (
                    <p className={styles.fieldError}>
                      {errors.flowerNames[index].name.message}
                    </p>
                  )}
                </div>
              </div>
            );
          })}

          {availableToAdd.length > 0 && (
            <div className={styles.addLanguageRow}>
              {availableToAdd.map((lang) => (
                <button
                  key={lang}
                  type="button"
                  className={styles.addLangBtn}
                  onClick={() => handleAddLanguage(lang)}
                >
                  <Plus size={14} />
                  Add {LANGUAGE_LABELS[lang]}
                </button>
              ))}
            </div>
          )}
        </section>

        <section className={styles.section}>
          <h2 className={styles.sectionTitle}>Price</h2>
          <Controller
            control={control}
            name="price"
            render={({ field }) => (
              <div>
                <div className={styles.priceWrapper}>
                  <span className={styles.priceCurrency}>₴</span>
                  <input
                    className={styles.priceInput}
                    type="number"
                    inputMode="decimal"
                    min="0.01"
                    step="0.01"
                    placeholder="0.00"
                    value={field.value ?? ""}
                    onChange={(e) => {
                      const val = e.target.value;
                      field.onChange(val === "" ? undefined : parseFloat(val));
                    }}
                  />
                </div>
                {errors.price && (
                  <p className={styles.fieldError}>{errors.price.message}</p>
                )}
              </div>
            )}
          />
        </section>

        <section className={styles.section}>
          <h2 className={styles.sectionTitle}>Description</h2>
          <Textarea
            placeholder="Describe the flower..."
            minRows={3}
            autosize
            {...register("description")}
            error={errors.description?.message}
          />
        </section>

        <section className={styles.section}>
          <h2 className={styles.sectionTitle}>Primary Image</h2>
          <input
            ref={primaryImageInputRef}
            type="file"
            accept="image/*"
            style={{ display: "none" }}
            onChange={handlePrimaryImageChange}
          />
          {!primaryImagePreview ? (
            <div
              className={styles.imageUpload}
              onClick={() => primaryImageInputRef.current?.click()}
            >
              <div className={styles.uploadIcon}><ImagePlus size={32} strokeWidth={1.5} /></div>
              <p className={styles.uploadLabel}>Click to upload primary image</p>
              <p className={styles.uploadHint}>JPG, PNG, WEBP</p>
            </div>
          ) : (
            <div className={styles.imagePreview}>
              <img src={primaryImagePreview} alt="Primary preview" />
              <button
                type="button"
                className={styles.removeBtn}
                onClick={handleRemovePrimaryImage}
                aria-label="Remove image"
              >
                <X size={12} />
              </button>
            </div>
          )}
          {errors.primaryImage && (
            <p className={styles.fieldError}>{errors.primaryImage.message}</p>
          )}
        </section>

        <section className={styles.section}>
          <h2 className={styles.sectionTitle}>Gallery Images <span style={{ fontWeight: 400, textTransform: "none", letterSpacing: 0, fontSize: "0.8em", color: "#999" }}>(optional)</span></h2>
          <input
            ref={galleryInputRef}
            type="file"
            accept="image/*"
            multiple
            style={{ display: "none" }}
            onChange={handleGalleryChange}
          />
          {galleryPreviews.length > 0 && (
            <div className={styles.galleryGrid}>
              {galleryPreviews.map((preview, index) => (
                <div key={index} className={styles.galleryItem}>
                  <img src={preview.url} alt={`Gallery ${index + 1}`} />
                  <button
                    type="button"
                    className={styles.removeBtn}
                    onClick={() => handleRemoveGalleryImage(index)}
                    aria-label="Remove gallery image"
                  >
                    <X size={12} />
                  </button>
                </div>
              ))}
            </div>
          )}
          <button
            type="button"
            className={styles.addGalleryBtn}
            onClick={() => galleryInputRef.current?.click()}
          >
            <ImagePlus size={16} />
            Add Images
          </button>
        </section>

        <div className={styles.formActions}>
          <Link to="/flowers">
            <Button variant="subtle" c="dark">Cancel</Button>
          </Link>
          <button
            className="black"
            type="submit"
            disabled={!isValid || isPending}
          >
            {isPending ? "Creating..." : "Create Flower"}
          </button>
        </div>
      </form>
    </div>
  );
};
