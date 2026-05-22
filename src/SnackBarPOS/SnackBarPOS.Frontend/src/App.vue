<script setup lang="ts">
import { RouterView, RouterLink, useRoute } from 'vue-router'
import { useOrderStore } from '@/stores/orderStore'

const orderStore = useOrderStore()
const route = useRoute()

const navItems = [
  { to: '/', label: 'Dashboard', icon: '📊' },
  { to: '/register', label: 'Kassa', icon: '🛒' },
  { to: '/orders', label: 'Bestellingen', icon: '📋' },
  { to: '/products', label: 'Producten', icon: '🍟' },
  { to: '/categories', label: 'Categorieën', icon: '🏷️' },
]
</script>

<template>
  <div class="min-h-screen bg-base-200">
    <!-- Navbar -->
    <nav class="navbar bg-primary text-primary-content shadow-lg sticky top-0 z-50">
      <div class="navbar-start">
        <span class="text-xl font-bold">🥨 SnackBar POS</span>
        <span class="ml-2 text-sm opacity-70">Amsterdam</span>
      </div>
      <div class="navbar-center hidden lg:flex gap-1">
        <RouterLink
          v-for="item in navItems"
          :key="item.to"
          :to="item.to"
          class="btn btn-ghost btn-sm"
          :class="{ 'btn-active': route.path === item.to || (item.to !== '/' && route.path.startsWith(item.to)) }"
        >
          <span>{{ item.icon }}</span>
          {{ item.label }}
        </RouterLink>
      </div>
      <div class="navbar-end gap-2">
        <div v-if="orderStore.currentOrder" class="badge badge-secondary gap-1">
          🛒 {{ orderStore.itemCount }} item{{ orderStore.itemCount !== 1 ? 's' : '' }}
        </div>
        <!-- Mobile menu -->
        <div class="dropdown dropdown-end lg:hidden">
          <label tabindex="0" class="btn btn-ghost">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </label>
          <ul tabindex="0" class="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 text-base-content rounded-box w-52">
            <li v-for="item in navItems" :key="item.to">
              <RouterLink :to="item.to">{{ item.icon }} {{ item.label }}</RouterLink>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Main content -->
    <main class="container mx-auto px-4 py-6 max-w-7xl">
      <RouterView />
    </main>
  </div>
</template>
