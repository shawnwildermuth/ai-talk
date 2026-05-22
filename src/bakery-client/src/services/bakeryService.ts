import type { BakeryItem, Order } from '@/types/bakery'

const API_BASE_URL = 'https://localhost:7214/api'

export const bakeryApi = {
  // Bakery Items
  async getAllItems(): Promise<BakeryItem[]> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems`)
    if (!response.ok) throw new Error('Failed to fetch items')
    return response.json()
  },

  async getItemById(id: number): Promise<BakeryItem> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems/${id}`)
    if (!response.ok) throw new Error('Failed to fetch item')
    return response.json()
  },

  async getItemsByCategory(category: string): Promise<BakeryItem[]> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems/category/${category}`)
    if (!response.ok) throw new Error('Failed to fetch items')
    return response.json()
  },

  async createItem(item: Omit<BakeryItem, 'id'>): Promise<BakeryItem> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(item)
    })
    if (!response.ok) throw new Error('Failed to create item')
    return response.json()
  },

  async updateItem(id: number, item: BakeryItem): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(item)
    })
    if (!response.ok) throw new Error('Failed to update item')
  },

  async deleteItem(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/bakeryitems/${id}`, {
      method: 'DELETE'
    })
    if (!response.ok) throw new Error('Failed to delete item')
  },

  // Orders
  async getAllOrders(): Promise<Order[]> {
    const response = await fetch(`${API_BASE_URL}/orders`)
    if (!response.ok) throw new Error('Failed to fetch orders')
    return response.json()
  },

  async getOrderById(id: number): Promise<Order> {
    const response = await fetch(`${API_BASE_URL}/orders/${id}`)
    if (!response.ok) throw new Error('Failed to fetch order')
    return response.json()
  },

  async createOrder(order: Order): Promise<Order> {
    const response = await fetch(`${API_BASE_URL}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(order)
    })
    if (!response.ok) throw new Error('Failed to create order')
    return response.json()
  },

  async updateOrderStatus(id: number, status: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/orders/${id}/status`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(status)
    })
    if (!response.ok) throw new Error('Failed to update order status')
  },

  async deleteOrder(id: number): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/orders/${id}`, {
      method: 'DELETE'
    })
    if (!response.ok) throw new Error('Failed to delete order')
  }
}
