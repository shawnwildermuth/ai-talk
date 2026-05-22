<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCategoryStore } from '@/stores/categoryStore'
import type { CreateCategoryRequest, UpdateCategoryRequest } from '@/types'

const categoryStore = useCategoryStore()

const showModal = ref(false)
const editingId = ref<number | null>(null)
const form = ref({ name: '', description: '', iconEmoji: '', sortOrder: 0, isActive: true })
const saving = ref(false)

onMounted(() => categoryStore.fetchAll())

function openNew() {
  editingId.value = null
  form.value = { name: '', description: '', iconEmoji: '', sortOrder: categoryStore.categories.length, isActive: true }
  showModal.value = true
}

function openEdit(id: number) {
  const cat = categoryStore.categories.find(c => c.id === id)
  if (!cat) return
  editingId.value = id
  form.value = {
    name: cat.name,
    description: cat.description ?? '',
    iconEmoji: cat.iconEmoji ?? '',
    sortOrder: cat.sortOrder,
    isActive: cat.isActive
  }
  showModal.value = true
}

async function handleSubmit() {
  saving.value = true
  try {
    if (editingId.value) {
      await categoryStore.update(editingId.value, form.value as UpdateCategoryRequest)
    } else {
      await categoryStore.create({
        name: form.value.name,
        description: form.value.description || undefined,
        iconEmoji: form.value.iconEmoji || undefined,
        sortOrder: form.value.sortOrder
      } as CreateCategoryRequest)
    }
    showModal.value = false
  } finally {
    saving.value = false
  }
}

async function handleDelete(id: number) {
  if (confirm('Weet u zeker dat u deze categorie wilt verwijderen?')) {
    await categoryStore.remove(id)
  }
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold">🏷️ Categorieën</h1>
      <button class="btn btn-primary" @click="openNew">+ Nieuwe Categorie</button>
    </div>

    <div v-if="categoryStore.loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      <div
        v-for="cat in categoryStore.categories"
        :key="cat.id"
        class="card bg-base-100 shadow-md"
        :class="{ 'opacity-60': !cat.isActive }"
      >
        <div class="card-body">
          <div class="flex items-start justify-between">
            <div class="flex items-center gap-3">
              <span class="text-3xl">{{ cat.iconEmoji ?? '📦' }}</span>
              <div>
                <h3 class="font-bold text-lg">{{ cat.name }}</h3>
                <p v-if="cat.description" class="text-sm text-base-content/60">{{ cat.description }}</p>
              </div>
            </div>
            <span class="badge" :class="cat.isActive ? 'badge-success' : 'badge-ghost'">
              {{ cat.isActive ? 'Actief' : 'Inactief' }}
            </span>
          </div>
          <div class="flex justify-between items-center mt-2 text-sm text-base-content/60">
            <span>{{ cat.productCount }} producten</span>
            <span>Volgorde: {{ cat.sortOrder }}</span>
          </div>
          <div class="card-actions justify-end mt-2">
            <button class="btn btn-sm btn-ghost" @click="openEdit(cat.id)">✏️ Bewerken</button>
            <button class="btn btn-sm btn-ghost text-error" @click="handleDelete(cat.id)">🗑️</button>
          </div>
        </div>
      </div>

      <div v-if="categoryStore.categories.length === 0" class="col-span-full text-center text-base-content/60 py-12">
        Geen categorieën gevonden
      </div>
    </div>

    <!-- Modal -->
    <dialog class="modal" :class="{ 'modal-open': showModal }">
      <div class="modal-box">
        <h3 class="font-bold text-lg mb-4">{{ editingId ? 'Categorie Bewerken' : 'Nieuwe Categorie' }}</h3>
        <form @submit.prevent="handleSubmit" class="space-y-4">
          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Naam *</span></label>
            <input v-model="form.name" type="text" required class="input input-bordered" placeholder="Bijv. Snacks" />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <div class="form-control">
              <label class="label"><span class="label-text font-semibold">Emoji icoon</span></label>
              <input v-model="form.iconEmoji" type="text" class="input input-bordered" placeholder="🍟" maxlength="4" />
            </div>
            <div class="form-control">
              <label class="label"><span class="label-text font-semibold">Sorteervolgorde</span></label>
              <input v-model.number="form.sortOrder" type="number" min="0" class="input input-bordered" />
            </div>
          </div>

          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Beschrijving</span></label>
            <input v-model="form.description" type="text" class="input input-bordered" placeholder="Optionele beschrijving..." />
          </div>

          <div v-if="editingId" class="form-control">
            <label class="flex items-center gap-2 cursor-pointer">
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-success" />
              <span class="label-text">Actief</span>
            </label>
          </div>

          <div class="modal-action">
            <button type="button" class="btn btn-ghost" @click="showModal = false">Annuleer</button>
            <button type="submit" class="btn btn-primary" :disabled="saving">
              {{ saving ? 'Opslaan...' : 'Opslaan' }}
            </button>
          </div>
        </form>
      </div>
    </dialog>
  </div>
</template>
