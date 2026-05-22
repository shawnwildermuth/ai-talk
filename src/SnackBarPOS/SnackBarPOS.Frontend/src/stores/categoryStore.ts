import { defineStore } from 'pinia'
import { ref } from 'vue'
import { categoryApi } from '@/services/api'
import type { Category, CreateCategoryRequest, UpdateCategoryRequest } from '@/types'

export const useCategoryStore = defineStore('categories', () => {
  const categories = ref<Category[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      categories.value = await categoryApi.getAll()
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Fout bij laden categorieën'
    } finally {
      loading.value = false
    }
  }

  async function create(data: CreateCategoryRequest) {
    const created = await categoryApi.create(data)
    categories.value.push(created)
    return created
  }

  async function update(id: number, data: UpdateCategoryRequest) {
    const updated = await categoryApi.update(id, data)
    const idx = categories.value.findIndex(c => c.id === id)
    if (idx !== -1) categories.value[idx] = updated
    return updated
  }

  async function remove(id: number) {
    await categoryApi.delete(id)
    categories.value = categories.value.filter(c => c.id !== id)
  }

  return { categories, loading, error, fetchAll, create, update, remove }
})
