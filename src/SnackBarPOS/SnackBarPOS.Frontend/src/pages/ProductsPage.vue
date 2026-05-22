<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { RouterLink } from 'vue-router'
import { useProductStore } from '@/stores/productStore'
import { useCategoryStore } from '@/stores/categoryStore'

const productStore = useProductStore()
const categoryStore = useCategoryStore()
const showInactive = ref(false)
const search = ref('')
const selectedCategory = ref<number | null>(null)

onMounted(async () => {
  await Promise.all([
    productStore.fetchAll(true),
    categoryStore.fetchAll()
  ])
})

const filtered = computed(() => {
  return productStore.products.filter(p => {
    if (!showInactive.value && !p.isActive) return false
    if (selectedCategory.value && p.categoryId !== selectedCategory.value) return false
    if (search.value) {
      const q = search.value.toLowerCase()
      return p.name.toLowerCase().includes(q) || p.categoryName.toLowerCase().includes(q)
    }
    return true
  })
})

function formatCurrency(amount: number) {
  return new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(amount)
}

async function handleToggleAvailability(id: number) {
  const product = productStore.products.find(p => p.id === id)
  if (!product) return
  await productStore.update(id, { ...product, isAvailable: !product.isAvailable })
}

async function handleDelete(id: number) {
  if (confirm('Weet u zeker dat u dit product wilt verwijderen?')) {
    await productStore.remove(id)
  }
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold">🍟 Producten</h1>
      <RouterLink to="/products/new" class="btn btn-primary">+ Nieuw Product</RouterLink>
    </div>

    <!-- Filters -->
    <div class="card bg-base-100 shadow-md">
      <div class="card-body p-4">
        <div class="flex flex-wrap gap-4 items-center">
          <input
            v-model="search"
            type="text"
            placeholder="Zoek producten..."
            class="input input-bordered input-sm flex-1 min-w-48"
          />
          <select v-model="selectedCategory" class="select select-bordered select-sm">
            <option :value="null">Alle categorieën</option>
            <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
              {{ cat.iconEmoji }} {{ cat.name }}
            </option>
          </select>
          <label class="flex items-center gap-2 cursor-pointer">
            <input v-model="showInactive" type="checkbox" class="toggle toggle-sm" />
            <span class="text-sm">Toon inactief</span>
          </label>
        </div>
      </div>
    </div>

    <!-- Products table -->
    <div class="card bg-base-100 shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Categorie</th>
              <th>Prijs</th>
              <th>Status</th>
              <th>Beschikbaar</th>
              <th>Acties</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="productStore.loading">
              <td colspan="6" class="text-center py-8">
                <span class="loading loading-spinner loading-md text-primary"></span>
              </td>
            </tr>
            <tr v-else-if="filtered.length === 0">
              <td colspan="6" class="text-center text-base-content/60 py-8">Geen producten gevonden</td>
            </tr>
            <tr v-for="product in filtered" :key="product.id" class="hover" :class="{ 'opacity-50': !product.isActive }">
              <td>
                <span class="font-semibold">{{ product.name }}</span>
                <p v-if="product.description" class="text-xs text-base-content/60">{{ product.description }}</p>
              </td>
              <td>
                <span class="badge badge-ghost gap-1">
                  {{ product.categoryEmoji }} {{ product.categoryName }}
                </span>
              </td>
              <td class="font-bold">{{ formatCurrency(product.price) }}</td>
              <td>
                <span class="badge" :class="product.isActive ? 'badge-success' : 'badge-error'">
                  {{ product.isActive ? 'Actief' : 'Inactief' }}
                </span>
              </td>
              <td>
                <input
                  type="checkbox"
                  class="toggle toggle-sm toggle-success"
                  :checked="product.isAvailable"
                  @change="handleToggleAvailability(product.id)"
                />
              </td>
              <td>
                <div class="flex gap-1">
                  <RouterLink :to="`/products/${product.id}/edit`" class="btn btn-xs btn-ghost">✏️</RouterLink>
                  <button class="btn btn-xs btn-ghost text-error" @click="handleDelete(product.id)">🗑️</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
