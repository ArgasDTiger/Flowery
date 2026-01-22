import type { SignUpRequest } from "./types.ts";
import { api } from "@lib/api-client.ts";
import type { MutationConfig } from "@lib/react-query.ts";
import { useMutation } from '@tanstack/react-query';

export const signUp = (request: SignUpRequest) => {
  return api.post("/auth/signUp", request);
};

type UseSignUpOptions = {
  mutationConfig?: MutationConfig<typeof signUp>;
};

export const useSignUp = ({ mutationConfig, }: UseSignUpOptions = {}) => {
  // const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: (...args) => {
      // queryClient.invalidateQueries({
      //   queryKey: getDiscussionsQueryOptions().queryKey,
      // });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: signUp,
  });
};
