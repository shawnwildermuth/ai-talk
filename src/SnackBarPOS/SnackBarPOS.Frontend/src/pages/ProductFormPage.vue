<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter, RouterLink } from 'vue-router'
import { useProductStore } from '@/stores/productStore'
import { useCategoryStore } from '@/stores/categoryStore'

const route = useRoute()
const router = useRouter()
const productStore = useProductStore()
const categoryStore = useCategoryStore()

const isEdit = computed(() => !!route.params.id)
const productId = computed(() => isEdit.value ? Number(route.params.id) : null)

const form = ref({
  name: '',
  description: '',
  price: 0,
  categoryId: 0,
  imageUrl: '',
  isActive: true,
  isAvailable: true
})

const error = ref<string | null>(null)
const saving = ref(false)

onMounted(async () => {
  await categoryStore.fetchAll()
  if (categoryStore.categories.length > 0 && !form.value.categoryId) {
    form.value.categoryId = categoryStore.categories[0].id
  }

  if (isEdit.value && productId.value) {
    await productStore.fetchAll(true)
    const product = productStore.products.find(p => p.id === productId.value)
    if (product) {
      form.value = {
        name: product.name,
        description: product.description ?? '',
        price: product.price,
        categoryId: product.categoryId,
        imageUrl: product.imageUrl ?? '',
        isActive: product.isActive,
        isAvailable: product.isAvailable
      }
    }
  }
})

async function handleSubmit() {
  saving.value = true
  error.value = null
  try {
    if (isEdit.value && productId.value) {
      await productStore.update(productId.value, {
        ...form.value,
        description: form.value.description || undefined,
        imageUrl: form.value.imageUrl || undefined
      })
    } else {
      await productStore.create({
        name: form.value.name,
        description: form.value.description || undefined,
        price: form.value.price,
        categoryId: form.value.categoryId,
        imageUrl: form.value.imageUrl || undefined
      })
    }
    router.push('/products')
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : 'Er is een fout opgetreden'
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <div class="max-w-xl mx-auto space-y-6">
    <div class="flex items-center gap-4">
      <RouterLink to="/products" class="btn btn-ghost btn-sm">← Terug</RouterLink>
      <h1 class="text-2xl font-bold">{{ isEdit ? 'Product Bewerken' : 'Nieuw Product' }}</h1>
    </div>

    <div class="card bg-base-100 shadow-md">
      <div class="card-body">
        <form @submit.prevent="handleSubmit" class="space-y-4">
          <div class="alert alert-error" v-if="error">{{ error }}</div>

          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Naam *</span></label>
            <input v-model="form.name" type="text" required class="input input-bordered" placeholder="Bijv. Kroket" />
          </div>

          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Beschrijving</span></label>
            <textarea v-model="form.description" class="textarea textarea-bordered" rows="2" placeholder="Korte beschrijving..." />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <div class="form-control">
              <label class="label"><span class="label-text font-semibold">Prijs (€) *</span></label>
              <input
                v-model.number="form.price"
                type="number"
                step="0.05"
                min="0.05"
                required
                class="input input-bordered"
              />
            </div>

            <div class="form-control">
              <label class="label"><span class="label-text font-semibold">Categorie *</span></label>
              <select v-model="form.categoryId" required class="select select-bordered">
                <option v-for="cat in categoryStore.categories" :key="cat.id" :value="cat.id">
                  {{ cat.iconEmoji }} {{ cat.name }}
                </option>
              </select>
            </div>
          </div>

          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Afbeelding URL</span></label>
            <input v-model="form.imageUrl" type="url" class="input input-bordered" placeholder="https://..." />
          </div>

          <div v-if="isEdit" class="flex gap-6">
            <label class="flex items-center gap-2 cursor-pointer">
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-success" />
              <span class="label-text">Actief</span>
            </label>
            <label class="flex items-center gap-2 cursor-pointer">
              <input v-model="form.isAvailable" type="checkbox" class="toggle toggle-primary" />
              <span class="label-text">Beschikbaar</span>
            </label>
          </div>

          <div class="flex gap-3 pt-2">
            <RouterLink to="/products" class="btn btn-ghost flex-1">Annuleer</RouterLink>
            <button type="submit" class="btn btn-primary flex-1" :disabled="saving">
              {{ saving ? 'Opslaan...' : (isEdit ? 'Bijwerken' : 'Aanmaken') }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
