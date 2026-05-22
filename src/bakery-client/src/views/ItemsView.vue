<script setup lang="ts">
import { onMounted } from 'vue'
import { useBakeryStore } from '@/stores/bakery'
import type { BakeryItem } from '@/types/bakery'

const store = useBakeryStore()

onMounted(() => {
  store.fetchItems()
})

const categories = ['All', 'Bread', 'Pastries', 'Cookies', 'Muffins', 'Pies']

function filterByCategory(category: string) {
  if (category === 'All') {
    store.fetchItems()
  } else {
    store.fetchItemsByCategory(category)
  }
}

function addToCart(item: BakeryItem) {
  store.addToCart(item)
}

function formatPrice(price: number): string {
  return `€${price.toFixed(2)}`
}
</script>

<template>
  <div class="items-view">
    <h1>Our Bakery Items</h1>

    <!-- Category Filter -->
    <div class="category-filter">
      <button
        v-for="category in categories"
        :key="category"
        @click="filterByCategory(category)"
        class="category-btn"
      >
        {{ category }}
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="store.loading" class="loading">Loading items...</div>

    <!-- Error State -->
    <div v-if="store.error" class="error">{{ store.error }}</div>

    <!-- Items Grid -->
    <div v-else class="items-grid">
      <div v-for="item in store.items" :key="item.id" class="item-card">
        <div class="item-content">
          <h3>{{ item.name }}</h3>
          <p class="description">{{ item.description }}</p>
          <div class="item-details">
            <span class="category">{{ item.category }}</span>
            <span class="stock" :class="{ 'out-of-stock': !item.isAvailable }">
              {{ item.isAvailable ? `${item.stockQuantity} in stock` : 'Out of stock' }}
            </span>
          </div>
          <div class="item-footer">
            <span class="price">{{ formatPrice(item.price) }}</span>
            <button
              @click="addToCart(item)"
              :disabled="!item.isAvailable"
              class="add-btn"
            >
              Add to Cart
            </button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="!store.loading && store.items.length === 0" class="empty">
      No items available
    </div>
  </div>
</template>

<style scoped>
.items-view {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

h1 {
  text-align: center;
  color: #8b4513;
  margin-bottom: 2rem;
}

.category-filter {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.category-btn {
  padding: 0.5rem 1rem;
  border: 2px solid #d2691e;
  background: white;
  color: #8b4513;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.3s;
}

.category-btn:hover {
  background: #d2691e;
  color: white;
}

.loading,
.error,
.empty {
  text-align: center;
  padding: 2rem;
  font-size: 1.2rem;
}

.error {
  color: #d32f2f;
}

.items-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

.item-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.2s, box-shadow 0.2s;
  background: white;
}

.item-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.item-content {
  padding: 1.5rem;
}

h3 {
  color: #8b4513;
  margin: 0 0 0.5rem 0;
  font-size: 1.25rem;
}

.description {
  color: #666;
  font-size: 0.9rem;
  margin: 0 0 1rem 0;
  line-height: 1.4;
}

.item-details {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;
  font-size: 0.85rem;
}

.category {
  background: #f5deb3;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  color: #8b4513;
  font-weight: 500;
}

.stock {
  color: #4caf50;
  font-weight: 500;
}

.stock.out-of-stock {
  color: #d32f2f;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.price {
  font-size: 1.5rem;
  font-weight: bold;
  color: #8b4513;
}

.add-btn {
  padding: 0.5rem 1rem;
  background: #d2691e;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.3s;
}

.add-btn:hover:not(:disabled) {
  background: #a0522d;
}

.add-btn:disabled {
  background: #ccc;
  cursor: not-allowed;
}
</style>
