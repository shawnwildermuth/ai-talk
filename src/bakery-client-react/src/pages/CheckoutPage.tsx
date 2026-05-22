import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useBakeryStore } from '../store/bakeryStore'
import styles from './CheckoutPage.module.css'

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}

export default function CheckoutPage() {
  const navigate = useNavigate()
  const { cart, loading, createOrder } = useBakeryStore()
  const cartTotal = useBakeryStore((s) => s.cartTotal())

  const [customerName, setCustomerName] = useState('')
  const [orderPlaced, setOrderPlaced] = useState(false)
  const [error, setError] = useState<string | null>(null)

  async function handlePlaceOrder(e: React.FormEvent) {
    e.preventDefault()
    if (!customerName.trim()) {
      setError('Please enter your name')
      return
    }
    setError(null)
    try {
      await createOrder(customerName)
      setOrderPlaced(true)
      setTimeout(() => navigate('/'), 3000)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to place order')
    }
  }

  if (orderPlaced) {
    return (
      <div className={styles.checkoutView}>
        <h1>Checkout</h1>
        <div className={styles.successMessage}>
          <h2>✓ Order Placed Successfully!</h2>
          <p>Thank you for your order, {customerName}!</p>
          <p>Redirecting to home page...</p>
        </div>
      </div>
    )
  }

  return (
    <div className={styles.checkoutView}>
      <h1>Checkout</h1>
      <div className={styles.checkoutContent}>
        <div className={styles.orderSummary}>
          <h2>Order Summary</h2>
          {cart.map((item) => (
            <div key={item.bakeryItemId} className={styles.summaryItem}>
              <span>
                {item.itemName} x {item.quantity}
              </span>
              <span>{formatPrice(item.price * item.quantity)}</span>
            </div>
          ))}
          <div className={styles.total}>
            <span>Total</span>
            <span>{formatPrice(cartTotal)}</span>
          </div>
        </div>

        <div className={styles.checkoutForm}>
          <h2>Customer Information</h2>
          <form onSubmit={handlePlaceOrder}>
            <div className={styles.formGroup}>
              <label htmlFor="name">Your Name *</label>
              <input
                id="name"
                type="text"
                placeholder="Enter your name"
                value={customerName}
                onChange={(e) => setCustomerName(e.target.value)}
                required
              />
            </div>

            {error && <div className={styles.errorMessage}>{error}</div>}

            <button
              type="submit"
              className={styles.placeOrderBtn}
              disabled={loading}
            >
              {loading ? 'Placing Order...' : 'Place Order'}
            </button>
          </form>
        </div>
      </div>
    </div>
  )
}
