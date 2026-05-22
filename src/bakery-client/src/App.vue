<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { computed } from 'vue'
import { useBakeryStore } from '@/stores/bakery'

const store = useBakeryStore()
const cartCount = computed(() => store.cartItemCount)
</script>

<template>
  <div class="app">
    <header>
      <div class="header-content">
        <h1 class="logo">🥐 The Bakery</h1>
        <nav>
          <RouterLink to="/">Browse Items</RouterLink>
          <RouterLink to="/cart" class="cart-link">
            🛒 Cart
            <span v-if="cartCount > 0" class="cart-badge">{{ cartCount }}</span>
          </RouterLink>
        </nav>
      </div>
    </header>

    <main>
      <RouterView />
    </main>

    <footer>
      <p>&copy; 2025 The Bakery. Fresh baked goods daily!</p>
    </footer>
  </div>
</template>

<style scoped>
.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

header {
  background: linear-gradient(135deg, #8b4513 0%, #d2691e 100%);
  color: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.header-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo {
  margin: 0;
  font-size: 1.8rem;
  font-weight: bold;
}

nav {
  display: flex;
  gap: 1.5rem;
  align-items: center;
}

nav a {
  color: white;
  text-decoration: none;
  font-weight: 500;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  transition: background 0.3s;
}

nav a:hover {
  background: rgba(255, 255, 255, 0.2);
}

nav a.router-link-exact-active {
  background: rgba(255, 255, 255, 0.3);
}

.cart-link {
  position: relative;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.cart-badge {
  background: #ff4444;
  color: white;
  border-radius: 50%;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.85rem;
  font-weight: bold;
}

main {
  flex: 1;
  background: #f5f5f5;
}

footer {
  background: #333;
  color: white;
  text-align: center;
  padding: 1.5rem;
  margin-top: auto;
}

footer p {
  margin: 0;
}
</style>
