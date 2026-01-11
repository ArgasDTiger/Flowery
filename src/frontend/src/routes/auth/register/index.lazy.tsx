import { createLazyFileRoute } from '@tanstack/react-router';
import { type SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormField, Error } from "@components/ui/form";
import { useState } from "react";

const schema = z.object({
  firstName: z.string().nonempty({ message: "First name is required." }),
  lastName: z.string().nonempty({ message: "Last name is required." }),
  email: z.email({ message: "Invalid email." }),
  phoneNumber: z.string().length(10, { message: "Invalid phone number." }).optional(),
  password: z.string().regex(new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$"),
      { message: "Password must contain at least 8 characters, one uppercase letter, one lowercase letter and one number." }),
  passwordConfirm: z.string(),
}).refine((data) => data.password === data.passwordConfirm, {
  message: "Passwords do not match",
  path: ["passwordConfirm"]
});

type FormFields = z.infer<typeof schema>;

export const Route = createLazyFileRoute('/auth/register/')({
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
      <h1>Register</h1>
      <FormField
        label='First Name'
        name='firstName'
        register={register}
        error={errors.firstName}
        type="text"
      />
      <FormField
        label='Last Name'
        name='lastName'
        register={register}
        error={errors.lastName}
        type="text"
      />
      <FormField
        label='Email'
        name='email'
        register={register}
        error={errors.email}
        type="email"
      />
      <FormField
        label='Phone Number'
        name='phoneNumber'
        register={register}
        error={errors.phoneNumber}
        type="tel"
      />
      <FormField
        label='Password'
        name='password'
        register={register}
        error={errors.password}
        type="password"
      />
      <FormField
        label='Confirm Password'
        name='passwordConfirm'
        register={register}
        error={errors.passwordConfirm}
        type="password"
      />
      <button className="black" disabled={!isValid || isSubmitting} type="submit">Register</button>
      {responseErrorMessage && <Error/>}
    </form>
  );
}
