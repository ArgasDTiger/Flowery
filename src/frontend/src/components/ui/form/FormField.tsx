import { Error } from "@components/ui/form/Error.tsx";
import type { FieldError, FieldValues, Path, UseFormRegister } from "react-hook-form";
import styles from './FormField.module.scss';

type FormFieldProps<T extends FieldValues> = {
  label: string;
  name: Path<T>;
  register?: UseFormRegister<T>;
  error?: FieldError;
  type?: 'email' | 'password' | 'text' | 'number' | 'tel' | 'checkbox';
  placeholder?: string;
};
// TODO: labels should be on input borders?
export const FormField = <T extends FieldValues>({
                                                   label,
                                                   name,
                                                   register,
                                                   error,
                                                   type = "text",
                                                   placeholder
                                                 }: FormFieldProps<T>) => {
  return (
    <>
      <label htmlFor={name}>{label}</label>
      <input
        id={name}
        type={type}
        placeholder={placeholder}
        {...(register ? register(name) : {})}
      />
      <Error error={error}/>
    </>
  );
};
