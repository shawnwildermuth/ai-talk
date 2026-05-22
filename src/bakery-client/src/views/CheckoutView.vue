<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useBakeryStore } from '@/stores/bakery'

const router = useRouter()
const store = useBakeryStore()

const customerName = ref('')
const orderPlaced = ref(false)
const error = ref<string | null>(null)

async function placeOrder() {
  if (!customerName.value.trim()) {
    error.value = 'Please enter your name'
    return
  }

  error.value = null

  try {
    await store.createOrder(customerName.value)
    orderPlaced.value = true
    setTimeout(() => {
      router.push('/')
    }, 3000)
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Failed to place order'
  }
}

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}
</script>

<template>
  <div class="checkout-view">
    <h1>Checkout</h1>

    <div v-if="orderPlaced" class="success-message">
      <h2>✓ Order Placed Successfully!</h2>
      <p>Thank you for your order, {{ customerName }}!</p>
      <p>Redirecting to home page...</p>
    </div>

    <div v-else class="checkout-content">
      <div class="order-summary">
        <h2>Order Summary</h2>
        <div v-for="item in store.cart" :key="item.bakeryItemId" class="summary-item">
          <span>{{ item.itemName }} x {{ item.quantity }}</span>
          <span>{{ formatPrice(item.price * item.quantity) }}</span>
        </div>
        <div class="total">
          <span>Total</span>
          <span>{{ formatPrice(store.cartTotal) }}</span>
        </div>
      </div>

      <div class="checkout-form">
        <h2>Customer Information</h2>
        <form @submit.prevent="placeOrder">
          <div class="form-group">
            <label for="name">Your Name *</label>
            <input
              id="name"
              v-model="customerName"
              type="text"
              placeholder="Enter your name"
              required
            />
          </div>

          <div v-if="error" class="error-message">{{ error }}</div>

          <button type="submit" class="place-order-btn" :disabled="store.loading">
            {{ store.loading ? 'Placing Order...' : 'Place Order' }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<style scoped>
.checkout-view {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

h1 {
  text-align: center;
  color: #8b4513;
  margin-bottom: 2rem;
}

.success-message {
  text-align: center;
  padding: 3rem;
  background: white;
  border: 2px solid #4caf50;
  border-radius: 8px;
}

.success-message h2 {
  color: #4caf50;
  margin: 0 0 1rem 0;
}

.success-message p {
  color: #666;
  margin: 0.5rem 0;
}

.checkout-content {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.order-summary,
.checkout-form {
  background: white;
  padding: 2rem;
  border: 1px solid #ddd;
  border-radius: 8px;
}

h2 {
  margin: 0 0 1.5rem 0;
  color: #8b4513;
}

.summary-item {
  display: flex;
  justify-content: space-between;
  padding: 0.75rem 0;
  border-bottom: 1px solid #eee;
}

.total {
  display: flex;
  justify-content: space-between;
  padding: 1rem 0;
  margin-top: 1rem;
  border-top: 2px solid #ddd;
  font-size: 1.25rem;
  font-weight: bold;
  color: #8b4513;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  color: #8b4513;
  font-weight: 600;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

input:focus {
  outline: none;
  border-color: #d2691e;
}

.error-message {
  color: #d32f2f;
  padding: 0.75rem;
  background: #ffebee;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.place-order-btn {
  width: 100%;
  padding: 1rem;
  background: #d2691e;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1.1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s;
}

.place-order-btn:hover:not(:disabled) {
  background: #a0522d;
}

.place-order-btn:disabled {
  background: #ccc;
  cursor: not-allowed;
}
</style>
