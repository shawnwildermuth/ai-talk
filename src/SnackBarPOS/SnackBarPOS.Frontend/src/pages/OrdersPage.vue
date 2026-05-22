<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { RouterLink } from 'vue-router'
import { useOrderStore } from '@/stores/orderStore'

const orderStore = useOrderStore()
const filterStatus = ref<string>('all')

onMounted(() => orderStore.fetchAll())

const filteredOrders = computed(() => {
  if (filterStatus.value === 'all') return orderStore.orders
  return orderStore.orders.filter(o => o.status.toLowerCase() === filterStatus.value)
})

function formatCurrency(amount: number) {
  return new Intl.NumberFormat('nl-NL', { style: 'currency', currency: 'EUR' }).format(amount)
}

function formatDate(dateStr: string) {
  return new Date(dateStr).toLocaleString('nl-NL', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  })
}

function statusBadge(status: string) {
  switch (status) {
    case 'Paid': return 'badge-success'
    case 'Open': return 'badge-warning'
    case 'Cancelled': return 'badge-error'
    default: return 'badge-ghost'
  }
}

function statusLabel(status: string) {
  switch (status) {
    case 'Paid': return 'Betaald'
    case 'Open': return 'Open'
    case 'Cancelled': return 'Geannuleerd'
    default: return status
  }
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold">📋 Bestellingen</h1>
      <RouterLink to="/register" class="btn btn-primary">+ Nieuwe Bestelling</RouterLink>
    </div>

    <!-- Filters -->
    <div class="flex gap-2 flex-wrap">
      <button
        v-for="{ value, label } in [
          { value: 'all', label: 'Alle' },
          { value: 'open', label: 'Open' },
          { value: 'paid', label: 'Betaald' },
          { value: 'cancelled', label: 'Geannuleerd' }
        ]"
        :key="value"
        class="btn btn-sm"
        :class="{ 'btn-primary': filterStatus === value, 'btn-ghost': filterStatus !== value }"
        @click="filterStatus = value"
      >
        {{ label }}
      </button>
    </div>

    <!-- Loading -->
    <div v-if="orderStore.loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <!-- Orders table -->
    <div v-else class="card bg-base-100 shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table">
          <thead>
            <tr>
              <th>Bestelnummer</th>
              <th>Status</th>
              <th>Items</th>
              <th>Betaalwijze</th>
              <th>Totaal</th>
              <th>Datum</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="filteredOrders.length === 0">
              <td colspan="7" class="text-center text-base-content/60 py-12">Geen bestellingen gevonden</td>
            </tr>
            <tr v-for="order in filteredOrders" :key="order.id" class="hover">
              <td class="font-mono font-bold">{{ order.orderNumber }}</td>
              <td>
                <span class="badge" :class="statusBadge(order.status)">
                  {{ statusLabel(order.status) }}
                </span>
              </td>
              <td>{{ order.items.reduce((s, i) => s + i.quantity, 0) }}</td>
              <td>
                <span v-if="order.paymentMethod">
                  {{ order.paymentMethod === 'Cash' ? '💵' : order.paymentMethod === 'Pin' ? '💳' : '📱' }}
                  {{ order.paymentMethod }}
                </span>
                <span v-else class="text-base-content/40">—</span>
              </td>
              <td class="font-bold">{{ formatCurrency(order.totalAmount) }}</td>
              <td class="text-sm text-base-content/70">{{ formatDate(order.createdAt) }}</td>
              <td>
                <RouterLink :to="`/orders/${order.id}`" class="btn btn-xs btn-ghost">
                  Details →
                </RouterLink>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
