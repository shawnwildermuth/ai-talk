import { NavLink, Outlet } from 'react-router-dom'
import { useBakeryStore } from './store/bakeryStore'
import styles from './App.module.css'

export default function App() {
  const cartItemCount = useBakeryStore((s) => s.cartItemCount())

  return (
    <div className={styles.app}>
      <header>
        <div className={styles.headerContent}>
          <h1 className={styles.logo}>🥐 The Bakery</h1>
          <nav>
            <NavLink to="/" end>
              Browse Items
            </NavLink>
            <NavLink to="/cart" className={styles.cartLink}>
              🛒 Cart
              {cartItemCount > 0 && (
                <span className={styles.cartBadge}>{cartItemCount}</span>
              )}
            </NavLink>
          </nav>
        </div>
      </header>

      <main>
        <Outlet />
      </main>

      <footer>
        <p>&copy; 2025 The Bakery. Fresh baked goods daily!</p>
      </footer>
    </div>
  )
}
