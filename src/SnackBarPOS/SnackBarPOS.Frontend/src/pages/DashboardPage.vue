<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { RouterLink } from 'vue-router'
import { useOrderStore } from '@/stores/orderStore'

const orderStore = useOrderStore()

onMounted(async () => {
  await Promise.all([
    orderStore.fetchTodaySummary(),
    orderStore.fetchCurrent()
  ])
})

const topCategories = computed(() => {
  if (!orderStore.todaySummary?.revenueByCategory) return []
  return Object.entries(orderStore.todaySummary.revenueByCategory)
    .sort(([, a], [, b]) => b - a)
    .slice(0, 5)
})

function formatCurrency(amount: number) {
  return new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(amount)
}
</script>

<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-3xl font-bold">Dashboard</h1>
        <p class="text-base-content/60">{{ new Date().toLocaleDateString('nl-NL', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }) }}</p>
      </div>
      <RouterLink to="/register" class="btn btn-primary gap-2">
        🛒 Nieuwe Bestelling
      </RouterLink>
    </div>

    <!-- Stats cards -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-base-content/60 text-sm">Omzet Vandaag</p>
              <p class="text-3xl font-bold text-success">
                {{ formatCurrency(orderStore.todaySummary?.totalRevenue ?? 0) }}
              </p>
            </div>
            <span class="text-4xl">💶</span>
          </div>
        </div>
      </div>

      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-base-content/60 text-sm">Bestellingen Vandaag</p>
              <p class="text-3xl font-bold text-primary">
                {{ orderStore.todaySummary?.totalOrders ?? 0 }}
              </p>
            </div>
            <span class="text-4xl">📋</span>
          </div>
        </div>
      </div>

      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-base-content/60 text-sm">Items Verkocht</p>
              <p class="text-3xl font-bold text-secondary">
                {{ orderStore.todaySummary?.totalItems ?? 0 }}
              </p>
            </div>
            <span class="text-4xl">🍟</span>
          </div>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Current order status -->
      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <h2 class="card-title">📦 Actieve Bestelling</h2>
          <div v-if="orderStore.currentOrder" class="space-y-3">
            <div class="flex justify-between text-sm">
              <span class="text-base-content/60">Bestelnummer</span>
              <span class="font-mono font-bold">{{ orderStore.currentOrder.orderNumber }}</span>
            </div>
            <div class="flex justify-between text-sm">
              <span class="text-base-content/60">Items</span>
              <span>{{ orderStore.itemCount }}</span>
            </div>
            <div class="flex justify-between font-bold text-lg">
              <span>Totaal</span>
              <span class="text-success">{{ formatCurrency(orderStore.totalAmount) }}</span>
            </div>
            <RouterLink to="/register" class="btn btn-primary btn-sm w-full">
              Naar Kassa →
            </RouterLink>
          </div>
          <div v-else class="text-center py-6">
            <p class="text-base-content/60 mb-4">Geen actieve bestelling</p>
            <RouterLink to="/register" class="btn btn-primary btn-sm">
              Start Bestelling
            </RouterLink>
          </div>
        </div>
      </div>

      <!-- Revenue by category -->
      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <h2 class="card-title">📊 Omzet per Categorie</h2>
          <div v-if="topCategories.length > 0" class="space-y-3">
            <div v-for="[cat, amount] in topCategories" :key="cat" class="flex items-center gap-3">
              <span class="text-sm font-medium w-24 truncate">{{ cat }}</span>
              <div class="flex-1 bg-base-200 rounded-full h-3">
                <div
                  class="bg-primary h-3 rounded-full"
                  :style="{ width: `${(amount / (orderStore.todaySummary?.totalRevenue || 1)) * 100}%` }"
                />
              </div>
              <span class="text-sm font-bold w-20 text-right">{{ formatCurrency(amount) }}</span>
            </div>
          </div>
          <div v-else class="text-center py-6 text-base-content/60">
            Nog geen verkopen vandaag
          </div>
        </div>
      </div>
    </div>

    <!-- Quick actions -->
    <div class="card bg-base-100 shadow-md">
      <div class="card-body">
        <h2 class="card-title">⚡ Snelle Acties</h2>
        <div class="flex flex-wrap gap-3">
          <RouterLink to="/register" class="btn btn-primary gap-2">🛒 Kassa</RouterLink>
          <RouterLink to="/orders" class="btn btn-secondary gap-2">📋 Bestellingen</RouterLink>
          <RouterLink to="/products" class="btn btn-accent gap-2">🍟 Producten</RouterLink>
          <RouterLink to="/categories" class="btn btn-ghost gap-2">🏷️ Categorieën</RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>
