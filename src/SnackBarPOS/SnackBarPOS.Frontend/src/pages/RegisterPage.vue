<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useOrderStore } from '@/stores/orderStore'
import { useProductStore } from '@/stores/productStore'
import { useCategoryStore } from '@/stores/categoryStore'
import type { PaymentMethod } from '@/types'

const orderStore = useOrderStore()
const productStore = useProductStore()
const categoryStore = useCategoryStore()

const selectedCategory = ref<number | null>(null)
const showPayModal = ref(false)
const paymentMethod = ref<PaymentMethod>('Pin')
const amountTendered = ref<number | undefined>(undefined)
const notes = ref('')
const processing = ref(false)
const lastPaidOrder = ref<{ orderNumber: string; change?: number } | null>(null)

onMounted(async () => {
  await Promise.all([
    productStore.fetchAll(),
    categoryStore.fetchAll(),
    orderStore.fetchCurrent()
  ])
  if (categoryStore.categories.length > 0) {
    selectedCategory.value = categoryStore.categories[0].id
  }
})

const filteredProducts = computed(() => {
  if (!selectedCategory.value) return productStore.available
  return productStore.available.filter(p => p.categoryId === selectedCategory.value)
})

const change = computed(() => {
  if (paymentMethod.value !== 'Cash' || !amountTendered.value) return undefined
  return Math.max(0, amountTendered.value - orderStore.totalAmount)
})

function formatCurrency(amount: number) {
  return new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(amount)
}

async function handleStartOrder() {
  await orderStore.startNewOrder()
}

async function handleAddToOrder(productId: number) {
  if (!orderStore.currentOrder) {
    await orderStore.startNewOrder()
  }
  await orderStore.addItem({ productId, quantity: 1 })
}

async function handleUpdateQty(itemId: number, qty: number) {
  await orderStore.updateItemQuantity(itemId, qty)
}

async function handleRemoveItem(itemId: number) {
  await orderStore.removeItem(itemId)
}

async function handlePay() {
  processing.value = true
  try {
    const paid = await orderStore.payOrder({
      paymentMethod: paymentMethod.value,
      amountTendered: paymentMethod.value === 'Cash' ? amountTendered.value : undefined,
      notes: notes.value || undefined
    })
    lastPaidOrder.value = {
      orderNumber: paid.orderNumber,
      change: paid.change ?? undefined
    }
    showPayModal.value = false
    paymentMethod.value = 'Pin'
    amountTendered.value = undefined
    notes.value = ''
  } catch (e) {
    console.error(e)
  } finally {
    processing.value = false
  }
}

async function handleCancel() {
  if (confirm('Weet u zeker dat u deze bestelling wilt annuleren?')) {
    await orderStore.cancelOrder()
  }
}
</script>

<template>
  <div class="grid grid-cols-1 xl:grid-cols-3 gap-6 h-full">
    <!-- Product grid (left 2/3) -->
    <div class="xl:col-span-2 space-y-4">
      <div class="flex justify-between items-center">
        <h1 class="text-2xl font-bold">🛒 Kassa</h1>
      </div>

      <!-- Category tabs -->
      <div class="tabs tabs-boxed bg-base-100 flex-wrap">
        <button
          v-for="cat in categoryStore.categories"
          :key="cat.id"
          class="tab"
          :class="{ 'tab-active': selectedCategory === cat.id }"
          @click="selectedCategory = cat.id"
        >
          {{ cat.iconEmoji }} {{ cat.name }}
        </button>
      </div>

      <!-- Products -->
      <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-3">
        <button
          v-for="product in filteredProducts"
          :key="product.id"
          class="card bg-base-100 shadow hover:shadow-md hover:scale-105 transition-all cursor-pointer text-left"
          @click="handleAddToOrder(product.id)"
        >
          <div class="card-body p-3">
            <div class="text-2xl text-center mb-1">{{ product.categoryEmoji ?? '🍽️' }}</div>
            <p class="font-semibold text-sm text-center leading-tight">{{ product.name }}</p>
            <p class="text-success font-bold text-center">{{ formatCurrency(product.price) }}</p>
          </div>
        </button>
        <div v-if="filteredProducts.length === 0" class="col-span-full text-center text-base-content/60 py-12">
          Geen producten in deze categorie
        </div>
      </div>
    </div>

    <!-- Order panel (right 1/3) -->
    <div class="xl:col-span-1">
      <div class="card bg-base-100 shadow-md sticky top-20">
        <div class="card-body p-4 space-y-4">
          <div class="flex justify-between items-center">
            <h2 class="card-title text-lg">
              Bestelling
              <span v-if="orderStore.currentOrder" class="text-xs font-mono badge badge-ghost">
                {{ orderStore.currentOrder.orderNumber }}
              </span>
            </h2>
          </div>

          <!-- No order yet -->
          <div v-if="!orderStore.currentOrder" class="text-center py-8 text-base-content/60">
            <p class="text-4xl mb-3">🛒</p>
            <p>Klik op een product om te beginnen</p>
          </div>

          <!-- Order items -->
          <div v-else class="space-y-2 max-h-80 overflow-y-auto">
            <div v-if="orderStore.currentOrder.items.length === 0" class="text-center py-4 text-base-content/60">
              Geen items toegevoegd
            </div>
            <div
              v-for="item in orderStore.currentOrder.items"
              :key="item.id"
              class="flex items-center gap-2 p-2 bg-base-200 rounded-lg"
            >
              <div class="flex-1 min-w-0">
                <p class="font-medium text-sm truncate">{{ item.productName }}</p>
                <p class="text-xs text-base-content/60">{{ formatCurrency(item.unitPrice) }}</p>
              </div>
              <div class="flex items-center gap-1">
                <button class="btn btn-xs btn-ghost" @click="handleUpdateQty(item.id, item.quantity - 1)">−</button>
                <span class="w-6 text-center text-sm font-bold">{{ item.quantity }}</span>
                <button class="btn btn-xs btn-ghost" @click="handleUpdateQty(item.id, item.quantity + 1)">+</button>
              </div>
              <div class="text-right">
                <p class="text-sm font-bold">{{ formatCurrency(item.lineTotal) }}</p>
                <button class="btn btn-xs btn-ghost text-error" @click="handleRemoveItem(item.id)">✕</button>
              </div>
            </div>
          </div>

          <!-- Total -->
          <div v-if="orderStore.currentOrder" class="border-t pt-3 space-y-3">
            <div class="flex justify-between text-xl font-bold">
              <span>Totaal</span>
              <span class="text-success">{{ formatCurrency(orderStore.totalAmount) }}</span>
            </div>
            <div class="grid grid-cols-2 gap-2">
              <button class="btn btn-error btn-sm" @click="handleCancel">Annuleer</button>
              <button
                class="btn btn-success btn-sm"
                :disabled="!orderStore.currentOrder.items.length"
                @click="showPayModal = true"
              >
                💳 Betalen
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Receipt notification -->
    <div v-if="lastPaidOrder" class="fixed bottom-6 right-6 z-50">
      <div class="alert alert-success shadow-xl w-80">
        <div>
          <p class="font-bold">✅ Betaald!</p>
          <p class="text-sm">Bestelling {{ lastPaidOrder.orderNumber }}</p>
          <p v-if="lastPaidOrder.change" class="text-sm">
            Wisselgeld: {{ formatCurrency(lastPaidOrder.change) }}
          </p>
        </div>
        <button class="btn btn-sm btn-ghost" @click="lastPaidOrder = null">✕</button>
      </div>
    </div>

    <!-- Payment modal -->
    <dialog class="modal" :class="{ 'modal-open': showPayModal }">
      <div class="modal-box">
        <h3 class="font-bold text-lg mb-4">💳 Betalen</h3>
        <div class="space-y-4">
          <div class="flex justify-between text-xl font-bold mb-4">
            <span>Te betalen:</span>
            <span class="text-success">{{ formatCurrency(orderStore.totalAmount) }}</span>
          </div>

          <!-- Payment method -->
          <div class="form-control">
            <label class="label"><span class="label-text font-semibold">Betaalwijze</span></label>
            <div class="grid grid-cols-3 gap-2">
              <button
                v-for="method in (['Cash', 'Pin', 'Contactless'] as PaymentMethod[])"
                :key="method"
                class="btn btn-sm"
                :class="{ 'btn-primary': paymentMethod === method, 'btn-outline': paymentMethod !== method }"
                @click="paymentMethod = method"
              >
                {{ method === 'Cash' ? '💵' : method === 'Pin' ? '💳' : '📱' }} {{ method }}
              </button>
            </div>
          </div>

          <!-- Cash amount tendered -->
          <div v-if="paymentMethod === 'Cash'" class="form-control">
            <label class="label"><span class="label-text">Ontvangen bedrag</span></label>
            <input
              v-model.number="amountTendered"
              type="number"
              step="0.05"
              min="0"
              class="input input-bordered"
              placeholder="0.00"
            />
            <div v-if="change !== undefined" class="label">
              <span class="label-text-alt text-success font-bold">Wisselgeld: {{ formatCurrency(change) }}</span>
            </div>
          </div>

          <!-- Quick cash amounts -->
          <div v-if="paymentMethod === 'Cash'" class="flex flex-wrap gap-2">
            <button
              v-for="amount in [5, 10, 20, 50]"
              :key="amount"
              class="btn btn-xs btn-outline"
              @click="amountTendered = amount"
            >
              €{{ amount }}
            </button>
          </div>

          <!-- Notes -->
          <div class="form-control">
            <label class="label"><span class="label-text">Notitie (optioneel)</span></label>
            <input v-model="notes" type="text" class="input input-bordered input-sm" placeholder="..." />
          </div>

          <div class="modal-action">
            <button class="btn btn-ghost" @click="showPayModal = false">Annuleer</button>
            <button
              class="btn btn-success"
              :disabled="processing || (paymentMethod === 'Cash' && (!amountTendered || amountTendered < orderStore.totalAmount))"
              @click="handlePay"
            >
              {{ processing ? 'Verwerken...' : 'Bevestig Betaling' }}
            </button>
          </div>
        </div>
      </div>
    </dialog>
  </div>
</template>
