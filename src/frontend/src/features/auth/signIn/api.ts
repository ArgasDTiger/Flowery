import type { SignInRequest } from "@features/auth/signIn";
import { api } from "@lib/api-client.ts";
import type { MutationConfig } from "@lib/react-query.ts";
import { useMutation } from "@tanstack/react-query";

export const signIn = (request: SignInRequest) => {
  return api.post("/auth/signIn", request);
};

type UseSignInOptions = {
  mutationConfig?: MutationConfig<typeof signIn>;
};

export const useSignIn = ({ mutationConfig, }: UseSignInOptions = {}) => {
  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: (...args) => {
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: signIn,
  });
};
