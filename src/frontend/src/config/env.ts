import * as z from 'zod';

const viteAppPrefix = 'VITE_';

const createEnv = () => {
  const envSchema = z.object({
    API_URL: z.string(),
  });

  const envVars = Object.entries(import.meta.env).reduce<
    Record<string, string>>((acc, curr) => {
    const [key, value] = curr;
    if (key.startsWith(viteAppPrefix)) {
      acc[key.replace(viteAppPrefix, '')] = value;
    }
    return acc;
  }, {});

  const parsedEnv = envSchema.safeParse(envVars);

  if (!parsedEnv.success) {
    const flattenedError = z.flattenError(parsedEnv.error);
    const errorMessage = Object.entries(flattenedError.fieldErrors)
      .map(([k, v]) => `- ${k}: ${v}`)
      .join('\n');
    throw new Error(`Invalid env provided. The following variables are missing or invalid: ${errorMessage}`,);
  }

  return parsedEnv.data;
};

export const env = createEnv();
