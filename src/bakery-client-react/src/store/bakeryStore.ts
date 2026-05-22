import { create } from 'zustand'
import type { BakeryItem, Order, OrderItem } from '../types/bakery'
import { bakeryApi } from '../services/bakeryService'

interface BakeryState {
  items: BakeryItem[]
  orders: Order[]
  cart: OrderItem[]
  loading: boolean
  error: string | null

  // Computed-style helpers (derived from state)
  cartTotal: () => number
  cartItemCount: () => number

  // Actions
  fetchItems: () => Promise<void>
  fetchItemsByCategory: (category: string) => Promise<void>
  addToCart: (item: BakeryItem, quantity?: number) => void
  removeFromCart: (bakeryItemId: number) => void
  updateCartItemQuantity: (bakeryItemId: number, quantity: number) => void
  clearCart: () => void
  fetchOrders: () => Promise<void>
  createOrder: (customerName: string) => Promise<Order>
}

export const useBakeryStore = create<BakeryState>((set, get) => ({
  items: [],
  orders: [],
  cart: [],
  loading: false,
  error: null,

  cartTotal: () =>
    get().cart.reduce((sum, item) => sum + item.price * item.quantity, 0),

  cartItemCount: () =>
    get().cart.reduce((sum, item) => sum + item.quantity, 0),

  async fetchItems() {
    set({ loading: true, error: null })
    try {
      const items = await bakeryApi.getAllItems()
      set({ items })
    } catch (e) {
      set({ error: e instanceof Error ? e.message : 'Failed to fetch items' })
    } finally {
      set({ loading: false })
    }
  },

  async fetchItemsByCategory(category: string) {
    set({ loading: true, error: null })
    try {
      const items = await bakeryApi.getItemsByCategory(category)
      set({ items })
    } catch (e) {
      set({ error: e instanceof Error ? e.message : 'Failed to fetch items' })
    } finally {
      set({ loading: false })
    }
  },

  addToCart(item: BakeryItem, quantity = 1) {
    const cart = get().cart
    const existing = cart.find((i) => i.bakeryItemId === item.id)
    if (existing) {
      set({
        cart: cart.map((i) =>
          i.bakeryItemId === item.id ? { ...i, quantity: i.quantity + quantity } : i
        ),
      })
    } else {
      set({
        cart: [...cart, { bakeryItemId: item.id, itemName: item.name, quantity, price: item.price }],
      })
    }
  },

  removeFromCart(bakeryItemId: number) {
    set({ cart: get().cart.filter((i) => i.bakeryItemId !== bakeryItemId) })
  },

  updateCartItemQuantity(bakeryItemId: number, quantity: number) {
    if (quantity <= 0) {
      get().removeFromCart(bakeryItemId)
    } else {
      set({
        cart: get().cart.map((i) =>
          i.bakeryItemId === bakeryItemId ? { ...i, quantity } : i
        ),
      })
    }
  },

  clearCart() {
    set({ cart: [] })
  },

  async fetchOrders() {
    set({ loading: true, error: null })
    try {
      const orders = await bakeryApi.getAllOrders()
      set({ orders })
    } catch (e) {
      set({ error: e instanceof Error ? e.message : 'Failed to fetch orders' })
    } finally {
      set({ loading: false })
    }
  },

  async createOrder(customerName: string) {
    set({ loading: true, error: null })
    try {
      const order: Order = { customerName, items: get().cart }
      const created = await bakeryApi.createOrder(order)
      set((state) => ({ orders: [...state.orders, created] }))
      get().clearCart()
      return created
    } catch (e) {
      set({ error: e instanceof Error ? e.message : 'Failed to create order' })
      throw e
    } finally {
      set({ loading: false })
    }
  },
}))
