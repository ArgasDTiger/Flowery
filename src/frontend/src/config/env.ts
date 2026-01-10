import * as z from 'zod';

const createEnv = () => {
  const envSchema = z.object({
    API_URL: z.string(),
  });

  const envVars = Object.entries(import.meta.env).reduce<
    Record<string, string>>((acc, _) => {
    return acc;
  }, {});

  const parsedEnv = envSchema.safeParse(envVars);

  if (!parsedEnv.success) {
    const errorMessage = Object.entries(parsedEnv.error.flatten().fieldErrors)
      .map(([k, v]) => `- ${k}: ${v}`)
      .join('\n');
    throw new Error(`Invalid env provided. The following variables are missing or invalid: ${errorMessage}`,);
  }

  return parsedEnv.data;
};

export const env = createEnv();