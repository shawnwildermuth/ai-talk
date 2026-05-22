import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { BakeryItem, Order, OrderItem } from '@/types/bakery'
import { bakeryApi } from '@/services/bakeryService'

export const useBakeryStore = defineStore('bakery', () => {
  const items = ref<BakeryItem[]>([])
  const orders = ref<Order[]>([])
  const cart = ref<OrderItem[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const cartTotal = computed(() => {
    return cart.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
  })

  const cartItemCount = computed(() => {
    return cart.value.reduce((sum, item) => sum + item.quantity, 0)
  })

  // Bakery Items
  async function fetchItems() {
    loading.value = true
    error.value = null
    try {
      items.value = await bakeryApi.getAllItems()
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to fetch items'
    } finally {
      loading.value = false
    }
  }

  async function fetchItemsByCategory(category: string) {
    loading.value = true
    error.value = null
    try {
      items.value = await bakeryApi.getItemsByCategory(category)
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to fetch items'
    } finally {
      loading.value = false
    }
  }

  // Cart Management
  function addToCart(item: BakeryItem, quantity: number = 1) {
    const existingItem = cart.value.find((i) => i.bakeryItemId === item.id)
    if (existingItem) {
      existingItem.quantity += quantity
    } else {
      cart.value.push({
        bakeryItemId: item.id,
        itemName: item.name,
        quantity,
        price: item.price
      })
    }
  }

  function removeFromCart(bakeryItemId: number) {
    const index = cart.value.findIndex((i) => i.bakeryItemId === bakeryItemId)
    if (index > -1) {
      cart.value.splice(index, 1)
    }
  }

  function updateCartItemQuantity(bakeryItemId: number, quantity: number) {
    const item = cart.value.find((i) => i.bakeryItemId === bakeryItemId)
    if (item) {
      if (quantity <= 0) {
        removeFromCart(bakeryItemId)
      } else {
        item.quantity = quantity
      }
    }
  }

  function clearCart() {
    cart.value = []
  }

  // Orders
  async function fetchOrders() {
    loading.value = true
    error.value = null
    try {
      orders.value = await bakeryApi.getAllOrders()
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to fetch orders'
    } finally {
      loading.value = false
    }
  }

  async function createOrder(customerName: string) {
    loading.value = true
    error.value = null
    try {
      const order: Order = {
        customerName,
        items: cart.value
      }
      const createdOrder = await bakeryApi.createOrder(order)
      orders.value.push(createdOrder)
      clearCart()
      return createdOrder
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to create order'
      throw e
    } finally {
      loading.value = false
    }
  }

  return {
    items,
    orders,
    cart,
    loading,
    error,
    cartTotal,
    cartItemCount,
    fetchItems,
    fetchItemsByCategory,
    addToCart,
    removeFromCart,
    updateCartItemQuantity,
    clearCart,
    fetchOrders,
    createOrder
  }
})
