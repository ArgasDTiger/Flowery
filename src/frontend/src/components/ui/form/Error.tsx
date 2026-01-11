import type { FieldError } from "react-hook-form";
import styles from './Error.module.scss';

type ErrorProps = {
  error?: FieldError | null;
  errorMessage?: string;
};

export const Error = ({ error, errorMessage }: ErrorProps) => {
  const getTemplate = (message: string) => {
    return (
      <span
        role="alert"
        aria-label={message}
        className={styles.error}
      >
      {message}
    </span>);
  };

  if (error?.message) return getTemplate(error.message);
  if (errorMessage) return getTemplate(errorMessage);
};
