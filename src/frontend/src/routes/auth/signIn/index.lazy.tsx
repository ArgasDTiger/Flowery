import { createLazyFileRoute } from '@tanstack/react-router';
import { type SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormField, Error, FormCheckbox } from "@components/ui/form";
import { useState } from "react";


const schema = z.object({
  email: z.email({ message: "Invalid email." }),
  password: z.string().nonempty({ message: "Password is required." }),
  rememberMe: z.boolean().optional(),
});

type FormFields = z.infer<typeof schema>;

export const Route = createLazyFileRoute('/auth/signIn/')({
  component: RouteComponent,
});

function RouteComponent() {
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
    try {
      await new Promise(resolve => setTimeout(resolve, 1000));
      console.log(data);
    } catch (e) {
      console.error(e);
      setResponseErrorMessage("Invalid credentials");
    }
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
