import { Link } from 'react-router-dom'
import { useBakeryStore } from '../store/bakeryStore'
import styles from './CartPage.module.css'

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}

export default function CartPage() {
  const { cart, updateCartItemQuantity, removeFromCart } = useBakeryStore()
  const cartItemCount = useBakeryStore((s) => s.cartItemCount())
  const total = useBakeryStore((s) => s.cartTotal())

  if (cart.length === 0) {
    return (
      <div className={styles.cartView}>
        <h1>Shopping Cart</h1>
        <div className={styles.emptyCart}>
          <p>Your cart is empty</p>
          <Link to="/" className={styles.continueBtn}>
            Continue Shopping
          </Link>
        </div>
      </div>
    )
  }

  return (
    <div className={styles.cartView}>
      <h1>Shopping Cart</h1>
      <div className={styles.cartContent}>
        <div className={styles.cartItems}>
          {cart.map((item) => (
            <div key={item.bakeryItemId} className={styles.cartItem}>
              <div className={styles.itemInfo}>
                <h3>{item.itemName}</h3>
                <p className={styles.itemPrice}>{formatPrice(item.price)} each</p>
              </div>
              <div className={styles.itemControls}>
                <div className={styles.quantityControl}>
                  <button
                    onClick={() =>
                      updateCartItemQuantity(item.bakeryItemId, item.quantity - 1)
                    }
                  >
                    -
                  </button>
                  <input
                    type="number"
                    value={item.quantity}
                    min={1}
                    onChange={(e) =>
                      updateCartItemQuantity(
                        item.bakeryItemId,
                        parseInt(e.target.value) || 1
                      )
                    }
                  />
                  <button
                    onClick={() =>
                      updateCartItemQuantity(item.bakeryItemId, item.quantity + 1)
                    }
                  >
                    +
                  </button>
                </div>
                <div className={styles.itemTotal}>
                  <span className={styles.subtotal}>
                    {formatPrice(item.price * item.quantity)}
                  </span>
                  <button
                    className={styles.removeBtn}
                    onClick={() => removeFromCart(item.bakeryItemId)}
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>

        <div className={styles.cartSummary}>
          <h2>Order Summary</h2>
          <div className={styles.summaryRow}>
            <span>Items ({cartItemCount})</span>
            <span>{formatPrice(total)}</span>
          </div>
          <div className={styles.summaryTotal}>
            <span>Total</span>
            <span>{formatPrice(total)}</span>
          </div>
          <Link to="/checkout" className={styles.checkoutBtn}>
            Proceed to Checkout
          </Link>
          <Link to="/" className={styles.continueShopping}>
            Continue Shopping
          </Link>
        </div>
      </div>
    </div>
  )
}
