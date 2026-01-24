import { createLazyFileRoute, useNavigate } from '@tanstack/react-router';
import { type SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormField, Error, FormCheckbox } from "@components/ui/form";
import { useState } from "react";
import { type SignInRequest, useSignIn } from "@features/auth/signIn";

const schema = z.object({
  email: z.email({ message: "Invalid email." }),
  password: z.string().nonempty({ message: "Password is required." }),
  rememberMe: z.boolean().optional(),
});

type FormFields = z.infer<typeof schema>;

export const Route = createLazyFileRoute('/auth/signIn/')({
  component: RouteComponent,
});

const invalidCredentials = "Invalid credentials";
const errorOccurred = "An error occurred. Please try again.";

function RouteComponent() {
  const navigate = useNavigate();

  const signInMutation = useSignIn({
    mutationConfig: {
      onSuccess: () => {
        navigate({ to: 'flowers'});
      },
      onError: (error) => {
        if (error.response?.status === 401) {
          setResponseErrorMessage(invalidCredentials);
        } else {
          setResponseErrorMessage(errorOccurred);
        }
        setResponseErrorMessage(invalidCredentials);
      }
    },
  });

  const [responseErrorMessage, setResponseErrorMessage] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting, isValid }
  } = useForm<FormFields>({
    resolver: zodResolver(schema),
    mode: "onChange"
  });

  const onSubmit: SubmitHandler<FormFields> = async (data) => {
    setResponseErrorMessage(null);
    const request: SignInRequest = { ...data, rememberMe: data.rememberMe ?? false };
    signInMutation.mutate(request);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h1>Login</h1>
      <FormField
        label='Email'
        name='email'
        register={register}
        error={errors.email}
        type="email"
      />
      <FormField
        label='Password'
        name='password'
        type="password"
        register={register}
        error={errors.password}
      />
      <FormCheckbox
        label='Remember me'
        name='rememberMe'
        register={register}
      />
      <button className="black" disabled={!isValid || isSubmitting} type="submit">Login</button>
      {responseErrorMessage && <Error/>}
    </form>
  );
}
