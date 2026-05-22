<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import { orderApi } from '@/services/api'
import type { Order } from '@/types'

const route = useRoute()
const order = ref<Order | null>(null)
const loading = ref(true)

onMounted(async () => {
  try {
    order.value = await orderApi.getById(Number(route.params.id))
  } finally {
    loading.value = false
  }
})

function formatCurrency(amount: number) {
  return new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(amount)
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleString('nl-NL', {
    weekday: 'long', day: '2-digit', month: 'long', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  })
}

function statusBadge(status: string) {
  return { 'Paid': 'badge-success', 'Open': 'badge-warning', 'Cancelled': 'badge-error' }[status] ?? 'badge-ghost'
}
</script>

<template>
  <div class="max-w-2xl mx-auto space-y-6">
    <div class="flex items-center gap-4">
      <RouterLink to="/orders" class="btn btn-ghost btn-sm">← Terug</RouterLink>
      <h1 class="text-2xl font-bold">Bestelling Details</h1>
    </div>

    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <template v-else-if="order">
      <!-- Order header -->
      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <div class="flex justify-between items-start">
            <div>
              <p class="text-3xl font-mono font-bold">{{ order.orderNumber }}</p>
              <p class="text-base-content/60">{{ formatDate(order.createdAt) }}</p>
            </div>
            <span class="badge badge-lg" :class="statusBadge(order.status)">
              {{ order.status === 'Paid' ? 'Betaald' : order.status === 'Open' ? 'Open' : 'Geannuleerd' }}
            </span>
          </div>

          <div class="divider"></div>

          <div class="grid grid-cols-2 gap-4 text-sm">
            <div v-if="order.paymentMethod">
              <p class="text-base-content/60">Betaalwijze</p>
              <p class="font-semibold">
                {{ order.paymentMethod === 'Cash' ? '💵' : order.paymentMethod === 'Pin' ? '💳' : '📱' }}
                {{ order.paymentMethod }}
              </p>
            </div>
            <div v-if="order.paidAt">
              <p class="text-base-content/60">Betaald om</p>
              <p class="font-semibold">{{ new Date(order.paidAt).toLocaleTimeString('nl-NL') }}</p>
            </div>
            <div v-if="order.amountTendered">
              <p class="text-base-content/60">Ontvangen</p>
              <p class="font-semibold">{{ formatCurrency(order.amountTendered) }}</p>
            </div>
            <div v-if="order.change">
              <p class="text-base-content/60">Wisselgeld</p>
              <p class="font-semibold text-success">{{ formatCurrency(order.change) }}</p>
            </div>
            <div v-if="order.notes" class="col-span-2">
              <p class="text-base-content/60">Notitie</p>
              <p class="font-semibold">{{ order.notes }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Order items -->
      <div class="card bg-base-100 shadow-md">
        <div class="card-body">
          <h2 class="card-title">Bestelde Items</h2>
          <table class="table">
            <thead>
              <tr>
                <th>Product</th>
                <th class="text-right">Prijs</th>
                <th class="text-center">Aantal</th>
                <th class="text-right">Totaal</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in order.items" :key="item.id">
                <td>{{ item.productName }}</td>
                <td class="text-right">{{ formatCurrency(item.unitPrice) }}</td>
                <td class="text-center">{{ item.quantity }}</td>
                <td class="text-right font-bold">{{ formatCurrency(item.lineTotal) }}</td>
              </tr>
            </tbody>
            <tfoot>
              <tr>
                <td colspan="3" class="text-right font-bold text-lg">Totaal:</td>
                <td class="text-right font-bold text-lg text-success">{{ formatCurrency(order.totalAmount) }}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
    </template>

    <div v-else class="alert alert-error">
      <p>Bestelling niet gevonden</p>
    </div>
  </div>
</template>
