<script setup lang="ts">
import { computed } from 'vue'
import { useBakeryStore } from '@/stores/bakery'

const store = useBakeryStore()

const cartItems = computed(() => store.cart)
const total = computed(() => store.cartTotal)

function updateQuantity(bakeryItemId: number, quantity: number) {
  store.updateCartItemQuantity(bakeryItemId, quantity)
}

function removeItem(bakeryItemId: number) {
  store.removeFromCart(bakeryItemId)
}

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}
</script>

<template>
  <div class="cart-view">
    <h1>Shopping Cart</h1>

    <div v-if="cartItems.length === 0" class="empty-cart">
      <p>Your cart is empty</p>
      <router-link to="/" class="continue-btn">Continue Shopping</router-link>
    </div>

    <div v-else class="cart-content">
      <div class="cart-items">
        <div v-for="item in cartItems" :key="item.bakeryItemId" class="cart-item">
          <div class="item-info">
            <h3>{{ item.itemName }}</h3>
            <p class="item-price">{{ formatPrice(item.price) }} each</p>
          </div>
          <div class="item-controls">
            <div class="quantity-control">
              <button @click="updateQuantity(item.bakeryItemId, item.quantity - 1)">-</button>
              <input
                type="number"
                :value="item.quantity"
                @input="updateQuantity(item.bakeryItemId, parseInt(($event.target as HTMLInputElement).value))"
                min="1"
              />
              <button @click="updateQuantity(item.bakeryItemId, item.quantity + 1)">+</button>
            </div>
            <div class="item-total">
              <span class="subtotal">{{ formatPrice(item.price * item.quantity) }}</span>
              <button @click="removeItem(item.bakeryItemId)" class="remove-btn">Remove</button>
            </div>
          </div>
        </div>
      </div>

      <div class="cart-summary">
        <h2>Order Summary</h2>
        <div class="summary-row">
          <span>Items ({{ store.cartItemCount }})</span>
          <span>{{ formatPrice(total) }}</span>
        </div>
        <div class="summary-total">
          <span>Total</span>
          <span>{{ formatPrice(total) }}</span>
        </div>
        <router-link to="/checkout" class="checkout-btn">Proceed to Checkout</router-link>
        <router-link to="/" class="continue-shopping">Continue Shopping</router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
.cart-view {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

h1 {
  text-align: center;
  color: #8b4513;
  margin-bottom: 2rem;
}

.empty-cart {
  text-align: center;
  padding: 3rem;
}

.empty-cart p {
  font-size: 1.2rem;
  color: #666;
  margin-bottom: 1.5rem;
}

.continue-btn {
  display: inline-block;
  padding: 0.75rem 1.5rem;
  background: #d2691e;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  font-weight: 600;
  transition: background 0.3s;
}

.continue-btn:hover {
  background: #a0522d;
}

.cart-content {
  display: grid;
  grid-template-columns: 1fr 350px;
  gap: 2rem;
}

@media (max-width: 768px) {
  .cart-content {
    grid-template-columns: 1fr;
  }
}

.cart-items {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.cart-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  gap: 1rem;
}

@media (max-width: 640px) {
  .cart-item {
    flex-direction: column;
    align-items: stretch;
  }
}

.item-info h3 {
  margin: 0 0 0.5rem 0;
  color: #8b4513;
}

.item-price {
  color: #666;
  margin: 0;
}

.item-controls {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.quantity-control {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.quantity-control button {
  width: 32px;
  height: 32px;
  border: 1px solid #ddd;
  background: white;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1.2rem;
  transition: background 0.2s;
}

.quantity-control button:hover {
  background: #f5f5f5;
}

.quantity-control input {
  width: 60px;
  text-align: center;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 0.5rem;
  font-size: 1rem;
}

.item-total {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 0.5rem;
}

.subtotal {
  font-size: 1.25rem;
  font-weight: bold;
  color: #8b4513;
}

.remove-btn {
  padding: 0.25rem 0.75rem;
  background: transparent;
  color: #d32f2f;
  border: 1px solid #d32f2f;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.85rem;
  transition: all 0.3s;
}

.remove-btn:hover {
  background: #d32f2f;
  color: white;
}

.cart-summary {
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1.5rem;
  height: fit-content;
  position: sticky;
  top: 2rem;
}

.cart-summary h2 {
  margin: 0 0 1rem 0;
  color: #8b4513;
  font-size: 1.5rem;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  padding: 0.5rem 0;
  color: #666;
}

.summary-total {
  display: flex;
  justify-content: space-between;
  padding: 1rem 0;
  margin-top: 1rem;
  border-top: 2px solid #ddd;
  font-size: 1.25rem;
  font-weight: bold;
  color: #8b4513;
}

.checkout-btn {
  display: block;
  width: 100%;
  padding: 1rem;
  background: #d2691e;
  color: white;
  text-align: center;
  text-decoration: none;
  border-radius: 4px;
  font-weight: 600;
  font-size: 1.1rem;
  margin-top: 1.5rem;
  transition: background 0.3s;
}

.checkout-btn:hover {
  background: #a0522d;
}

.continue-shopping {
  display: block;
  text-align: center;
  margin-top: 1rem;
  color: #8b4513;
  text-decoration: none;
  font-size: 0.9rem;
}

.continue-shopping:hover {
  text-decoration: underline;
}
</style>
