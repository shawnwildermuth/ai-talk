import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { orderApi } from '@/services/api'
import type { Order, DailySummary, AddOrderItemRequest, PayOrderRequest } from '@/types'

export const useOrderStore = defineStore('orders', () => {
  const currentOrder = ref<Order | null>(null)
  const orders = ref<Order[]>([])
  const todaySummary = ref<DailySummary | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const itemCount = computed(() =>
    currentOrder.value?.items.reduce((sum, i) => sum + i.quantity, 0) ?? 0
  )

  const totalAmount = computed(() => currentOrder.value?.totalAmount ?? 0)

  async function fetchCurrent() {
    try {
      currentOrder.value = await orderApi.getCurrent()
    } catch {
      currentOrder.value = null
    }
  }

  async function fetchAll(from?: string, to?: string) {
    loading.value = true
    error.value = null
    try {
      orders.value = await orderApi.getAll(from, to)
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Fout bij laden bestellingen'
    } finally {
      loading.value = false
    }
  }

  async function fetchTodaySummary() {
    try {
      todaySummary.value = await orderApi.getTodaySummary()
    } catch {
      todaySummary.value = null
    }
  }

  async function startNewOrder() {
    loading.value = true
    error.value = null
    try {
      currentOrder.value = await orderApi.create()
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Fout bij aanmaken bestelling'
      throw e
    } finally {
      loading.value = false
    }
  }

  async function addItem(request: AddOrderItemRequest) {
    if (!currentOrder.value) throw new Error('Geen actieve bestelling')
    currentOrder.value = await orderApi.addItem(currentOrder.value.id, request)
  }

  async function updateItemQuantity(itemId: number, quantity: number) {
    if (!currentOrder.value) throw new Error('Geen actieve bestelling')
    currentOrder.value = await orderApi.updateItem(currentOrder.value.id, itemId, quantity)
  }

  async function removeItem(itemId: number) {
    if (!currentOrder.value) throw new Error('Geen actieve bestelling')
    currentOrder.value = await orderApi.removeItem(currentOrder.value.id, itemId)
  }

  async function payOrder(request: PayOrderRequest) {
    if (!currentOrder.value) throw new Error('Geen actieve bestelling')
    const paid = await orderApi.pay(currentOrder.value.id, request)
    orders.value.unshift(paid)
    currentOrder.value = null
    await fetchTodaySummary()
    return paid
  }

  async function cancelOrder() {
    if (!currentOrder.value) throw new Error('Geen actieve bestelling')
    await orderApi.cancel(currentOrder.value.id)
    currentOrder.value = null
  }

  return {
    currentOrder, orders, todaySummary, loading, error,
    itemCount, totalAmount,
    fetchCurrent, fetchAll, fetchTodaySummary,
    startNewOrder, addItem, updateItemQuantity, removeItem,
    payOrder, cancelOrder
  }
})
