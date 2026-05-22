import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { productApi } from '@/services/api'
import type { Product, CreateProductRequest, UpdateProductRequest } from '@/types'

export const useProductStore = defineStore('products', () => {
  const products = ref<Product[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const byCategory = computed(() => {
    return (categoryId: number) => products.value.filter(p => p.categoryId === categoryId)
  })

  const available = computed(() =>
    products.value.filter(p => p.isActive && p.isAvailable)
  )

  async function fetchAll(includeInactive = false) {
    loading.value = true
    error.value = null
    try {
      products.value = await productApi.getAll(includeInactive)
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Fout bij laden producten'
    } finally {
      loading.value = false
    }
  }

  async function create(data: CreateProductRequest) {
    const created = await productApi.create(data)
    products.value.push(created)
    return created
  }

  async function update(id: number, data: UpdateProductRequest) {
    const updated = await productApi.update(id, data)
    const idx = products.value.findIndex(p => p.id === id)
    if (idx !== -1) products.value[idx] = updated
    return updated
  }

  async function remove(id: number) {
    await productApi.delete(id)
    products.value = products.value.filter(p => p.id !== id)
  }

  return { products, loading, error, byCategory, available, fetchAll, create, update, remove }
})
