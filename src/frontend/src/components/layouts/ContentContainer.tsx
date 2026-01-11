import * as React from "react";
import styles from './ContentContainer.module.scss';

interface ContentContainerProps {
  children: React.ReactNode;
}

function ContentContainer({ children}: ContentContainerProps) {
  return (
    <main className={styles.main}>
      {children}
    </main>
  );
}

export default ContentContainer;