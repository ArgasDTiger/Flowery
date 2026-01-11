import type { FieldValues, Path, UseFormRegister } from "react-hook-form";
import styles from './FormCheckbox.module.scss';

type CheckboxProps<T extends FieldValues> = {
  label: string;
  name: Path<T>;
  register?: UseFormRegister<T>;
};

export const FormCheckbox = <T extends FieldValues>({
                                                   label,
                                                   name,
                                                   register,
                                                 }: CheckboxProps<T>) => {
  return (
    <div className={styles.checkboxGroup}>
      <label htmlFor={name}>{label}</label>
      <input
        id={name}
        type='checkbox'
        {...(register ? register(name) : {})}
      />
    </div>
  );
};
