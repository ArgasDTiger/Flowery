import { Link } from '@tanstack/react-router';
import styles from './Header.module.scss';

export const Header = () => {
  return (
    <header>
      <div className={styles.pageNames}>
        <button className={styles.pageName}>
          <Link to='/home'>Home</Link>
        </button>
        <button className={styles.pageName}>
          Flowers
        </button>
        <button className={styles.pageName}>
          About Us
        </button>
      </div>
      <div className={styles.authButtons}>
        <button>
          <Link to='/auth/login'>Login</Link>
        </button>
        <button>
          <Link to='/auth/register'>Register</Link>
        </button>
      </div>
    </header>
  );
};