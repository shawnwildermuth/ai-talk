import { useEffect } from 'react'
import { useBakeryStore } from '../store/bakeryStore'
import type { BakeryItem } from '../types/bakery'
import styles from './ItemsPage.module.css'

const CATEGORIES = ['All', 'Bread', 'Pastries', 'Cookies', 'Muffins', 'Pies']

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}

export default function ItemsPage() {
  const { items, loading, error, fetchItems, fetchItemsByCategory, addToCart } =
    useBakeryStore()

  useEffect(() => {
    fetchItems()
  }, [])

  function filterByCategory(category: string) {
    if (category === 'All') {
      fetchItems()
    } else {
      fetchItemsByCategory(category)
    }
  }

  function handleAddToCart(item: BakeryItem) {
    addToCart(item)
  }

  return (
    <div className={styles.itemsView}>
      <h1>Our Bakery Items</h1>

      <div className={styles.categoryFilter}>
        {CATEGORIES.map((category) => (
          <button
            key={category}
            className={styles.categoryBtn}
            onClick={() => filterByCategory(category)}
          >
            {category}
          </button>
        ))}
      </div>

      {loading && <div className={styles.loading}>Loading items...</div>}
      {error && <div className={styles.error}>{error}</div>}

      {!loading && (
        <div className={styles.itemsGrid}>
          {items.map((item) => (
            <div key={item.id} className={styles.itemCard}>
              <div className={styles.itemContent}>
                <h3>{item.name}</h3>
                <p className={styles.description}>{item.description}</p>
                <div className={styles.itemDetails}>
                  <span className={styles.category}>{item.category}</span>
                  <span
                    className={`${styles.stock} ${!item.isAvailable ? styles.outOfStock : ''}`}
                  >
                    {item.isAvailable
                      ? `${item.stockQuantity} in stock`
                      : 'Out of stock'}
                  </span>
                </div>
                <div className={styles.itemFooter}>
                  <span className={styles.price}>{formatPrice(item.price)}</span>
                  <button
                    className={styles.addBtn}
                    onClick={() => handleAddToCart(item)}
                    disabled={!item.isAvailable}
                  >
                    Add to Cart
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {!loading && items.length === 0 && (
        <div className={styles.empty}>No items available</div>
      )}
    </div>
  )
}
