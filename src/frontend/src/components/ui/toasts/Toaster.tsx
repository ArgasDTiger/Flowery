import { useTheme } from "next-themes"
import { Toaster as Sonner, type ToasterProps } from "sonner"
import { CircleCheckIcon, InfoIcon, TriangleAlertIcon, OctagonXIcon, Loader2Icon } from "lucide-react"
import styles from './Toaster.module.scss';

export const Toaster = ({ ...props }: ToasterProps) => {
  const { theme = "system" } = useTheme()

  return (
    <Sonner
      theme={theme as ToasterProps["theme"]}
      icons={{
        success: (
          <CircleCheckIcon className={styles.iconSize} style={{ color: styles.iconColor }} />
        ),
        info: (
          <InfoIcon className={styles.iconSize} style={{ color: styles.iconColor }} />
        ),
        warning: (
          <TriangleAlertIcon className={styles.iconSize} style={{ color: styles.iconColor }} />
        ),
        error: (
          <OctagonXIcon className={styles.iconSize} style={{ color: styles.iconColor }} />
        ),
        loading: (
          <Loader2Icon className={`${styles.iconSize} ${styles.spinner}`} style={{ color: styles.spinnerIconColor }} />
        ),
      }}
      toastOptions={{
        classNames: {
          toast: "cn-toast",
        },
      }}
      {...props}
    />
  )
};
